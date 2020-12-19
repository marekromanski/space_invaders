using Battles.Entities.Player;
using Battles.Scoring;
using Highscores;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace Battles.UI
{
    public class BattleHud : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerLives;

        [SerializeField]
        private TextMeshProUGUI waveNumber;

        [SerializeField]
        private TextMeshProUGUI currentScore;

        [SerializeField]
        private TextMeshProUGUI highScore;

        private SignalBus signalBus;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IPlayerConfiguration playerConfiguration, IHighScoresKeeper highScoresKeeper)
        {
            this.signalBus = signalBus;
            playerLives.text = playerConfiguration.LivesTotal.ToString();
            highScore.text = highScoresKeeper.GetCurrenHighScore().ToString();
            
            signalBus.Subscribe<PlayerLivesAmountChangedSignal>(OnPlayerLivesChanged);
            signalBus.Subscribe<PlayerScoreChangedSignal>(OnPlayerScoreChanged);
            signalBus.Subscribe<WaveSpawnedSignal>(OnWaveSpawned);
        }

        private void OnWaveSpawned(WaveSpawnedSignal signal)
        {
            waveNumber.text = signal.currentWave.ToString();
        }

        private void OnPlayerScoreChanged(PlayerScoreChangedSignal signal)
        {
            currentScore.text = signal.newScore.ToString();
        }

        private void OnPlayerLivesChanged(PlayerLivesAmountChangedSignal signal)
        {
            playerLives.text = signal.livesRemaining.ToString();
        }

        void Start()
        {
            currentScore.text = 0.ToString();
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<PlayerLivesAmountChangedSignal>(OnPlayerLivesChanged);
            signalBus.Unsubscribe<PlayerScoreChangedSignal>(OnPlayerScoreChanged);
            signalBus.Unsubscribe<WaveSpawnedSignal>(OnWaveSpawned);
        }
    }
}