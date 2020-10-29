using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIDemo : MonoBehaviour
{


    public Transform target;
    public float speed = 400.0f;
    public float nextWaypointDistance = 3.0f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    Rigidbody2D rigidB;
    private Animator _anim;
    

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        seeker = GetComponent<Seeker>();
        rigidB = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.25f);
    }

    void UpdatePath()
    {
        if(Vector3.Distance(rigidB.position, target.position) > 1.0f)
        {
            seeker.StartPath(rigidB.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rigidB.position).normalized;
        Vector2 force = direction * (speed * Time.deltaTime);
        
        _anim.SetFloat("Speed", Mathf.Abs(rigidB.velocity.x));
        if (Mathf.Abs(rigidB.velocity.x) > 0.1f)
        {
            Vector3 lscale = gameObject.transform.localScale; 
            gameObject.transform.localScale = new Vector3(Mathf.Sign(rigidB.velocity.x) * Mathf.Abs(lscale.x), lscale.y, 1);
        }
        
        rigidB.AddForce(force);

        float distance = Vector2.Distance(rigidB.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
