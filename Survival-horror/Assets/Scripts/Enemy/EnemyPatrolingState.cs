using UnityEngine;

namespace Enemy
{
    public class EnemyPatrolingState : EnemyBaseState
    {
        EnemyStateManager manager;
    
        public override void EnterState(EnemyStateManager _stateManager)
        {
            manager = _stateManager;

            manager.agent.isStopped = false;

            manager.animator.SetBool("IsPatroling", true);
            manager.agent.speed = manager.patrolSpeed;
        }

        public override void UpdateState()
        {
            Patrol();
        }

        void Patrol()
        {
            if (!manager.playerInSight)
            {
                // if we're close enough to the current patrol point, go to the next one
                FindNextPatrolPoint();

                // setting the next navmesh position of patroling
                manager.agent.SetDestination(manager.patrolPoints[manager.currentPatrolPoint].position);
            }
            else
            {
                manager.animator.SetBool("IsPatroling", false);
                manager.SwitchState(manager.chasingState);
            }
        }

        void FindNextPatrolPoint()
        {
            if (Vector3.Distance(manager.transform.position, manager.patrolPoints[manager.currentPatrolPoint].position) < 2f)
            {
                manager.currentPatrolPoint = (manager.currentPatrolPoint + 1) % manager.patrolPoints.Length;
            }
        }
    }
}
