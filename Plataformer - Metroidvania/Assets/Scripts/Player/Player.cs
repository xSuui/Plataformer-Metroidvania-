using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    private PlayerAudio playerAudio;
    public Animator anim;
    public Transform point;

    public LayerMask enemyLayer;

    private Health healthSystem;

    public float radius;
    public float speed;
    public float jumpForce;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;

    private bool recovery;


    private static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        playerAudio = GetComponent<PlayerAudio>();

        healthSystem = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if(movement > 0)
        {
            if(!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if(movement < 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if(movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("Transition", 0);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if(doubleJump)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
        }
    }

    void Attack()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);

            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);

            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {
                if(hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();
                }
                
                if(hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().OnHit();
                }
            }

            StartCoroutine(OnAttack());
        }
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.33f);
        isAttacking = false;
    }

    float recoveryCount;
    public void OnHit()
    {
        recoveryCount += Time.deltaTime;

        if(recoveryCount >= 2f)
        {
            anim.SetTrigger("hit");
            healthSystem.health--;

            recoveryCount = 0f;
        }

        if(healthSystem.health <= 0 && !recovery)
        {
            recovery = true;
            anim.SetTrigger("dead");
            //game over aq
            GameController.instance.ShowGameOver();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    void OnCollisionEnter2D(Collision2D colisor)
    {
        if(colisor.gameObject.layer == 6)
        {
            isJumping = false;
        }

        if(colisor.gameObject.layer == 10)
        {
            PlayerPos.instance.Checkpoint();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnHit();
        }

        if(collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("hit");
            GameController.instance.GetCoin();
            Destroy(collision.gameObject, 0.4f);
        }

        if(collision.gameObject.layer == 9)
        {
            GameController.instance.NextLvl();
        }
    }
}
