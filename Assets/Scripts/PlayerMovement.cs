using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public float slidingGravity;
    [SerializeField] private float baseGravity;

    private Rigidbody2D _body;
    private BoxCollider2D _collisionBox;
    private SpriteRenderer _sprite;
    // private 

    private bool _grounded;
    private bool _sliding;
    private int _jumpsLeft;

    public int extraJumps;
    
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _collisionBox = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        Physics2D.gravity = new Vector3(0f, baseGravity, 0f);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        switch (other.collider.gameObject.tag)
        {
            case "Floor":
                _grounded = true;
                _sliding = false;
                _jumpsLeft = extraJumps;
                break;
            case "Platform":
                _grounded = true;
                _sliding = false;
                _jumpsLeft = extraJumps;
                break;
            case "Slide":
                _sliding = true;
                _jumpsLeft = extraJumps;
                // other.gameObject.transform.rotation.eulerAngles;
                Physics2D.gravity = new Vector3(0f, slidingGravity, 0f);
                break;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        switch (other.collider.gameObject.tag)
        {
            case "Floor":
                _grounded = false;
                break;
            case "Platform":
                _grounded = false;
                break;
            case "Slide":
                _sliding = false;
                Physics2D.gravity = new Vector3(0f, baseGravity, 0f);
                break;
        }
    }

    void Update()
    {
        if (!_sliding)
        {
            float deltaX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            Vector2 movement = new Vector2(deltaX, _body.velocity.y);
            _body.velocity = movement;
            if (movement.x != 0)
                _sprite.flipX = movement.x < 0;
        }

        // Jump Mechanic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_grounded)
            {
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }
            else if (_sliding)
            {
                _body.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }

            else if (_jumpsLeft > 0)
            {
                float height = Mathf.Min(jumpHeight, 1.25f * jumpHeight * _jumpsLeft / extraJumps);
                _body.velocity = Vector2.zero;
                _body.AddForce(Vector2.up * height, ForceMode2D.Impulse);
                _jumpsLeft--;
            }
        }
    }
}