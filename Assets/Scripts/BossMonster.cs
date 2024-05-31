using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : MonoBehaviour
{
    private Animator MonsterAnimator;

    public float BossMonsterHP = 5000f;
    public float BossMonsterDamage = 15f;
    public float BossMonsterExp = 40;

    private float moveTime = 0;
    private float turnTime = 0;
    private bool isDie = false;

    private Animator BossMonsterAnimator;

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
            BossMonsterAnimator.SetTrigger("Attack");
            GameManager.Instance.PlayerHP -= BossMonsterDamage;
        }
        if (collision.gameObject.tag == "Attack")
        {
            BossMonsterAnimator.SetTrigger("Damage");
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
        BossMonsterAnimator.SetTrigger("Die");
        GameManager.Instance.PlayerExp += BossMonsterExp;

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
}