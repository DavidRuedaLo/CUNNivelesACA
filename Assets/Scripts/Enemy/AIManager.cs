using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public float detectRadius, attackRadius;
    public float rotationSpeed;
    public LayerMask layer;
    private bool isAttacking = false;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int currentWaypoint = 0;

    public Transform player;

    public enum State { Patrol, Pursuit, Attack}
    public State currentState = State.Patrol;

    public Weapon gun;

    private Coroutine shoot;


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
                    agent.SetDestination(waypoints[currentWaypoint].position);
                    currentState = State.Patrol;
                }
                break;

            case State.Attack:
                if (distance > attackRadius) currentState = State.Pursuit;
                Attack(distance);
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

    void Attack(float dist)
    {
        agent.stoppingDistance = attackRadius - 0.1f;
        //transform.LookAt(player);
        Vector3 targetDirection = player.position - transform.position;
        float speed = rotationSpeed / dist * rotationSpeed;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirection), speed * Time.deltaTime);

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
