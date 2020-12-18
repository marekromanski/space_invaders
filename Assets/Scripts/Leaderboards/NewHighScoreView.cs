using Highscores;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace Leaderboards
{
    public class NewHighScoreView : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField nameInput;

        private HighScoresKeeper highScoresKeeper;

        [Inject, UsedImplicitly]
        private void Construct(HighScoresKeeper highScoresKeeper)
        {
            this.highScoresKeeper = highScoresKeeper;
        }

        private void Start()
        {
            nameInput.onSubmit.AddListener(OnNameSubmited);
        }

        private void OnNameSubmited(string name)
        {
            highScoresKeeper.AddNewHighScore(name);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            nameInput.onSubmit.RemoveListener(OnNameSubmited);
        }
    }
}