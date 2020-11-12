using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public GameObject[] waypointArray;
    private int wpArrayLength = 4;
    public Transform target;
    public float speed = 400.0f;
    public float nextWaypointDistance = 3.0f;

    Path path;
    int currentWaypoint = 0;
    bool attacking = false;
    bool reachedEndofPath = false;

    IEnumerator coroutine;
    Seeker seeker;
    Rigidbody2D rigidB;

    // Start is called before the first frame update
    void Start()
    {
        waypointArray = GameObject.FindGameObjectsWithTag("waypoint");
        seeker = GetComponent<Seeker>();
        rigidB = GetComponent<Rigidbody2D>();

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
    
    public IEnumerator cycleTarget(GameObject[] waypoints)
    {
        while (!attacking)
        {
            for (int i = 0; i < 4; i++)
            {
                if(i < 3)
                {
                    target = waypoints[i].transform;
                    yield return new WaitForSecondsRealtime(3.0f);
                }
                else
                {
                    attacking = true;
                    target = GameObject.FindGameObjectWithTag("Player").transform;
                    yield return new WaitForSecondsRealtime(15.0f);
                    attacking = false;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        coroutine = cycleTarget(waypointArray);
        StartCoroutine(coroutine);

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

        rigidB.AddForce(force);

        float distance = Vector2.Distance(rigidB.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}
