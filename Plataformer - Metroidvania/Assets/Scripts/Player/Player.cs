using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rig;
    public Animator anim;

    public float speed;
    public float jumpForce;

    private bool isJumping;
    private bool doubleJump;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
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
            if(!isJumping)
            {
                anim.SetInteger("Transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if(movement < 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 1);
            }

            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if(movement == 0 && !isJumping)
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
            }
            else if(doubleJump)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D colisor)
    {
        if(colisor.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }
}
