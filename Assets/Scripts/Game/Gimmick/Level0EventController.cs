using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectYouMustClickYes
{
    public class Level0EventController : MonoBehaviour
    {
        [Header("Button")]
        public Button yesButton;
        public Button noButton;
        public Button audioButton;
        public Button credit;
        public Button language;
        public Sprite kr;
        public Sprite eng;

        [Header("TMP_Text")]
        public TMP_Text title;
        public TMP_Text dialogue;
        public TMP_Text yesText;
        public TMP_Text noText;

        void Start()
        {
            SoundManager.Instance.AmbientBGM();
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.muteButtonImage = audioButton.image;

            yesButton.onClick.AddListener(() => SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.Level1));
            noButton.onClick.AddListener(SceneTransitionManager.Instance.QuitGame);
            audioButton.onClick.AddListener(SoundManager.Instance.ToggleMute);
            credit.onClick.AddListener(() => SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.Credit));
            language.onClick.AddListener(ToggleLanguage);

            UpdateLanguageDisplay();
        }

        public void ToggleLanguage()
        {
            DialogueManager.Instance.currentLang = (DialogueManager.Instance.currentLang == Language.KR) ? Language.EN : Language.KR;

            UpdateLanguageDisplay();
        }

        private void UpdateLanguageDisplay()
        {
            DialogueEntry dialogueData = DialogueManager.Instance.LoadDialogueEntry();
            
            language.image.sprite = (DialogueManager.Instance.currentLang == Language.KR) ? kr : eng;

            title.text = dialogueData.dialogueList[3];
            dialogue.text = dialogueData.dialogueList[4];
            yesText.text = dialogueData.dialogueList[0];
            noText.text = dialogueData.dialogueList[1];
        }
    }
}