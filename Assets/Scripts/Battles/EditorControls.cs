using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Battles
{
    public class EditorControls : ITickable
    {
        private readonly SignalBus signalBus;

        [UsedImplicitly]
        public EditorControls(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }

        public void Tick()
        {
            var delta = 0f;
            if (PlayerMovedLeft())
            {
                delta = -1f;
            }
            else if (PlayerMovedRight())
            {
                delta = 1f;
            }

            if (delta != 0f)
            {
                signalBus.Fire(new PlayerMovedSignal(delta));
            }

            if (PlayerShotBullet())
            {
                signalBus.Fire<PlayerShotSignal>();
            }
        }

        private bool PlayerShotBullet()
        {
            return Input.GetKey(KeyCode.Space);
        }

        private static bool PlayerMovedRight()
        {
            return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        }

        private static bool PlayerMovedLeft()
        {
            return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        }
    }
}