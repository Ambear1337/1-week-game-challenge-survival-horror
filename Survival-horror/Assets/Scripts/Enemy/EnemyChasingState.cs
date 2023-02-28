using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Enemy
{
    public class EnemyChasingState : EnemyBaseState
    {
        EnemyStateManager manager;

        public override void EnterState(EnemyStateManager stateManager)
        {
            manager = stateManager;

            stateManager.ChangePlayerChaseState(true);
            
            manager.agent.isStopped = false;

            manager.animator.SetBool("IsChasing", true);
            manager.agent.speed = manager.chaseSpeed;
        }

        public override void UpdateState()
        {
            if (manager.playerInSight)
            {
                manager.playerLastPosition = manager.player.position;

                if (Vector3.Distance(manager.transform.position, manager.player.position) > manager.attackDistance)
                {
                    manager.agent.SetDestination(manager.player.position);
                }
                else
                {
                    manager.animator.SetBool("IsChasing", false);
                    manager.SwitchState(manager.attackingState);
                }
            }
            else
            {
                if (Vector3.Distance(manager.transform.position, manager.playerLastPosition) <= 1.5f)
                {
                    manager.agent.isStopped = true;

                    manager.animator.SetBool("IsChasing", false);
                    manager.SwitchState(manager.idleState);
                }
                else
                {
                    if (manager.CheckIfCanReachPlayer(manager.playerLastPosition))
                    {
                        manager.agent.SetDestination(manager.playerLastPosition);
                    }
                    else
                    {
                        manager.agent.isStopped = true;

                        manager.animator.SetBool("IsChasing", false);
                        manager.SwitchState(manager.idleState);
                    }
                }
            }
        }
    }
}