using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public static class SceneLoader
    {
        private const int LoadSceneCheckInterval = 500;

        public static async UniTask<AsyncOperation> LoadScene(int sceneIndex)
        {
            var loadSceneOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);

            loadSceneOperation.allowSceneActivation = false;
            while (!loadSceneOperation.isDone)
            {
                await UniTask.Delay(LoadSceneCheckInterval);
            }

            return loadSceneOperation;
        }
    }
}