using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class AIScript : MonoBehaviour
{
    public enum State{
        Patrol, 
        Searching,
        WorkingComputer,
        TalkingToCollegue
    }
    public float maxAngle = 45;
    public float maxDistance = 10;
    public float timer = 1.0f;
    public float visionCheckRate = 1.0f;
    public GameObject[] points;
    public Animator animator;
    private int destPoint = 0;
    State agentState;

    NavMeshAgent agent; 
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        //animator = GetComponent<Animator>();
        agent.autoBraking = false;
    }

    
    // Update is called once per frame
    void Update()
    { 
        UpdateAnimator();

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !IsInvoking("GotoNextPoint"))
        {
            int i = (destPoint - 1 >= 0)? destPoint -1:0;
            State animStateToUse =  points[i].GetComponent<PatrolPointData>().patrolPointState;
            agent.isStopped= true;
            Debug.Log(animStateToUse);
            agentState =  animStateToUse;
            Invoke("GotoNextPoint", 2.0f);
        }
    }
    void UpdateAnimator()
    {
        animator.SetFloat("CharacterSpeed",agent.velocity.magnitude);
        animator.SetBool("isLookingAround", agentState == State.Searching);
        animator.SetBool("isTalkingToCollegue", agentState == State.TalkingToCollegue);
        animator.SetBool("isWorkingComputer", agentState == State.WorkingComputer);
    }

    void GotoNextPoint() {
        agent.isStopped= false;
        agentState =  State.Patrol;
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].transform.position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

}
