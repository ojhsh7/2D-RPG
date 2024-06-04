using System.Collections;
using System.Collections.Generic;
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

    public GameObject[] ItemObj;

    private Animator MonsterAnimator;
    public float moveSpeed = 3f;
    public float RunSpeed = 4f;
    private Transform playerTransform; // 플레이어의 Transform을 저장할 변수

    void Start()
    {
        MonsterAnimator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어 찾기
    }

    void Update()
    {
        // 보스 몬스터 이동
        BossMonsterMove();

        // 보스가 아이들 상태가 아닐 때만 플레이어와의 거리 계산 및 공격을 체크
        if (!MonsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
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
            this.transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
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

            // 보스가 공격받을 때 Run 애니메이션을 20초 동안 실행
            StartCoroutine(RunForSeconds(20));

            if (BossMonsterHP <= 0)
            {
                BossMonsterDie();
            }
        }
    }

    private IEnumerator RunForSeconds(float seconds)
    {
        MonsterAnimator.SetBool("Run", true);
        yield return new WaitForSeconds(seconds);
        MonsterAnimator.SetBool("Run", false);
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
}
