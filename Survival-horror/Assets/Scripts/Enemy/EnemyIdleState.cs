namespace Enemy
{
    public class EnemyIdleState : EnemyBaseState
    {
        EnemyStateManager manager;

        public override void EnterState(EnemyStateManager stateManager)
        {
            manager = stateManager;

            manager.agent.isStopped = true;

            manager.animator.SetTrigger("PlayerLost");
        }

        public override void UpdateState()
        {
        
        }
    }
}
