using UnityEngine;
using Leusin.Tools;

namespace ProjectYouMustClickYes
{
    // 첫 번째 레벨의 모든 이벤트를 관리하는 하드코딩용 클래스
    public class Level1EventController : MonoBehaviour
    {
        [Header("Setting")]
        public string nextLevel = SceneName.Level2;
        public PopupUIController popupUIController;

        [Header("Emerge No Button")]
        [SerializeField] private int showNoAtIndex = 8;

        private void Start()
        {
            SoundManager.Instance.PlayBGM();

            // 씬 이동
            popupUIController.OnLoopEnd.AddListener(() => popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition(nextLevel))));
            popupUIController.noButton.onClick.AddListener(() => popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.Lobby))));

            // 이벤트 할당
            popupUIController.OnChangeTextEvnet.AddListener(ChangeTextEvnet);
            popupUIController.noButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            popupUIController.OnChangeTextEvnet.RemoveListener(ChangeTextEvnet);
        }

        public void ChangeTextEvnet()
        {
            
            if (popupUIController.dialogueIndex >= showNoAtIndex)
            {
                popupUIController.noButton.gameObject.SetActive(true);
            }
            else
            {
                popupUIController.noButton.gameObject.SetActive(false);
            }
            Canvas.ForceUpdateCanvases();
        }
    }
}