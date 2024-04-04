using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    private Rigidbody2D rig;
    private bool isFront;

    public Transform point;

    public float speed;
    public float maxVision;

    // Start is called before the first frame update
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
        GetPlayer();
    }

    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(point.position, Vector2.right, maxVision);

        if(hit.collider != null)
        {
            if(hit.transform.CompareTag("Player"))
            {
                Debug.Log("Player ali!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(point.position, Vector2.right * maxVision);
    }
}
