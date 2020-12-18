using Core;
using Highscores;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace Leaderboards
{
    public class LeaderboardsView : MonoBehaviour
    {
        [SerializeField]
        private Button mainMenuButton;

        [SerializeField]
        private LeaderboardEntryView[] entryViews;

        private IHighScoresKeeper highScoresKeeper;
        private SignalBus signalBus;

        private void Awake()
        {
            Assert.IsTrue(entryViews.Length == ProjectConsts.LeaderboarEntries);
        }

        [Inject, UsedImplicitly]
        void Construct(IHighScoresKeeper highScoresKeeper, SignalBus signalBus)
        {
            this.highScoresKeeper = highScoresKeeper;
            this.signalBus = signalBus;
        }

        private void Start()
        {
            DisplyEntries();

            mainMenuButton.onClick.AddListener(ShowMainMenu);
            signalBus.Subscribe<NewHighScoreInsertedSignal>(DisplyEntries);
        }

        private void ShowMainMenu()
        {
            signalBus.Fire<LoadMainMenuSignal>();
        }

        private void DisplyEntries()
        {
            var entries = highScoresKeeper.GetHighScores();

            int entriesCount = entries.Count;

            for (int i = 0; i < entriesCount; ++i)
            {
                entryViews[i].Setup(i, entries[i]);
            }

            var emptyEntry = new HighScoreEntry("...........", 0);
            for (int i = entriesCount; i < ProjectConsts.LeaderboarEntries; ++i)
            {
                entryViews[i].Setup(i, emptyEntry);
            }
        }

        private void OnDestroy()
        {
            mainMenuButton.onClick.RemoveListener(ShowMainMenu);
            signalBus.Unsubscribe<NewHighScoreInsertedSignal>(DisplyEntries);
        }
    }
}