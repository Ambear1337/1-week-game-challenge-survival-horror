namespace Enemy
{
    public abstract class EnemyBaseState
    {
        public abstract void EnterState(EnemyStateManager stateManager);
    
        public abstract void UpdateState();
    }
}
