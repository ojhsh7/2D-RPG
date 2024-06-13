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
    private bool isAttacking = false; // ���� ������ ����
    private bool playerInRange = false; // �÷��̾ �ݶ��̴� �ȿ� �ִ��� ����

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

        // �÷��̾ �ݶ��̴� �ȿ� ���� ���� ���� üũ
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

        // �̵� ���߱�
        isRunning = false;
        MonsterAnimator.SetBool("Run", false); // Run �ִϸ��̼� ����

        // Attack�� Attack1�� �������� ����
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
        yield return new WaitForSeconds(1f); // ���� �ִϸ��̼� ��� �ð�

        isAttacking = false;

        // ���� �ִϸ��̼��� ���� �� �÷��̾ ������ �ݶ��̴� �ȿ� ������ �ٽ� ������ �õ�
        if (!isDie && playerInRange)
        {
            StartCoroutine(AttackWithInterval());
        }
        else if (!isDie) // �÷��̾ �ݶ��̴� �ȿ� ������ �ٽ� �̵� ����
        {
            isRunning = true;
            MonsterAnimator.SetBool("Run", true); // Run �ִϸ��̼� ����
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
        MonsterAnimator.SetBool("Run", true); // Run �ִϸ��̼� ����
        yield return new WaitForSeconds(20f); // 20�� ���� �޸���
        isRunning = false;
        MonsterAnimator.SetBool("Run", false); // Run �ִϸ��̼� ����

        // �޸��Ⱑ ����� �� �÷��̾ �ݶ��̴� �ȿ� ������ �ٽ� �޸��� ���·� ��ȯ
        if (!playerInRange)
        {
            isRunning = true;
            MonsterAnimator.SetBool("Run", true);
        }
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
        int itemCount = Random.Range(2, 6);  // 2~5���� �������� ����

        for (int i = 0; i < itemCount; i++)
        {
            int itemRandom = Random.Range(0, ItemObj.Length);
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0); // ������ ��ġ�� �����ϰ� ���ݾ� �̵�
            Instantiate(ItemObj[itemRandom], transform.position + offset, Quaternion.identity);
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
