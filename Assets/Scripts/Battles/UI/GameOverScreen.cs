using System;
using Battles.Entities.Player;
using Battles.Scoring;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Battles.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerScore;
        
        [SerializeField]
        private TextMeshProUGUI highScore;

        [SerializeField]
        private Button mainMenuButton;

        private IScoreProvider scoreProvider;
        private SignalBus signalBus;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IScoreProvider scoreProvider)
        {
            this.signalBus = signalBus;
            this.scoreProvider = scoreProvider;

            signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(OnMainButtonClicked);
        }

        private void OnMainButtonClicked()
        {
            Debug.Log("Clicked");
            signalBus.Fire<LoadMainMenuSignal>();
        }

        private void OnPlayerDied()
        {
            playerScore.text = scoreProvider.GetScore().ToString();
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            gameObject.SetActive(false);   
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            mainMenuButton.onClick.RemoveListener(OnMainButtonClicked);
        }
    }
}