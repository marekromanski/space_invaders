using System.Collections.Generic;

namespace Highscores
{
    public interface IHighScoresKeeper
    {
        bool IsHighScore(int score);
        void AddNewHighScore(string name);
        bool HasPendingHighScore();
        List<HighScoreEntry> GetHighScores();
        int GetCurrenHighScore();
    }
}