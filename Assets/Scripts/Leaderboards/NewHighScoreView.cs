using Highscores;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Leaderboards
{
    public class NewHighScoreView : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField nameInput;

        [SerializeField]
        private Button confirmButton;

        private IHighScoresKeeper highScoresKeeper;

        [Inject, UsedImplicitly]
        private void Construct(IHighScoresKeeper highScoresKeeper)
        {
            this.highScoresKeeper = highScoresKeeper;
        }

        private void Start()
        {
            nameInput.onSubmit.AddListener(OnNameSubmited);
            confirmButton.onClick.AddListener(OnNameConfirmed);

            gameObject.SetActive(highScoresKeeper.HasPendingHighScore());
        }

        private void OnNameConfirmed()
        {
            OnNameSubmited(nameInput.text);
        }

        private void OnNameSubmited(string name)
        {
            if (name.Trim().Length == 0)
            {
                return;
            }

            highScoresKeeper.AddNewHighScore(name);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            nameInput.onSubmit.RemoveListener(OnNameSubmited);
            confirmButton.onClick.RemoveListener(OnNameConfirmed);
        }
    }
}