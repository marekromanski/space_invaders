namespace Battles.Mechanics.Shooting.StateMachine
{
    public interface IState
    {
        void Enter();
        void Exit();
        bool CanTransitionTo(ShootingStates state);
    }
}