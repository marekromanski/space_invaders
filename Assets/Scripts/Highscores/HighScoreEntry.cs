using System;
using UnityEngine;

namespace Highscores
{
    [Serializable]
    public class HighScoreEntry
    {
        [SerializeField]
        public string name;

        [SerializeField]
        public int score;

        public HighScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}