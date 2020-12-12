using System;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameFlow : IDisposable
    {
        private const int MainMenuSceneIndex = 1;
        private const int BattleSceneIndex = 2;

        private readonly SignalBus signalBus;

        public GameFlow(SignalBus signalBus)
        {
            this.signalBus = signalBus;
            Debug.Log("MainMenu controller created");

            signalBus.Subscribe<DependenciesLoadedSignal>(OnDependenciesLoaded);
            signalBus.Subscribe<StartGameSignal>(OnStartGameSignalReceived);
        }

        private async void OnDependenciesLoaded()
        {
            SceneLoader.LoadScene(MainMenuSceneIndex);
        }

        private async void OnStartGameSignalReceived()
        {
            SceneLoader.LoadScene(BattleSceneIndex);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<DependenciesLoadedSignal>(OnDependenciesLoaded);
            signalBus.Unsubscribe<StartGameSignal>(OnStartGameSignalReceived);
            Debug.Log("Disposed");
        }
    }
}