using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using JetBrains.Annotations;
using Leaderboards;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Highscores
{
    public class HighScoresKeeper : IHighScoresKeeper, IDisposable
    {
        private readonly SignalBus signalBus;

        private readonly List<HighScoreEntry> entries;
        private const string HighScoresKey = "high_scores";

        private int? pendingHighScore;

        [UsedImplicitly]
        public HighScoresKeeper(SignalBus signalBus)
        {
            this.signalBus = signalBus;

            entries = LoadHighScores();

            signalBus.Subscribe<NewHighScoreSignal>(OnNewHighScore);
        }

        private void OnNewHighScore(NewHighScoreSignal signal)
        {
            pendingHighScore = signal.highScore;
        }

        private List<HighScoreEntry> LoadHighScores()
        {
            if (PlayerPrefs.HasKey(HighScoresKey))
            {
                var json = PlayerPrefs.GetString(HighScoresKey);
                var serializable = JsonUtility.FromJson<EntriesList>(json);
                return serializable.entries;
            }
            else
            {
                return new List<HighScoreEntry>();
            }
        }

        private void SaveEntries(List<HighScoreEntry> entries)
        {
            var serializable = new EntriesList(entries);

            var json = JsonUtility.ToJson(serializable);
            PlayerPrefs.SetString(HighScoresKey, json);
            PlayerPrefs.Save();
        }

        public bool IsHighScore(int score)
        {
            if (entries.Count < ProjectConsts.LeaderboarEntries)
            {
                return true;
            }

            foreach (var entry in entries)
            {
                if (score > entry.score)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<NewHighScoreSignal>(OnNewHighScore);
        }

        public void AddNewHighScore(string name)
        {
            Assert.IsTrue(pendingHighScore.HasValue);
            var newEntry = new HighScoreEntry(name, pendingHighScore.Value);
            pendingHighScore = null;

            var newEntryIndex = FindIndexOfNewEntry(newEntry.score);
            entries.Insert(newEntryIndex, newEntry);
            SaveEntries(entries);

            signalBus.Fire<NewHighScoreInsertedSignal>();
        }

        public bool HasPendingHighScore()
        {
            return pendingHighScore.HasValue;
        }

        public List<HighScoreEntry> GetHighScores()
        {
            return entries.ToList();
        }

        private int FindIndexOfNewEntry(int newEntryScore)
        {
            for (int i = 0; i < entries.Count; ++i)
            {
                if (newEntryScore > entries[i].score)
                {
                    return i;
                }
            }

            if (entries.Count < ProjectConsts.LeaderboarEntries)
            {
                return entries.Count;
            }

            throw new Exception("We should not be here");
        }
    }

    // This class is needed for serialization purposes
    [Serializable]
    public class EntriesList
    {
        [SerializeField]
        public List<HighScoreEntry> entries;

        public EntriesList(List<HighScoreEntry> entries)
        {
            this.entries = entries;
        }
    }
}