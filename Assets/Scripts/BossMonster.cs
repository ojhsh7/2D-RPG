using System.Collections;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public float BossMonsterHP = 5000f;
    public float BossMonsterDamage = 15f;
    public float BossMonsterExp = 40;
    public float attackRange = 2.0f; // 보스의 공격 범위 설정

    private float moveTime = 0;
    private float turnTime = 0;
    private bool isDie = false;
    private bool isRunning = false;
    private bool isAttacking = false; // 공격 중인지 여부
    private bool playerInRange = false; // 플레이어가 콜라이더 안에 있는지 여부

    public GameObject[] ItemObj;

    private Animator MonsterAnimator;
    public float moveSpeed = 3f;
    public float RunSpeed = 6f; // 달리기 속도 증가
    private Transform playerTransform; // 플레이어의 Transform을 저장할 변수

    void Start()
    {
        MonsterAnimator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기

        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Player found: " + player.name); // 플레이어가 발견되었음을 로그에 출력
        }
    }

    void Update()
    {
        // 보스 몬스터 이동
        BossMonsterMove();

        // 플레이어가 콜라이더 안에 있을 때만 공격 체크
        if (playerInRange && !isAttacking)
        {
            StartCoroutine(AttackWithInterval());
        }
    }

    private void BossMonsterMove()
    {
        if (isDie || isAttacking) return;

        moveTime += Time.deltaTime;

        if (moveTime <= turnTime)
        {
            this.transform.Translate((isRunning ? RunSpeed : moveSpeed) * Time.deltaTime, 0, 0);
        }
        else
        {
            turnTime = Random.Range(1, 5);
            moveTime = 0;
            transform.Rotate(0, 180, 0);
        }
    }

    private IEnumerator AttackWithInterval()
    {
        isAttacking = true;

        // 이동 멈추기
        isRunning = false;
        MonsterAnimator.SetBool("Run", false); // Run 애니메이션 종료

        // Attack과 Attack1을 랜덤으로 선택
        if (Random.Range(0, 2) == 0)
        {
            MonsterAnimator.SetTrigger("Attack");
            Debug.Log("Attack executed");
        }
        else
        {
            MonsterAnimator.SetTrigger("Attack1");
            Debug.Log("Attack1 executed");
        }

        GameManager.Instance.PlayerHP -= BossMonsterDamage;
        yield return new WaitForSeconds(1f); // 공격 애니메이션 재생 시간

        isAttacking = false;

        // 공격 애니메이션이 끝난 후 플레이어가 여전히 콜라이더 안에 있으면 다시 공격을 시도
        if (!isDie && playerInRange)
        {
            StartCoroutine(AttackWithInterval());
        }
        else if (!isDie) // 플레이어가 콜라이더 안에 없으면 다시 이동 시작
        {
            isRunning = true;
            MonsterAnimator.SetBool("Run", true); // Run 애니메이션 시작
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered attack range");
        }

        if (collision.gameObject.CompareTag("Attack"))
        {
            Debug.Log("Taking Damage");
            MonsterAnimator.SetTrigger("Damage");
            BossMonsterHP -= collision.gameObject.GetComponent<Attack>().AttackDamage;

            // 데미지 애니메이션 후 트리거 리셋
            StartCoroutine(ResetTrigger("Damage"));

            // 공격을 맞았을 때 20초 동안 달리기 시작
            StartCoroutine(StartRunning());

            if (BossMonsterHP <= 0)
            {
                BossMonsterDie();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player exited attack range");
        }
    }

    private IEnumerator StartRunning()
    {
        isRunning = true;
        MonsterAnimator.SetBool("Run", true); // Run 애니메이션 시작
        yield return new WaitForSeconds(20f); // 20초 동안 달리기
        isRunning = false;
        MonsterAnimator.SetBool("Run", false); // Run 애니메이션 종료

        // 달리기가 종료된 후 플레이어가 콜라이더 안에 없으면 다시 달리기 상태로 전환
        if (!playerInRange)
        {
            isRunning = true;
            MonsterAnimator.SetBool("Run", true);
        }
    }

    private IEnumerator ResetTrigger(string triggerName)
    {
        yield return new WaitForSeconds(0.1f); // 잠시 대기
        MonsterAnimator.ResetTrigger(triggerName); // 트리거 리셋
    }

    private void BossMonsterDie()
    {
        isDie = true;
        MonsterAnimator.SetTrigger("Die");
        GameManager.Instance.PlayerExp += BossMonsterExp;

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1.5f); // Die 애니메이션 재생 시간 보장
    }

    private void OnDestroy()
    {
        int itemCount = Random.Range(2, 6);  // 2~5개의 아이템을 생성

        for (int i = 0; i < itemCount; i++)
        {
            int itemRandom = Random.Range(0, ItemObj.Length);
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0); // 아이템 위치를 랜덤하게 조금씩 이동
            Instantiate(ItemObj[itemRandom], transform.position + offset, Quaternion.identity);
        }
    }

    public class BossDeath : MonoBehaviour
    {
        // 보스 몬스터가 죽을 때 호출되는 함수
        public void Die()
        {
            // 일정 시간 후에 RemoveComponents 함수 호출 (예: 2초 후)
            Invoke("RemoveComponents", 2.0f);
        }

        void RemoveComponents()
        {
            // Rigidbody와 Collider 제거
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
