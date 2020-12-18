﻿using System;
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

            signalBus.Subscribe<DependenciesLoadedSignal>(LoadMainMenu);
            signalBus.Subscribe<StartGameSignal>(OnStartGameSignalReceived);
            signalBus.Subscribe<LoadMainMenuSignal>(LoadMainMenu);
        }

        private void LoadMainMenu()
        {
            LoadScene(MainMenuSceneIndex);
        }

        private void OnStartGameSignalReceived()
        {
            LoadScene(BattleSceneIndex);
        }

        private static void LoadScene(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<DependenciesLoadedSignal>(LoadMainMenu);
            signalBus.Unsubscribe<StartGameSignal>(OnStartGameSignalReceived);
            signalBus.Unsubscribe<LoadMainMenuSignal>(LoadMainMenu);
        }
    }
}