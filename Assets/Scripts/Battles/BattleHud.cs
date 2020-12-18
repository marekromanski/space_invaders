using Battles.Entities.Player;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace Battles
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
        }

        private void OnPlayerLivesChanged(PlayerLivesAmountChangedSignal signal)
        {
            playerLives.text = signal.livesRemaining.ToString();
        }

        void Start()
        {
            currentScore.text = 0.ToString();
        }
    }
}