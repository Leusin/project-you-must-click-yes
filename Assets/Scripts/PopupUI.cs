using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectYouMustClickYes
{
    public class PopupUI : MonoBehaviour
    {
        public TextMeshProUGUI dialogueText;
        public Button yesButton;
        private int dialogueIndex = 0;
        private List<string> dialogues = new List<string>();

        void Start()
        {
            LoadDialogues();
            yesButton.onClick.AddListener(ChangeText);
            if (dialogues.Count > 0)
            {
                dialogueText.text = dialogues[0];
            }
        }

        void LoadDialogues()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "dialogues.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                DialogueData data = JsonUtility.FromJson<DialogueData>(json);
                dialogues = data.dialogues;
            }
        }

        void ChangeText()
        {
            if (dialogueIndex < dialogues.Count - 1)
            {
                dialogueIndex++;
                dialogueText.text = dialogues[dialogueIndex];
            }
        }
    }
}