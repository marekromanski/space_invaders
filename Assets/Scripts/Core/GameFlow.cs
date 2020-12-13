using System;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core
{
    [UsedImplicitly]
    public class GameFlow : IDisposable
    {
        private const int MainMenuSceneIndex = 1;
        private const int BattleSceneIndex = 2;

        private readonly SignalBus signalBus;

        public GameFlow(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            signalBus.Subscribe<DependenciesLoadedSignal>(OnDependenciesLoaded);
            signalBus.Subscribe<StartGameSignal>(OnStartGameSignalReceived);
        }

        private void OnDependenciesLoaded()
        {
            LoadScene(MainMenuSceneIndex);
        }

        private void OnStartGameSignalReceived()
        {
            LoadScene(BattleSceneIndex);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<DependenciesLoadedSignal>(OnDependenciesLoaded);
            signalBus.Unsubscribe<StartGameSignal>(OnStartGameSignalReceived);
        }

        private static void LoadScene(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        }
    }
}