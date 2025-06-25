using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ProjectYouMustClickYes
{
    public class SceneController : MonoBehaviour
    {
        public CRTTransitionController transitionController;
        public GameObject loadingUI;
        private bool _isTransitioning = false;
        public void Start()
        {
            loadingUI.SetActive(false);
            transitionController.PlayIn();
        }

        public void LoadSceneWithTransition(string nextScene)
        {
            if (_isTransitioning)
            {
                return;
            }

            _isTransitioning = true;

            SoundManager.Instance.PlayTransition();
            transitionController.PlayOut(() =>
            {
                loadingUI.SetActive(true);  // 1. 트랜지션 끝나면 로딩 화면 보이기
                StartCoroutine(LoadScene(nextScene));   // 2. 씬 로딩 시작
            });
        }

        public void QuitGame()
        {
            transitionController.PlayIn(() => Application.Quit());

            // 에디터에서 실행 중일 경우 강제 종료
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        IEnumerator LoadScene(string nextScene)
        {
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
            op.allowSceneActivation = false;

            // 로딩이 90% 될 때까지 기다리기
            // while (op.progress < 0.9f)
            // {
            //     yield return null;
            // }

            // 연출 대기
            yield return new WaitForSeconds(1f);

            // 씬 활성화
            op.allowSceneActivation = true;
            _isTransitioning = false;
        }
    }
}