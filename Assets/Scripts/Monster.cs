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
    public AudioClip dieSound; // Add this line

    private Animator MonsterAnimator;
    private AudioSource audioSource; // Add this line

    void Start()
    {
        MonsterAnimator = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>(); // Add this line
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

            if (MonsterHP <= 0)
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

        // Play die sound
        audioSource.PlayOneShot(dieSound); // Add this line

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 3f); //Die 애니매이션  재생 시간 보장
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
