using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Monster : MonoBehaviour
{
    public float MonsterHP = 30f;
    public float MonsterDamage = 2f;
    public float MonsterExp = 3;

    private float moveTime = 0;
    private float turnTime = 0;
    private bool isDie = false;

    public float moveSpeed = 3f;
    public GameObject[] ItemObj; 

    private Animator MonsterAnimator;

    void Start()
    {
        MonsterAnimator = this.GetComponent<Animator>();
       
        {
            // 몬스터가 생성될 때 MonsterManager에 추가합니다.
            MonsterManager.Instance.AddMonster(gameObject);
        }

    }

    void Update()
    {
        MonsterMove();
    }
    private void MonsterMove()
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
            GameManager.Instance.PlayerHP -= MonsterDamage;
        }
        else if (collision.gameObject.tag == "Player")
        {
            MonsterAnimator.SetTrigger("Attack1");
            GameManager.Instance.PlayerHP -= MonsterDamage;
        }
        if (collision.gameObject.tag == "Attack")
        {
            MonsterAnimator.SetTrigger("Damage");
            MonsterHP -= collision.gameObject.GetComponent<Attack>().AttackDamage;

            if(MonsterHP <= 0)
            {
                MonsterDie();
            }
        }
    }
    private void MonsterDie()
    {
        isDie = true;
        MonsterAnimator.SetTrigger("Die");
        GameManager.Instance.PlayerExp += MonsterExp;

        GetComponent<Collider2D>().enabled = false;
        Invoke("CreateItem", 1.5f); //Die 애니매이션  재생 시간 보장
    }
    private void CreateItem()
    {
        int itemRandom = Random.Range(0, ItemObj.Length);
        if (itemRandom < ItemObj.Length)
        {
            Instantiate(ItemObj[itemRandom], new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        // 몬스터가 파괴될 때 MonsterManager에서 제거합니다.
        MonsterManager.Instance.RemoveMonster(gameObject);
    }
}
