using System.Collections;
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
    private bool isRunning = false;

    public GameObject[] ItemObj;

    private Animator MonsterAnimator;
    public float moveSpeed = 3f;
    public float RunSpeed = 6f; // �޸��� �ӵ� ����
    private Transform playerTransform; // �÷��̾��� Transform�� ������ ����

    void Start()
    {
        MonsterAnimator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // �÷��̾� ã��

        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Player found: " + player.name); // �÷��̾ �߰ߵǾ����� �α׿� ���
        }
    }

    void Update()
    {
        // ���� ���� �̵�
        BossMonsterMove();

        // ������ ���̵� ���°� �ƴ� ���� �÷��̾���� �Ÿ� ��� �� ������ üũ
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

            // ������ �¾��� �� 20�� ���� �޸��� ����
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
        MonsterAnimator.SetBool("Run", true); // Run �ִϸ��̼� ����
        yield return new WaitForSeconds(20f); // 20�� ���� �޸���
        isRunning = false;
        MonsterAnimator.SetBool("Run", false); // Run �ִϸ��̼� ����
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
    public class BossDeath : MonoBehaviour
    {
        // ���� ���Ͱ� ���� �� ȣ��Ǵ� �Լ�
        public void Die()
        {
            // ���� �ð� �Ŀ� RemoveComponents �Լ� ȣ�� (��: 2�� ��)
            Invoke("RemoveComponents", 2.0f);
        }

        void RemoveComponents()
        {
            // Rigidbody�� Collider ����
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
