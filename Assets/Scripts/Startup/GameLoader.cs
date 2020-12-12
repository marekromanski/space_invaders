using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Startup
{
    public class GameLoader
    {
        private const int MainMenuIndex = 1;

        [UsedImplicitly]
        public GameLoader()
        {
            LoadGame().Forget();
        }

        private async UniTask LoadGame()
        {
            var loadSceneTask = SceneLoader.LoadScene(MainMenuIndex);
            var depenencies = LoadDependencies();

            await UniTask.WhenAll(depenencies);
            Debug.Log("Dependencies loaded");

            var result = await loadSceneTask;
            Debug.Log("Main Menu Scene loaded");
            result.allowSceneActivation = true;
            Debug.Log("Game Loaded");
        }

        private IEnumerable<UniTask> LoadDependencies()
        {
            Debug.Log("Loading dependencies");
            var dependencies = new List<UniTask>();

            return dependencies;
        }
    }
}