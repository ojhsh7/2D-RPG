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
        floorCollider = null; // �ʱ�ȭ
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

        // �̵� �Է� ó��
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
            floorCollider = collision.collider; // Floor �ݶ��̴� ����
        }
        else if (collision.gameObject.CompareTag("Monster"))
        {
            TakeDamage(10); // ���Ϳ� �浹 �� �������� ����
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider == floorCollider)
        {
            isFloor = false;
            floorCollider = null; // Floor �ݶ��̴� �ʱ�ȭ
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
            animator.SetBool("Jump", false); // �ٴڿ� �ְ� �ӵ��� 0�� ��� ���� �ִϸ��̼� ��Ȱ��ȭ
        }

        if (isFloor && Input.GetKeyDown(KeyCode.Space))
        {
            justJump = true;
            animator.SetBool("Jump", true); // ���� ���� �� �ִϸ��̼� Ʈ����
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

            // ���� �ִϸ��̼��� ���� ���̸� �ٸ� �ִϸ��̼� ����
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

            // Ű���忡�� ����Ű �Է¿� ���� ���� ���� ����
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
        Debug.Log("Player died!"); // �ֿܼ� ��� �޽��� ���

        // ���⿡ ���ϴ� ��� ó���� �߰��մϴ�. ���� ���:
        // 1. �÷��̾� ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        gameObject.SetActive(false);

        // 2. ���� ���� ������ �̵��ϰų� �ٸ� ó���� ������ �� �ֽ��ϴ�.
        SceneManager.LoadScene("GameOverScene"); // �� �̸��� ���� �����ϴ� �� �̸����� ����
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
