using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class AIManager : MonoBehaviour
{
    public float detectRadius, attackRadius;
    public float rotationSpeed;
    public LayerMask layer;
    public float currentHP, maxHP = 15f;
    private bool isAttacking = false;
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public int currentWaypoint = 0;
    public Slider healthBarSlider;

    public Transform player;

    public enum State { Patrol, Pursuit, Attack}
    public State currentState = State.Patrol;

    public Weapon gun;

    private Coroutine shoot;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        currentHP = maxHP;
        healthBarSlider.maxValue = maxHP;
    }

    void Update()
    {
        CheckState();
        healthBarSlider.transform.rotation = mainCamera.transform.rotation;
        healthBarSlider.value = currentHP;
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
                agent.isStopped = false;
                agent.SetDestination(player.position);
                Aim(distance);
                if (distance <= attackRadius) currentState = State.Attack;
                if (distance > detectRadius)
                {
                    agent.SetDestination(waypoints[currentWaypoint].position);
                    currentState = State.Patrol;
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

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy HP: " + currentHP);
        if(currentHP <= 0)
        {
            Destroy(gameObject);
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
