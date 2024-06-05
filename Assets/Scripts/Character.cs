using System;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private AudioSource audioSource;

    public GameObject AttackObj;
    public float AttackSpeed = 3f;


    public AudioClip JumpClip;
    public AudioClip AttackClip;
    public float Speed = 4f;
    public float JumpPower = 6f;

    private bool isFloor;
    private bool isLadder;
    private bool isClimbing;
    private float inputVertical;
    private bool justJump, justAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();
        JumpCheck();
        AttackCheck();
        ClimbingCheck();
    }

    private void FixedUpdate()
    {
        Jump();
        Attack();
        Climbing();
    }
    private void Move()
    {
        //이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
            animator.SetBool("Move", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        //좌우 반전에 따른 이동
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isFloor = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isFloor = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = true;
            Debug.Log("isLadder: " + isLadder);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = false;
            isClimbing = false;
            Debug.Log("isLadder: " + isLadder);
        }
    }

    private void JumpCheck()
    {
        if (isFloor)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                justJump = true;
            }
        }
    }
    private void Jump()
    {
        if (justJump)
        {

            {
                justJump = false;

                rigidbody2d.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                audioSource.PlayOneShot(JumpClip);
            }
        }
    }
    private void Attack()
    {
        if (justAttack)

        {
            justAttack = false;
            animator.SetTrigger("Attack");
            audioSource.PlayOneShot(AttackClip);
            if (gameObject.name == "Warrior2(Clone)")
            {
                AttackObj.SetActive(true);
                Invoke("SetAttackObjInactive", 0.5f);
            }
            else
                justAttack = false;
            animator.SetTrigger("Attack");

            if (spriteRenderer.flipX)
            {
                GameObject obj = Instantiate(AttackObj, transform.position, Quaternion.Euler(0f, 180f, 0f));
                obj.GetComponent<Rigidbody2D>().AddForce(Vector2.left * AttackSpeed, ForceMode2D.Impulse);
                Destroy(obj, 3f);
            }
            else
            {
                GameObject obj = Instantiate(AttackObj, transform.position, Quaternion.Euler(0, 0, 0));
                obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * AttackSpeed, ForceMode2D.Impulse);
                Destroy(obj, 3f);
            }
        }
    }
    private void AttackCheck()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            justAttack = true;
        }
    }
    private void ClimbingCheck()
    {
        inputVertical = Input.GetAxis("Vertical");
        if (isLadder && Math.Abs(inputVertical) > 0)
        {
            isClimbing = true;
            Debug.Log("isClimbing : " + isClimbing);
        }
    }
    private void Climbing()
    {
        if (isClimbing)
        {
            rigidbody2d.gravityScale = 0f;
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, inputVertical * Speed);
        }
        else
        {
            rigidbody2d.gravityScale = 1f;
        }
    }
}




