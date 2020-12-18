using Battles.Entities.Player;
using Battles.Scoring;
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
        private TextMeshProUGUI currentScore;

        [SerializeField]
        private TextMeshProUGUI highScore;

        private SignalBus signalBus;

        [Inject, UsedImplicitly]
        private void Construct(SignalBus signalBus, IPlayerConfiguration playerConfiguration)
        {
            this.signalBus = signalBus;
            playerLives.text = playerConfiguration.LivesTotal.ToString();
            
            signalBus.Subscribe<PlayerLivesAmountChangedSignal>(OnPlayerLivesChanged);
            signalBus.Subscribe<PlayerScoreChangedSignal>(OnPlayerScoreChanged);
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

        }
    }
}