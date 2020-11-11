using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class runEnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 30.0f;
    private IEnumerator coroutine;
    private Rigidbody2D rigidB;
    bool jumping;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rigidB = GetComponent<Rigidbody2D>();
        coroutine = WaitToRun();
        jumping = false;
    }

    public IEnumerator WaitToRun()
    {
        yield return new WaitForSecondsRealtime(3.0f);
    }

    void FixedUpdate()
    {
        Vector2 direction = new Vector2(target.position.x, 0);
        Vector2 trueSpeed =  new Vector2(speed * Time.deltaTime, 0);

        if(!jumping)
        {
            if(transform.position.x < (direction.x - 1))
            {
                rigidB.AddForce(transform.right * speed);      
            }

            if(transform.position.x > (direction.x + 1))
            {
            
                rigidB.AddForce(-transform.right * speed);
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {    
        if(col.tag == "jumppad")
        {
            jumping = true;
            Transform destination = col.GetComponentInChildren<Transform>();
            rigidB.AddForce(transform.up * 20.0f, ForceMode2D.Impulse);
        }
    }
}
