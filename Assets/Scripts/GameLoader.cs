using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class GameLoader
{
    [UsedImplicitly]
    public GameLoader()
    {
        LoadGame().Forget();
    }

    private async UniTask LoadGame()
    {
        await UniTask.Delay(100);
        Debug.Log("Game Loaded");
    }
}
