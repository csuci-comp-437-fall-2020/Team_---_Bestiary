using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolBehavior : StateMachineBehaviour
{
    private GameObject[] waypoints;
    private Transform currentTarget;
    private Transform player;
    public float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waypoints = GameObject.FindGameObjectsWithTag("waypoint2");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        int randInt = Random.Range(0, 2);
        currentTarget = waypoints[randInt].transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, currentTarget.position, speed * Time.deltaTime);

        if (currentTarget == waypoints[1].transform && animator.transform.position.y < waypoints[1].transform.position.y + 0.25)
        {
            currentTarget = waypoints[0].transform;
        }
        else if(currentTarget == waypoints[0].transform && animator.transform.position.y > waypoints[0].transform.position.y - 0.25)
        {
            currentTarget = waypoints[1].transform;
        }


        if (player.position.y <= animator.transform.position.y + .75f && player.position.y >= animator.transform.position.y - .75f)
        {
                animator.SetBool("isShooting", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
