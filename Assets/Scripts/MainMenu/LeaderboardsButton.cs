using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace MainMenu
{
    public class LeaderboardsButton : MonoBehaviour
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

            button.onClick.AddListener(SendShowLeaderboardsSignal);
        }

        private void SendShowLeaderboardsSignal()
        {
            signalBus.Fire<ShowLeaderboardsSignal>();
            Debug.Log("Sent show leaderboards");
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(SendShowLeaderboardsSignal);
        }
    }
}