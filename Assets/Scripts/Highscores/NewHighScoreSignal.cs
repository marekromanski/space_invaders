using UnityEngine;

namespace Highscores
{
    public class NewHighScoreSignal
    {
        public readonly int highScore;

        public NewHighScoreSignal(int highScore)
        {
            Debug.Log("New HighScore signal");
            this.highScore = highScore;
        }
    }
}