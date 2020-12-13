using System.Collections.Generic;
using AssetManagement;
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
        private readonly AssetFactory assetFactory;

        [UsedImplicitly]
        public GameLoader(SignalBus signalBus, AssetFactory assetFactory)
        {
            this.signalBus = signalBus;
            this.assetFactory = assetFactory;

            LoadGame().Forget();
        }

        private async UniTask LoadGame()
        {
            var depenencies = LoadDependencies();

            await UniTask.WhenAll(depenencies);
            Debug.Log("Dependencies loaded");

            signalBus.Fire<DependenciesLoadedSignal>();
        }

        private IEnumerable<UniTask> LoadDependencies()
        {
            Debug.Log("Loading dependencies");
            var dependencies = new List<UniTask> {assetFactory.LoadAssets()};


            return dependencies;
        }
    }
}