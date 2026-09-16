using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class AIManager : MonoBehaviour
{
    public float detectRadius, attackRadius;

    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int currentWaypoint = 0;

    public Transform player;

    public enum State { Patrol, Pursuit, Attack}
    public State currentState = State.Patrol;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        CheckState();
    }

    public void CheckState()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case State.Patrol:
                agent.stoppingDistance = 0f;
                Patrol();
                if (distance <= detectRadius) currentState = State.Pursuit;
                break;

            case State.Pursuit:
                agent.SetDestination(player.position);
                if (distance <= attackRadius) currentState = State.Attack;
                else if (distance > detectRadius)
                {
                    agent.SetDestination(waypoints[0].position);
                    currentState = State.Patrol;
                }
                break;

            case State.Attack:
                agent.stoppingDistance = attackRadius - 0.5f;
                if (distance > attackRadius) currentState = State.Pursuit;
                break;

            default:
                break;
        }
    }

    void Patrol()
    {
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
