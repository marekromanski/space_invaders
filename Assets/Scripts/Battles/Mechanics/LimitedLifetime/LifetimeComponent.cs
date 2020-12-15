using System;
using Cysharp.Threading.Tasks;

namespace Battles.Mechanics.LimitedLifetime
{
    public class LifetimeComponent : ITimeLimited
    {
        private readonly float lifeTime;

        private bool timeElapsed;

        public LifetimeComponent(float lifeTime)
        {
            this.lifeTime = lifeTime;

            CountTime().Forget();
        }

        public bool TimeElapsed()
        {
            return timeElapsed;
        }

        private async UniTask CountTime()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            timeElapsed = true;
        }
    }
}