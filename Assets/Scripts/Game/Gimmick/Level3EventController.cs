using UnityEngine;
using Leusin.Tools;

namespace ProjectYouMustClickYes
{
    // 첫 번째 레벨의 모든 이벤트를 관리하는 하드코딩용 클래스
    public class Level3EventController : MonoBehaviour
    {
        [Header("Setting")]
        public string nextLevel = SceneName.Lobby;
        public PopupUIController popupUIController;

        private void Start()
        {
            SoundManager.Instance.AmbientBGM();
            SoundManager.Instance.StopBGM();

            // 씬 이동
            popupUIController.OnLoopEnd.AddListener(() => popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition(nextLevel))));
            popupUIController.noButton.onClick.AddListener(() => popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.Lobby))));
        }
    }
}