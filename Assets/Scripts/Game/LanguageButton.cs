using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectYouMustClickYes
{
    public enum Language
    {
        KR,
        ENG
    }

    public class LanguageButton : MonoBehaviour
    {
        // Setting
        public Sprite kr;
        public Sprite eng;

        private Button button;
        private Language currentLang;

        // Title
        public TMP_Text  title;
        public TMP_Text dialogue;
        public TMP_Text yes;
        public TMP_Text  no;


        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(ToggleLanguage);

            currentLang = Language.KR;
            UpdateLanguageDisplay();
        }

        public void ToggleLanguage()
        {
            currentLang = (currentLang == Language.KR) ? Language.ENG : Language.KR;
            button.image.sprite = (currentLang == Language.KR) ? kr : eng;

            DialogueManager.Instance.currentLang = currentLang;

            UpdateLanguageDisplay();
        }

        private void UpdateLanguageDisplay()
        {
            title.text = (currentLang == Language.KR) ? "반드시 '예' 버튼을 누르세요." : "You Must Click 'Yes'.";
            yes.text = (currentLang == Language.KR) ? "예" : "Yes";
            no.text = (currentLang == Language.KR) ? "아니오" : "No";
            dialogue.text = (currentLang == Language.KR) ? "게임을 시작하려면 '<b>예</b>' 버튼을 누르세요." : "To start the game, press the '<b>Yes</b>'.";
        }
    }
}
