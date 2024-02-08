using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    public float speed;

    public Transform point;
    public float radius;
    public LayerMask layer;
    
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rig.velocity = new Vector2(speed, rig.velocity.y);
        OnCollision();
    }

    void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);

        if (hit != null)
        {
            speed = -speed;

            if(transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }
}
