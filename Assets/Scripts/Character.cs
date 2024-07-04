using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private AudioSource audioSource;

    public GameObject AttackObj;
    public GameObject CrossAttackObj;
    public float AttackSpeed = 3f;
    public float MaxAttackSpeed = 6f;
    public float HoldDuration = 3f;
    public float Attack2Cooldown = 3f;

    public AudioClip JumpClip;
    public AudioClip AttackClip;
    public AudioClip Attack2Clip;
    public float Speed = 4f;
    public float JumpPower = 6f;

    private bool isFloor;
    private bool isLadder;
    private bool isClimbing;
    private float inputVertical;
    private bool justJump, justAttack;
    private bool isHoldingAttack2;
    private float attack2HoldTime;
    private float attack2CooldownTimer;
    private bool isFacingRight = true; // 현재 바라보고 있는 방향
    private bool attack2Triggered; // Attack2 트리거 상태 확인

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
        UpdateCooldowns();
    }

    private void FixedUpdate()
    {
        Jump();
        Attack();
        Climbing();
    }

    private void Move()
    {
        // 이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);
            animator.SetBool("Move", true);
            if (!isFacingRight) Flip();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed * Time.deltaTime);
            animator.SetBool("Move", true);
            if (isFacingRight) Flip();
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    private void Flip()
    {
        // 캐릭터 방향 전환
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
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
            justJump = false;
            rigidbody2d.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(JumpClip);
        }
    }

    private void Attack()
    {
        if (justAttack)
        {
            justAttack = false;
            animator.SetTrigger("Attack");
            audioSource.PlayOneShot(AttackClip);

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

        if (isHoldingAttack2)
        {
            isHoldingAttack2 = false;
            float finalAttackSpeed = Mathf.Lerp(AttackSpeed, MaxAttackSpeed, attack2HoldTime / HoldDuration);
            animator.SetTrigger("Attack2");
            audioSource.PlayOneShot(Attack2Clip);
            attack2Triggered = true;

            if (spriteRenderer.flipX)
            {
                GameObject obj = Instantiate(CrossAttackObj, transform.position, Quaternion.Euler(0f, 180f, 0f));
                obj.GetComponent<Rigidbody2D>().AddForce(Vector2.left * finalAttackSpeed, ForceMode2D.Impulse);
                Destroy(obj, 3f);
            }
            else
            {
                GameObject obj = Instantiate(CrossAttackObj, transform.position, Quaternion.Euler(0, 0, 0));
                obj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * finalAttackSpeed, ForceMode2D.Impulse);
                Destroy(obj, 3f);
            }

            attack2CooldownTimer = Attack2Cooldown; // 쿨타임 타이머 시작
            GameManager.Instance.UpdateCooldownText(attack2CooldownTimer); // 쿨타임 업데이트
        }
    }

    private void AttackCheck()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            justAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.X) && attack2CooldownTimer <= 0)
        {
            attack2HoldTime = 0f;
        }

        if (Input.GetKey(KeyCode.X) && attack2CooldownTimer <= 0)
        {
            attack2HoldTime += Time.deltaTime;
            if (attack2HoldTime > HoldDuration)
            {
                attack2HoldTime = HoldDuration;
            }
        }

        if (Input.GetKeyUp(KeyCode.X) && attack2CooldownTimer <= 0)
        {
            isHoldingAttack2 = true; // 키를 놓을 때 공격 실행
        }
    }

    private void UpdateCooldowns()
    {
        if (attack2CooldownTimer > 0)
        {
            attack2CooldownTimer -= Time.deltaTime;
            GameManager.Instance.UpdateCooldownText(attack2CooldownTimer); // 쿨타임 업데이트
        }
        else
        {
            GameManager.Instance.UpdateCooldownText(0); // 쿨타임 초기화
        }

        // Attack2 애니메이션이 끝났는지 확인
        if (attack2Triggered && animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            ResetCharacterState();
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

    private void ResetCharacterState()
    {
        // 원래 상태로 돌아가기 위한 초기화 코드
        animator.ResetTrigger("Attack2");
        animator.SetBool("Move", false);
        attack2Triggered = false;
    }
}
