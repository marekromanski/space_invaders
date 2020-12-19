using Battles.BattleField;
using Battles.Entities.Player;
using Battles.Scoring;
using Core;
using Highscores;
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
        private TextMeshProUGUI wavesBeaten;

        [SerializeField]
        private TextMeshProUGUI playerScore;

        [SerializeField]
        private TextMeshProUGUI highScore;

        [SerializeField]
        private Button mainMenuButton;
        
        [SerializeField]
        private Button addHighScoreButton;

        private SignalBus signalBus;
        private IScoreProvider scoreProvider;
        private IHighScoresKeeper highScoresKeeper;
        private IWavesCounter wavesCounter;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IWavesCounter wavesCounter, IScoreProvider scoreProvider, IHighScoresKeeper highScoresKeeper)
        {
            this.signalBus = signalBus;
            this.scoreProvider = scoreProvider;
            this.highScoresKeeper = highScoresKeeper;
            this.wavesCounter = wavesCounter;

            signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        private void Start()
        {
            gameObject.SetActive(false);   
        }

        private void Awake()
        {
            mainMenuButton.onClick.AddListener(OnMainButtonClicked);
            addHighScoreButton.onClick.AddListener(OnNewHighScoreClicked);
        }

        private void OnNewHighScoreClicked()
        {
            signalBus.Fire(new NewHighScoreSignal(scoreProvider.GetScore()));
        }

        private void OnPlayerDied()
        {
            wavesBeaten.text = wavesCounter.GetAmountOfWavesBeaten().ToString();
            highScore.text = highScoresKeeper.GetCurrenHighScore().ToString();

            var score = scoreProvider.GetScore();
            playerScore.text = score.ToString();
            DisplayApropriateExitButton(score);
            
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }

        private void DisplayApropriateExitButton(int score)
        {
            var isNewHighScore = highScoresKeeper.IsHighScore(score);
            mainMenuButton.gameObject.SetActive(!isNewHighScore);
            addHighScoreButton.gameObject.SetActive(isNewHighScore);
        }

        private void OnMainButtonClicked()
        {
            signalBus.Fire<LoadMainMenuSignal>();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            mainMenuButton.onClick.RemoveListener(OnMainButtonClicked);
            addHighScoreButton.onClick.RemoveListener(OnNewHighScoreClicked);
        }
    }
}