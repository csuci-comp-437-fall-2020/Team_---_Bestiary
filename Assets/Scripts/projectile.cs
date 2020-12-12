using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private Transform currentTarget;
    private Vector2 currentPos;
    public float speed;
    Animator enemyAnim;
    Rigidbody2D rb;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        enemyAnim = enemyObj.GetComponent<Animator>();
        currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
        currentPos = currentTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget != null){
            transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);
            
            if (Mathf.Abs(transform.position.x - currentPos.x) <= .001 && Mathf.Abs(transform.position.y - currentPos.y) <= .001)
            {
                Destroy(gameObject);
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = col.gameObject.GetComponentInParent<PlayerHealth>();
            playerHealth.TakeDamage(damage); 
        }


        Destroy(gameObject);
    }
}
