using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public float BossMonsterHP = 5000f;
    public float BossMonsterDamage = 15f;
    public float BossMonsterExp = 40;
    public float attackRange = 2.0f; // ������ ���� ���� ����

    private float moveTime = 0;
    private float turnTime = 0;
    private bool isDie = false;

    public GameObject[] ItemObj;

    private Animator MonsterAnimator;
    public float moveSpeed = 3f;
    public float RunSpeed = 4f;
    private Transform playerTransform; // �÷��̾��� Transform�� ������ ����

    void Start()
    {
        MonsterAnimator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾� ã��
    }

    void Update()
    {
        // ���� ���� �̵�
        BossMonsterMove();

        // ������ ���̵� ���°� �ƴ� ���� �÷��̾���� �Ÿ� ��� �� ������ üũ
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

        Debug.Log("Distance to Player: " + distanceToPlayer); // �÷��̾���� �Ÿ� �����

        if (distanceToPlayer <= attackRange)
        {
            // �÷��̾ ���� ���� ���� ���� �� ���� �ִϸ��̼� ����
            Debug.Log("Player within attack range");
            MonsterAnimator.SetTrigger("Attack");
            GameManager.Instance.PlayerHP -= BossMonsterDamage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie) return;

        Debug.Log("Collision detected with: " + collision.gameObject.tag); // ����� �α� �߰�

        if (collision.gameObject.CompareTag("Attack"))
        {
            Debug.Log("Taking Damage");
            MonsterAnimator.SetTrigger("Damage");
            BossMonsterHP -= collision.gameObject.GetComponent<Attack>().AttackDamage;

            // ������ �ִϸ��̼� �� Ʈ���� ����
            StartCoroutine(ResetTrigger("Damage"));

            // ������ ���ݹ��� �� Run �ִϸ��̼��� 20�� ���� ����
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
        yield return new WaitForSeconds(0.1f); // ��� ���
        MonsterAnimator.ResetTrigger(triggerName); // Ʈ���� ����
    }

    private void BossMonsterDie()
    {
        isDie = true;
        MonsterAnimator.SetTrigger("Die");
        GameManager.Instance.PlayerExp += BossMonsterExp;

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1.5f); // Die �ִϸ��̼� ��� �ð� ����
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
