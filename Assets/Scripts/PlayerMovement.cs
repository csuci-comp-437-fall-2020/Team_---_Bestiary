using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;

    private Rigidbody2D _body;
    private BoxCollider2D _collisionBox;
    private SpriteRenderer _sprite;
    private float _gravity = -25f;
    private bool _grounded;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collisionBox = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        Physics2D.gravity = new Vector3(0f, _gravity,0f);
    }
    
    void OnCollisionEnter2D(Collision2D Other)
    {
        switch (Other.collider.gameObject.tag)
        {
            case "Floor":
                _grounded = true;
                break;
            case "Platform":
                _grounded = true;
                break;
        }
    }
 
    void OnCollisionExit2D(Collision2D Other){
        switch (Other.collider.gameObject.tag)
        {
            case "Floor":
                _grounded = false;
                break;
            case "Platform":
                _grounded = false;
                break;
        }
    }
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        _body.velocity = movement;
        if (movement.x != 0)
            _sprite.flipX = movement.x < 0;
        
        // Jump Mechanic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grounded)
            {
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }
            // Soon TM
            // else if (_jumpsLeft > 0)
            // {
            //     _body.velocity = Vector2.zero;
            //     _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            // }
        }
    }
}
