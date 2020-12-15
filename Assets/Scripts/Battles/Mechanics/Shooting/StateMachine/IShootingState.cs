namespace Battles.Mechanics.Shooting.StateMachine
{
    public abstract class AbstractShootingState : IState
    {
        protected readonly ShootingComponent component;

        protected AbstractShootingState(ShootingComponent component)
        {
            this.component = component;
        }
        
        public abstract void Enter();
        public abstract void Exit();
        public abstract bool CanTransitionTo(ShootingStates state);
    }
}