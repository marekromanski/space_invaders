using System;
using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class PlayButton : MonoBehaviour
    {
        private SignalBus signalBus;
        private Button button;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus)
        {
            this.signalBus = signalBus;
        }
        private void Awake()
        {
            button = GetComponent<Button>();
            Assert.IsNotNull(button);
            
            button.onClick.AddListener(SendStartGameSignal);
        }

        private void SendStartGameSignal()
        {
            signalBus.Fire<StartGameSignal>();
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(SendStartGameSignal);
        }
    }
}