using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runEnemyAI : MonoBehaviour
{
    public float moveSpeed;
    private float moveDirection = 1; 
    public Transform groundCheck;
    public Transform wallCheck;
    public float circleRadius;
    public LayerMask groundLayer;
    private bool checkingWall;
    private bool checkingGround;

    public float jumpHeight;
    public Transform destination;
    public Transform touchingCheck;
    public Vector2 boxSize;
    private bool grounded;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        checkingGround = Physics2D.OverlapCircle(groundCheck.position, circleRadius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallCheck.position, circleRadius, groundLayer);
        grounded = Physics2D.OverlapBox(touchingCheck.position, boxSize, 0, groundLayer);
        OnPatrol();
    }
    
    void OnPatrol()
    {   
        if (checkingWall)
        {
            if (moveDirection > 0)
            {
                Flip();
            }
            else 
            {
                Flip();
            }
        }
        rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
    }

    void Jump()
    {
        float xDistanceFromDestination = destination.position.x - transform.position.x;

        if (grounded)
        {
            rb.AddForce(new Vector2(xDistanceFromDestination, jumpHeight), ForceMode2D.Impulse);
        }
    }
    
    void Flip()
    {
        moveDirection *= -1;
        transform.Rotate(0, 180, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("jumppad"))
        {
            Jump();
        }
    }
}
