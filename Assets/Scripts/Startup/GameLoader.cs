using System.Collections.Generic;
using AssetManagement;
using Battles;
using Core;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
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

            var player = assetFactory.GetAsset<GameObject>();
            Assert.IsNotNull(player);
            var playerComponent = player.GetComponent<Player>();
            Assert.IsNotNull(playerComponent);

            signalBus.Fire<DependenciesLoadedSignal>();

            Debug.Log("Game Loaded");
        }

        private IEnumerable<UniTask> LoadDependencies()
        {
            Debug.Log("Loading dependencies");
            var dependencies = new List<UniTask>();

            dependencies.Add(assetFactory.LoadAssets());

            return dependencies;
        }
    }
}