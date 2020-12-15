namespace Battles.Mechanics.Shooting.StateMachine
{
    public class IdleState : AbstractShootingState
    {
        public IdleState(ShootingComponent shootingComponent) : base(shootingComponent)
        {
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override bool CanTransitionTo(ShootingStates state)
        {
            return state == ShootingStates.Firing;
        }
    }
}