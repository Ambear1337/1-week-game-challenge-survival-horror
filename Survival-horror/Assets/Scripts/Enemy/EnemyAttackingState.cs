using UnityEngine;

namespace Enemy
{
    public class EnemyAttackingState : EnemyBaseState
    {
        EnemyStateManager manager;

        public override void EnterState(EnemyStateManager _stateManager)
        {
            manager = _stateManager;

            manager.agent.isStopped = true;

            manager.AttackPlayer();

            var enemyAudioSource = manager.GetComponent<AudioSource>();

            for (var i = 0; i < manager.audioManager.enemySounds.Count; i++)
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
}
