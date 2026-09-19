using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public float detectRadius, attackRadius;
    public float rotationSpeed;
    public LayerMask layer;
    private bool isAttacking = false, inPatrolZone = true;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int currentWaypoint = 0;

    public Transform player;

    public enum State { Patrol, Pursuit, Attack}
    public State currentState = State.Patrol;

    public Weapon gun;

    private Coroutine shoot, returnToZone;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PatrolZone"))
        {
            Debug.Log("Exited patrol zone");
            inPatrolZone = false;
        }
    }

    void Update()
    {
        CheckState();
        Debug.Log("Current state: " + currentState + "\nInPatrolZone: " + inPatrolZone);
    }

    public void CheckState()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case State.Patrol:
                agent.isStopped = false;
                agent.stoppingDistance = 0f;
                Patrol();
                if (distance <= detectRadius) currentState = State.Pursuit;
                break;

            case State.Pursuit:
                if (inPatrolZone)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    Aim(distance);
                    if (distance <= attackRadius) currentState = State.Attack;
                    if (distance > detectRadius)
                    {
                        agent.SetDestination(waypoints[currentWaypoint].position);
                        currentState = State.Patrol;
                    }
                }
                else
                {
                    agent.SetDestination(waypoints[currentWaypoint].position);
                    returnToZone = StartCoroutine(ReturnToZone());
                    //if (!agent.pathPending)
                    //{
                    //}
                }
                break;

            case State.Attack:
                if (distance > attackRadius) currentState = State.Pursuit;
                Aim(distance);
                Attack();
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

    void Aim(float dist)
    {
        Vector3 targetDirection = player.position - transform.position;
        float speed = rotationSpeed / dist * rotationSpeed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), speed * Time.deltaTime);
    }

    void Attack()
    {
        agent.stoppingDistance = attackRadius - 0.1f;
        

        if (Physics.Raycast(gun.firePoint.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, attackRadius, layer, QueryTriggerInteraction.Ignore)){
            if (!isAttacking && hit.collider.gameObject.CompareTag("Player")) shoot = StartCoroutine(Shoot());
            Debug.DrawRay(gun.firePoint.position, transform.TransformDirection(Vector3.forward) * attackRadius, Color.red);
        }
        else Debug.DrawRay(gun.firePoint.position, transform.TransformDirection(Vector3.forward) * attackRadius, Color.blue);

    }

    public IEnumerator Shoot()
    {
        isAttacking = true;
        gun.Fire();
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    public IEnumerator ReturnToZone()
    {
        yield return new WaitForSeconds(2f);
        currentState = State.Patrol;
        inPatrolZone = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
