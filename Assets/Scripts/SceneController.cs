using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectYouMustClickYes
{
    public class SceneController : MonoBehaviour
    {
        public void LoadNextScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            Application.Quit();

            // 에디터에서 실행 중일 경우 강제 종료
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

    }
}