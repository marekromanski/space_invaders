using System;
using System.Collections.Generic;
using Battles;
using Battles.UI;
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
                return JsonUtility.FromJson <List<HighScoreEntry>>(json);
            }
            else
            {
                return new List<HighScoreEntry>();
            }
        }

        private void SaveEntries(List<HighScoreEntry> entries)
        {
            var json = JsonUtility.ToJson(entries);
            PlayerPrefs.SetString(HighScoresKey, json);
            PlayerPrefs.Save();
        }

        public bool IsHighScore(int score)
        {
            if (entries.Count < 10)
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

            var newEntryIndex = FindIndexOfNewEntry(newEntry.score);
            entries.Insert(newEntryIndex, newEntry);
            SaveEntries(entries);
            signalBus.Fire<NewHighScoreInsertedSignal>();
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

            if (entries.Count < 10)
            {
                return entries.Count;
            }
            
            throw new Exception("We should not be here");
        }
    }

    [Serializable]
    public class HighScoreEntry
    {
        public readonly string name;
        public readonly int score;

        public HighScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}