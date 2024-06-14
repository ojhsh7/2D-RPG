using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private AudioSource audioSource;
    private Collider2D floorCollider;

    public GameObject AttackObj;
    public float AttackSpeed = 3f;
    public AudioClip JumpClip;
    public AudioClip AttackClip;
    public float Speed = 4f;
    public float JumpPower = 6f;
    public int Damage = 10;
    public int health = 100;

    private bool isFloor;
    private bool isLadder;
    private bool isClimbing;
    private float inputVertical;
    private bool justJump;
    private bool justAttack;
    private bool faceRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        floorCollider = null; // 초기화
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
        float horizontalInput = Input.GetAxis("Horizontal");

        // 이동 입력 처리
        if (Mathf.Approximately(horizontalInput, 0f))
        {
            animator.SetBool("Move", false);
        }
        else
        {
            transform.Translate(Vector3.right * horizontalInput * Speed * Time.deltaTime);
            if (horizontalInput > 0 && !faceRight) Flip();
            else if (horizontalInput < 0 && faceRight) Flip();
            animator.SetBool("Move", true);
        }
    }

    private void Flip()
    {
        faceRight = !faceRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isFloor = true;
            floorCollider = collision.collider; // Floor 콜라이더 저장
        }
        else if (collision.gameObject.CompareTag("Monster"))
        {
            TakeDamage(10); // 몬스터와 충돌 시 데미지를 입음
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == floorCollider)
        {
            isFloor = false;
            floorCollider = null; // Floor 콜라이더 초기화
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void JumpCheck()
    {
        if (isFloor && Mathf.Approximately(rigidbody2d.velocity.y, 0f))
        {
            animator.SetBool("Jump", false); // 바닥에 있고 속도가 0인 경우 점프 애니메이션 비활성화
        }

        if (isFloor && Input.GetKeyDown(KeyCode.Space))
        {
            justJump = true;
            animator.SetBool("Jump", true); // 점프 시작 시 애니메이션 트리거
        }
    }

    private void Jump()
    {
        if (justJump)
        {
            justJump = false;
            rigidbody2d.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            audioSource.PlayOneShot(JumpClip);
        }
    }

    private void AttackCheck()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            justAttack = true;

            // 공격 애니메이션이 실행 중이면 다른 애니메이션 종료
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                animator.SetBool("Move", false);
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

            // 키보드에서 방향키 입력에 따라 공격 방향 설정
            float horizontalInput = Input.GetAxis("Horizontal");
            if (Mathf.Approximately(horizontalInput, 0f))
            {
                horizontalInput = faceRight ? 1f : -1f;
            }

            if (gameObject.name == "Warrior(Clone)")
            {
                AttackObj.GetComponent<Collider2D>().enabled = true;
                Invoke("SetAttackObjInactive", 0.5f);
            }
            else
            {
                if (!faceRight)
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
    }

    private void ClimbingCheck()
    {
        inputVertical = Input.GetAxis("Vertical");
        if (isLadder && Mathf.Abs(inputVertical) > 0)
        {
            isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }
    }

    private void Climbing()
    {
        if (isClimbing)
        {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, inputVertical * Speed);
        }
    }

    private void SetAttackObjInactive()
    {
        AttackObj.GetComponent<Collider2D>().enabled = false;
        Debug.Log("AttackObj.inactive position : " + AttackObj.transform.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!"); // 콘솔에 사망 메시지 출력

        // 여기에 원하는 사망 처리를 추가합니다. 예를 들어:
        // 1. 플레이어 게임 오브젝트를 비활성화합니다.
        gameObject.SetActive(false);

        // 2. 게임 오버 씬으로 이동하거나 다른 처리를 수행할 수 있습니다.
        SceneManager.LoadScene("GameOverScene"); // 씬 이름을 실제 존재하는 씬 이름으로 변경
    }

    public void IncreaseSpeed(float amount)
    {
        Speed += amount;
    }

    public void IncreaseDamage(int amount)
    {
        Damage += amount;
    }
}
