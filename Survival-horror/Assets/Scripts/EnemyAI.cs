using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public float patrolSpeed = 5f;
    public float chaseSpeed = 10f;
    public float fieldOfView = 120f;
    public float viewDistance = 10f;
    public float attackDistance = 2f;

    public int damage = 20;

    private Transform _player;
    private bool _playerInSight;
    private bool attacking;

    // patrol variables
    public Transform[] patrolPoints;
    private int _currentPatrolPoint;

    private NavMeshAgent agent;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _currentPatrolPoint = 0;
    }

    void Update()
    {
        if (attacking) return;
        
        if (!IsPlayerInSight())
        {
            Patrol();
            return;
        }

        if (Vector3.Distance(transform.position, _player.position) > attackDistance)
        {
            animator.SetBool("IsPatroling", false);
            animator.SetBool("IsChasing", true);

            agent.speed = chaseSpeed;

            agent.SetDestination(_player.position);
            Vector3 lastPlayerPosition = _player.position;

            if (!_playerInSight)
            {
                agent.SetDestination(lastPlayerPosition);
            }
        }
        else
        {
            animator.SetTrigger("Attack");
        }
    }

    void Patrol()
    {
        // if we're close enough to the current patrol point, go to the next one
        FindNextPatrolPoint();

        // setting the next navmesh position of patroling

        animator.SetBool("IsChasing", false);
        animator.SetBool("IsPatroling", true);

        agent.speed = patrolSpeed;

        agent.SetDestination(patrolPoints[_currentPatrolPoint].position);
    }

    void FindNextPatrolPoint()
    {
        if (Vector3.Distance(transform.position, patrolPoints[_currentPatrolPoint].position) < 2f)
        {
            _currentPatrolPoint = (_currentPatrolPoint + 1) % patrolPoints.Length;
        }
    }

    bool IsPlayerInSight()
    {
        if (Vector3.Distance(transform.position, _player.position) > viewDistance)
        {
            _playerInSight = false;
            return false;
        }

        Vector3 direction = _player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle > fieldOfView / 2f)
        {
            _playerInSight = false;
            return false;
        }

        Debug.DrawRay(transform.position, direction.normalized, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out hit, viewDistance))
        {
            if (hit.collider.tag == "Player")
            {
                // Check if the player is in the light
                if (hit.collider.GetComponent<PlayerController>().IsInLight())
                {
                    _playerInSight = true;
                    return true;
                }
                else
                {
                    _playerInSight = false;
                    return false;
                }
            }
        }

        _playerInSight = false;
        return false;
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void Attack()
    {
        attacking = true;

        //_player.GetComponent<PlayerStats>().GetDamage(damage);
        StartCoroutine(AttackCooldown());
    }

    IEnumerator LosePlayer()
    {
        yield return new WaitForSeconds(3f);
    }

    IEnumerator AttackCooldown()
    {
        animator.SetTrigger("PlayerLost");
        yield return new WaitForSeconds(2f);
        attacking = false;
    }
}
