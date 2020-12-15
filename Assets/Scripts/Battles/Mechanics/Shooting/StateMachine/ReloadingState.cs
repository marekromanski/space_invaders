using System;
using Cysharp.Threading.Tasks;

namespace Battles.Mechanics.Shooting.StateMachine
{
    public class ReloadingState : AbstractShootingState
    {
        private readonly float minShotInterval;

        private bool reloadingFinished = false;

        public ReloadingState(ShootingComponent component, float minShotInterval) : base(component)
        {
            this.minShotInterval = minShotInterval;
        }

        public override async void Enter()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(minShotInterval));
            reloadingFinished = true;
            component.MakeTransition(ShootingStates.Idle);
        }

        public override void Exit()
        {
            reloadingFinished = false;
        }

        public override bool CanTransitionTo(ShootingStates state)
        {
            return reloadingFinished && state == ShootingStates.Idle;
        }
    }
}