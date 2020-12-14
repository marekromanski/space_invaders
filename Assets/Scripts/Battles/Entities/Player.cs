using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles.Entities
{
    public class Player : MonoBehaviour
    {
        private SignalBus signalBus;
        private IPlayerConfiguration playerConfiguration;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IPlayerConfiguration playerConfiguration)
        {
            this.signalBus = signalBus;
            this.playerConfiguration = playerConfiguration;

            signalBus.Subscribe<PlayerMovedSignal>(OnPlayerMoved);
        }

        private void OnPlayerMoved(PlayerMovedSignal signal)
        {
            var currentPosition = transform.position;
            var targetPosition = new Vector3(currentPosition.x + signal.delta, currentPosition.y, currentPosition.z);

            transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * playerConfiguration.MoveSpeed);
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<PlayerMovedSignal>(OnPlayerMoved);
        }
    }
}