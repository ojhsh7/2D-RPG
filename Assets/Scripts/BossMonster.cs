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
        // 보스를 공격하는 입력을 감지
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 보스를 공격했을 때 달리기 애니메이션 실행
            MonsterAnimator.SetBool("Run", true);
        }

        // 플레이어가 달리는 입력을 감지
        Move();

        // 보스 몬스터 이동
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
        Destroy(gameObject, 1.5f); //Die 애니매이션  재생 시간 보장
    }
    private void Move()
    {
        // 이동
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