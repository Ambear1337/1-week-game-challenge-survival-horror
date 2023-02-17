using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    EnemyStateManager manager;

    public override void EnterState(EnemyStateManager _stateManager)
    {
        manager = _stateManager;

        manager.agent.isStopped = false;

        manager.animator.SetBool("IsChasing", true);
        manager.agent.speed = manager.chaseSpeed;
    }

    public override void UpdateState()
    {
        if (manager._playerInSight)
        {
            manager.playerLastPosition = manager._player.position;
            
            if (Vector3.Distance(manager.transform.position, manager._player.position) > manager.attackDistance)
            {
                manager.agent.SetDestination(manager._player.position);
            }
            else
            {
                manager.animator.SetBool("IsChasing", false);
                manager.SwitchState(manager.attackingState);
            }
        }
        else
        {
            if (Vector3.Distance(manager.transform.position, manager.playerLastPosition) <= 2.5f)
            {
                manager.agent.isStopped = true;

                manager.animator.SetBool("IsChasing", false);
                manager.SwitchState(manager.idleState);
            }
            else
            {
                manager.agent.SetDestination(manager.playerLastPosition);
            }
        }
    }
}
