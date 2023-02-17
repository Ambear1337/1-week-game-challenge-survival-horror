using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyAttackingState : EnemyBaseState
{
    EnemyStateManager manager;

    public override void EnterState(EnemyStateManager _stateManager)
    {
        manager = _stateManager;

        manager.agent.isStopped = true;

        manager.AttackPlayer();

        AudioSource enemyAudioSource = manager.GetComponent<AudioSource>();

        for (int i = 0; i < manager.audioManager.enemySounds.Count; i++)
        {
            if (manager.audioManager.enemySounds[i].name == "CultistScream")
            {
                enemyAudioSource.clip = manager.audioManager.enemySounds[i];
                enemyAudioSource.Play();
            }
        }

        manager.animator.SetTrigger("Attack");
    }

    public override void UpdateState()
    {
        
    }
}
