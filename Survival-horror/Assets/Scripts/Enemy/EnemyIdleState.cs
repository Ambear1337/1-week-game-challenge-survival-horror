using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    EnemyStateManager manager;

    public override void EnterState(EnemyStateManager _stateManager)
    {
        manager = _stateManager;

        manager.agent.isStopped = true;

        manager.animator.SetTrigger("PlayerLost");
    }

    public override void UpdateState()
    {
        
    }
}
