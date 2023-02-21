using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy
{
    public class EnemyStateManager : MonoBehaviour
    {
        private EnemyBaseState currentState;
        public EnemyBaseState previousState;

        public readonly EnemyIdleState idleState = new EnemyIdleState();
        public readonly EnemyAttackingState attackingState = new EnemyAttackingState();
        public readonly EnemyPatrolingState patrolingState = new EnemyPatrolingState();
        public readonly EnemyChasingState chasingState = new EnemyChasingState();

        public Animator animator;

        public float patrolSpeed = 5f;
        public float chaseSpeed = 10f;
        public float fieldOfView = 120f;
        public float viewDistance = 10f;
        public float viewDistanceInDark = 5f;
        public float attackDistance = 2f;
        public float hearDistanceCrouch = 2f;
        public float hearDistanceWalk = 5f;
        public float hearDistanceRun = 10f;

        public int damage = 90;

        public Transform enemyFace;
        public Transform player;
        public bool playerInSight;
        public bool attacking;

        // patrol variables
        public Transform[] patrolPoints;

        [FormerlySerializedAs("_currentPatrolPoint")]
        public int currentPatrolPoint;
        
        public Vector3 playerLastPosition;
        public LayerMask playerMask;

        public NavMeshAgent agent;
        public EnemyAudioManager audioManager;

        private void Awake()
        {
            player = FindObjectOfType<PlayerManager>().transform;
        }

        private void Start()
        {
            currentState = patrolingState;

            currentState.EnterState(this);
        }

        private void Update()
        {
            playerInSight = IsPlayerInSight();
        }

        private void FixedUpdate()
        {
            currentState.UpdateState();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void SwitchState(EnemyBaseState nextState)
        {
            previousState = currentState;
            currentState = nextState;

            currentState.EnterState(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private bool IsPlayerInSight()
        {
            if (!CheckIfCanReachPlayer(player.position))
            {
                return false;
            }
            
            if (Vector3.Distance(transform.position, player.position) > viewDistance)
            {
                return false;
            }

            var direction = player.position - transform.position;
            var angle = Vector3.Angle(direction, transform.forward);

            if (angle > fieldOfView / 2f)
            {
                if (!Physics.Raycast(transform.position + Vector3.up * 0.3f, direction.normalized, out var hit, viewDistance, playerMask)) return false;

                var playerManager = hit.collider.GetComponent<PlayerManager>();
                var distance = Vector3.Distance(transform.position, playerManager.transform.position);

                switch (playerManager.PlayerController.noiseValue)
                {
                    case 0.3f:
                        if (distance > hearDistanceCrouch)
                        {
                            return false;
                        }
                        return true;
                    case 0.6f:
                        if (distance > hearDistanceWalk)
                        {
                            return false;
                        }
                        return true;
                    case 1f: return true;
                    default: return false;
                }
            }
            else
            {
                if (!Physics.Raycast(transform.position + Vector3.up * 0.3f, direction.normalized, out var hit, viewDistance, playerMask)) return false;

                var distance = Vector3.Distance(transform.position, player.position);

                if (hit.collider.GetComponent<PlayerManager>().PlayerController.isInLight)
                {
                    return !(distance > viewDistance);
                }
                else
                {
                    return !(distance > viewDistanceInDark);
                }
            }
        }

        public void CheckPlayerInSightAfterAttack()
        {
            if (playerInSight)
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
            player.GetComponent<PlayerManager>().PlayerStats.GetDamage(damage, enemyFace);
            transform.LookAt(player);
        }

        public void FinishAttack()
        {
            player.GetComponent<PlayerManager>().PlayerStats.ReleasePlayer();

            SwitchState(idleState);
        }

        public void FinishStun()
        {
            if (playerInSight && CheckIfCanReachPlayer(player.position))
            {
                SwitchState(chasingState);
            }
            else
            {
                SwitchState(patrolingState);
            }
        }

        public bool CheckIfCanReachPlayer(Vector3 target)
        {
            var navMeshPath = new NavMeshPath();
            
            if (agent.CalculatePath(target, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}