using Battles.Entities.Projectiles;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Battles.Mechanics.Shooting.StateMachine
{
    public class FiringState : AbstractShootingState
    {
        private readonly SignalBus signalBus;
        private readonly ShootingParameters parameters;

        public FiringState(ShootingComponent component, SignalBus signalBus, ShootingParameters parameters) : base(component)
        {
            this.signalBus = signalBus;
            this.parameters = parameters;
        }
        
        public override async void Enter()
        {
            signalBus.Fire(new SpawnProjectileSignal(parameters.spawn.position, parameters.projectileVelocity, parameters.direction, parameters.lifeTime));

            await UniTask.DelayFrame(1);
            component.MakeTransition(ShootingStates.Reloading);
        }

        public override void Exit()
        {
        }

        public override bool CanTransitionTo(ShootingStates state)
        {
            return state == ShootingStates.Reloading;
        }
    }
}