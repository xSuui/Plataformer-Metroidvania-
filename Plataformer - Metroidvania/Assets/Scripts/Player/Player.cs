using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    private PlayerAudio playerAudio;
    public Animator anim;
    public Transform point;

    public LayerMask enemyLayer;

    public float recoveryTime;

    [Header("UI")]
    public Text scoreText;
    public GameObject gameOver;

    private Health healthSystem;

    public float radius;
    public float speed;
    public float jumpForce;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;

    private bool recovery;


    public static Player instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        if(!recovery)
        {
            anim.SetTrigger("hit");
            healthSystem.health--;

            if (healthSystem.health <= 0)
            {
                recovery = true;
                anim.SetTrigger("dead");
                //game over aq
                GameController.instance.ShowGameOver();
            }
        }
        else
        {
            StartCoroutine(Recover());
        }
        
    }

    private IEnumerator Recover()
    {
        recovery = true;
        yield return new WaitForSeconds(recoveryTime);
        recovery = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    void OnCollisionEnter2D(Collision2D colisor)
    {
        if(colisor.gameObject.layer == 6 || colisor.gameObject.layer == 11)
        {
            isJumping = false;
        }

        if(colisor.gameObject.layer == 10)
        {
            PlayerPos.instance.Checkpoint();
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
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
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Colisão com: {collision.name}");

        if (collision.gameObject.layer == 7)
        {
            //Debug.Log("Chamando OnHit()");
            OnHit();
        }

        if (collision.CompareTag("Coin"))
        {
            //Debug.Log("Colisão com moeda detectada");

            if (playerAudio == null)
            {
                //Debug.LogError("playerAudio está null!");
            }
            else
            {
                playerAudio.PlaySFX(playerAudio.coinSound);
            }

            Animator anim = collision.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("hit");
            }
            else
            {
                //Debug.LogWarning($"Objeto {collision.name} não possui Animator.");
            }

            if (GameController.instance == null)
            {
                //Debug.LogError("GameController.instance está null!");
            }
            else
            {
                GameController.instance.GetCoin();
            }

            Destroy(collision.gameObject, 0.4f);
        }
    }

}
