using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    EnemyBaseState currentState;
    public EnemyBaseState previousState;

    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyAttackingState attackingState = new EnemyAttackingState();
    public EnemyPatrolingState patrolingState = new EnemyPatrolingState();
    public EnemyChasingState chasingState = new EnemyChasingState();

    public Animator animator;

    public float patrolSpeed = 5f;
    public float chaseSpeed = 10f;
    public float fieldOfView = 120f;
    public float viewDistance = 10f;
    public float attackDistance = 2f;

    public int damage = 99;

    public Transform enemyFace;
    public Transform _player;
    public PlayerStats playerStats;
    public bool _playerInSight;
    public bool attacking;

    // patrol variables
    public Transform[] patrolPoints;
    public int _currentPatrolPoint;

    public Vector3 playerLastPosition;

    public NavMeshAgent agent;
    public EnemyAudioManager audioManager;

    private void Start()
    {
        currentState = patrolingState;

        currentState.EnterState(this);
    }

    private void Update()
    {
        _playerInSight = IsPlayerInSight();
    }

    private void FixedUpdate()
    {
        currentState.UpdateState();
    }

    public void SwitchState(EnemyBaseState nextState)
    {
        previousState = currentState;
        currentState = nextState;

        currentState.EnterState(this);
    }

    bool IsPlayerInSight()
    {
        if (Vector3.Distance(transform.position, _player.position) > viewDistance)
        {
            return false;
        }

        Vector3 direction = _player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle > fieldOfView / 2f)
        {
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    public void CheckPlayerInSightAfterAttack()
    {
        if (_playerInSight)
        {
            SwitchState(chasingState);
        }
        else
        {
            SwitchState(patrolingState);
        }
    }

    public void AttackPlayer()
    {
        _player.GetComponent<PlayerStats>().GetDamage(damage, enemyFace);
        transform.LookAt(_player);
    }

    public void FinishAttack()
    {
        _player.GetComponent<PlayerStats>().ReleasePlayer();

        SwitchState(idleState);
    }

    public void FinishStun()
    {
        if (_playerInSight)
        {
            SwitchState(chasingState);
        }   
        else
        {
            SwitchState(patrolingState);
        }
    }
}
