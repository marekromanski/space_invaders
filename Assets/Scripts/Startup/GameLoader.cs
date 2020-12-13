using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Startup
{
    public class GameLoader
    {
        private readonly SignalBus signalBus;

        [UsedImplicitly]
        public GameLoader(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            LoadGame().Forget();
        }

        private async UniTask LoadGame()
        {
            var depenencies = LoadDependencies();

            await UniTask.WhenAll(depenencies);
            Debug.Log("Dependencies loaded");

            signalBus.Fire<DependenciesLoadedSignal>();

            Debug.Log("Game Loaded");
        }

        private IEnumerable<UniTask> LoadDependencies()
        {
            Debug.Log("Loading dependencies");
            var dependencies = new List<UniTask>();
            //TODO: load asset bundles here
            return dependencies;
        }
    }
}