using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UiUtils
{
    [RequireComponent(typeof(Button))]
    public class MultiClickPreventer : MonoBehaviour
    {
        [SerializeField, Range(1, 1000)] private int inactiveDurationMs = 500;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            Assert.IsNotNull(button);
        }

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            button.interactable = false;

            RestoreInteractivityAfterDelay(inactiveDurationMs).Forget();
        }

        private async UniTask RestoreInteractivityAfterDelay(int delayMs)
        {
            await UniTask.Delay(delayMs);
            ReactivateButton();
        }

        private void ReactivateButton()
        {
            if (button != null)
            {
                button.interactable = true;
            }
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}