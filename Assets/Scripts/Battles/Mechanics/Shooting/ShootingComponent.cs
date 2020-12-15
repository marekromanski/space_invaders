using System.Collections.Generic;
using Battles.Mechanics.Shooting.StateMachine;
using Zenject;

namespace Battles.Mechanics.Shooting
{
    public class ShootingComponent
    {
        private readonly ShootingParameters parameters;
        private readonly SignalBus signalBus;

        private readonly Dictionary<ShootingStates, AbstractShootingState> statesMapping =
            new Dictionary<ShootingStates, AbstractShootingState>(3);

        private IState currentState;

        public ShootingComponent(ShootingParameters parameters, SignalBus signalBus)
        {
            this.parameters = parameters;
            this.signalBus = signalBus;

            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            statesMapping.Add(ShootingStates.Idle, new IdleState(this));
            statesMapping.Add(ShootingStates.Firing, new FiringState(this, signalBus, parameters));
            statesMapping.Add(ShootingStates.Reloading, new ReloadingState(this, parameters.minShotsInterval));

            currentState = statesMapping[ShootingStates.Idle];
        }

        public void MakeTransition(ShootingStates state)
        {
            if (currentState.CanTransitionTo(state))
            {
                currentState.Exit();
                currentState = statesMapping[state];
                currentState.Enter();
            }
        }

        public void AttemptShot()
        {
            MakeTransition(ShootingStates.Firing);
        }
    }
}