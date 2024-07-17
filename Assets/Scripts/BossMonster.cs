using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    public float BossMonsterHP = 5000f;
    public float BossMonsterDamage = 15f;
    public float BossMonsterExp = 40;

    private float moveTime = 0;
    private float turnTime = 0;
    private bool isDie = false;

    public GameObject[] ItemObj;

    private Animator MonsterAnimator;

    public float moveSpeed = 3f;
    public float RunSpeed = 4f;
    void Start()
    {
        MonsterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // ������ �����ϴ� �Է��� ����
        if (Input.GetKeyDown(KeyCode.C))
        {
            // ������ �������� �� �޸��� �ִϸ��̼� ����
            MonsterAnimator.SetBool("Run", true);
        }

        // �÷��̾ �޸��� �Է��� ����
        Move();

        // ���� ���� �̵�
        BossMonsterMove();
    }




    private void BossMonsterMove()
    {
        if (isDie) return;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDie) return;

        if (collision.gameObject.tag == "Player")
        {
            MonsterAnimator.SetTrigger("Attack");
            GameManager.Instance.PlayerStat.HP -= BossMonsterDamage;
        }
        else if (collision.gameObject.tag == "Player")
        {
            MonsterAnimator.SetTrigger("Attack1");
            GameManager.Instance.PlayerStat.HP -= BossMonsterDamage;
        }
        if (collision.gameObject.tag == "Attack")
        {
            MonsterAnimator.SetTrigger("Damage");
            BossMonsterHP -= collision.gameObject.GetComponent<Attack>().AttackDamage;

            if (BossMonsterHP <= 0)
            {
                BossMonsterDie();
            }
        }

    }
    private void BossMonsterDie()
    {
        isDie = true;
        MonsterAnimator.SetTrigger("Die");
        GameManager.Instance.PlayerStat.Exp += BossMonsterExp;

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1.5f); //Die �ִϸ��̼�  ��� �ð� ����
    }
    private void Move()
    {
        // �̵�
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * RunSpeed * Time.deltaTime);
            MonsterAnimator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * RunSpeed * Time.deltaTime);
            MonsterAnimator.SetBool("Run", true);
        }
        else
        {
            MonsterAnimator.SetBool("Run", false);
        }
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