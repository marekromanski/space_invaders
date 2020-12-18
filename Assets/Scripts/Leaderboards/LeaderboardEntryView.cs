using Highscores;
using TMPro;
using UnityEngine;

namespace Leaderboards
{
    public class LeaderboardEntryView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI number;

        [SerializeField]
        private TextMeshProUGUI name;

        [SerializeField]
        private TextMeshProUGUI score;

        public void Setup(int index, HighScoreEntry entry)
        {
            number.text = (index + 1).ToString();
            name.text = entry.name;
            score.text = entry.score.ToString();
        }
    }
}