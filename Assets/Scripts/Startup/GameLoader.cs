using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Startup
{
    public class GameLoader
    {
        private const int MainMenuIndex = 1;
        private const int LoadSceneCheckInterval = 500;

        [UsedImplicitly]
        public GameLoader()
        {
            LoadGame().Forget();
        }

        private async UniTask LoadGame()
        {
            var loadSceneTask = LoadMainMenu();

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

        private async UniTask<AsyncOperation> LoadMainMenu()
        {
            var loadSceneOperation = SceneManager.LoadSceneAsync(MainMenuIndex, LoadSceneMode.Single);

            loadSceneOperation.allowSceneActivation = false;
            while (!loadSceneOperation.isDone)
            {
                await UniTask.Delay(LoadSceneCheckInterval);
            }

            return loadSceneOperation;
        }
    }
}
