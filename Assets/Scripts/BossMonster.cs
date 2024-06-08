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

        // 보스가 아이들 상태가 아닐 때만 플레이어와의 거리 계산 및 공격을 체크
        if (!MonsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && playerTransform != null)
        {
            CheckAndAttackPlayer();
        }
    }

    private void BossMonsterMove()
    {
        if (isDie || MonsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

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

    private void CheckAndAttackPlayer()
    {
        if (isDie) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        Debug.Log("Distance to Player: " + distanceToPlayer); // 플레이어와의 거리 디버깅

        if (distanceToPlayer <= attackRange)
        {
            // 플레이어가 공격 범위 내에 있을 때 공격 애니메이션 실행
            Debug.Log("Player within attack range");
            MonsterAnimator.SetTrigger("Attack");
            GameManager.Instance.PlayerHP -= BossMonsterDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie) return;

        Debug.Log("Collision detected with: " + collision.gameObject.tag); // 디버그 로그 추가

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

    private IEnumerator StartRunning()
    {
        isRunning = true;
        MonsterAnimator.SetBool("Run", true); // Run 애니메이션 시작
        yield return new WaitForSeconds(20f); // 20초 동안 달리기
        isRunning = false;
        MonsterAnimator.SetBool("Run", false); // Run 애니메이션 종료
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
        int itemRandom = Random.Range(0, ItemObj.Length);
        if (itemRandom < ItemObj.Length)
        {
            Instantiate(ItemObj[itemRandom], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
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
