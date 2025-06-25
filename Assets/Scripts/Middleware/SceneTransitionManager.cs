using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Leusin.Tools;
using UnityEngine.UI;

namespace ProjectYouMustClickYes
{
    public class SceneTransitionManager : MonoBehaviourSingleton<SceneTransitionManager>
    {
        private CRTTransitionController _transitionController;
        private GameObject _loadingUI;
        private bool _isTransitioning = false;

        protected override void OnAwake()
        {
            if (_transitionController == null)
            {
                gameObject.AddComponent<CRTTransitionController>();
                _transitionController = GetComponent<CRTTransitionController>();
            }

            if (_loadingUI == null)
            {
                _loadingUI = new GameObject();
                _loadingUI.name =nameof(_loadingUI);
                _loadingUI.AddComponent<Canvas>();
                _loadingUI.AddComponent<CanvasScaler>();
                _loadingUI.AddComponent<GraphicRaycaster>();
                _loadingUI.AddComponent<CanvasRenderer>();
                _loadingUI.AddComponent<Image>();
                DontDestroyOnLoad(_loadingUI);
                _loadingUI.SetActive(false);
            }
        }

        public void LoadSceneWithTransition(string nextScene)
        {
            if (_isTransitioning)
            {
                return;
            }

            _isTransitioning = true;

            SoundManager.Instance.PlayTransition();
            _transitionController.PlayOut(() =>
            {
                _loadingUI.SetActive(true);  // 1. 트랜지션 끝나면 로딩 화면 보이기
                SceneManager.sceneLoaded += OnNextSceneLoaded;
                StartCoroutine(LoadScene(nextScene));   // 2. 씬 로딩 시작
            });
        }

        public void QuitGame()
        {
            _transitionController.PlayIn(() => Application.Quit());

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
            while (op.progress < 0.9f)
            {
                yield return null;
            }

            // 연출 대기
            yield return new WaitForSeconds(1f);

            // 씬 활성화
            op.allowSceneActivation = true;
            _isTransitioning = false;
        }
        
        private void OnNextSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnNextSceneLoaded;

            // 새 씬의 Start 직전에 실행됨
            _loadingUI.SetActive(false);

            _transitionController.PlayIn();  
        }
    }
    
}