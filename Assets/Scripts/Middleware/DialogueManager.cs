using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leusin.Tools;

namespace ProjectYouMustClickYes
{
    public enum Language
    {
        KR,
        EN
    }

    public class DialogueManager : MonoBehaviourSingleton<DialogueManager>
    {
        public Language currentLang = Language.KR;
        
        public DialogueEntry LoadDialogueEntry()
        {
            string path = ResourcePaths.GetDialogue(currentLang);
            TextAsset jsonAsset = Resources.Load<TextAsset>(path);

            if (jsonAsset == null)
            {
                Debug.LogError($"리소스를 찾을 수 없습니다: {path}");
                return null;
            }

            DialogueData data;
            try
            {
                data = JsonUtility.FromJson<DialogueData>(jsonAsset.text);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"JSON 파싱 오류: {e.Message}");
                return null;
            }

            string currentSceneName = SceneManager.GetActiveScene().name;
            DialogueEntry entry = data.dialogues.Find(d => d.sceneName == currentSceneName);

            if (entry != null)
            {
                return entry;
            }

            Debug.LogError($"해당 씬({currentSceneName})에 대한 대화 데이터가 없습니다.");
            return null;
        }

    }
}