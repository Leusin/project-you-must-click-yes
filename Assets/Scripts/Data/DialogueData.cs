using System.Collections.Generic;

namespace ProjectYouMustClickYes
{
    [System.Serializable]
    public class DialogueData
    {
        public List<DialogueEntry> dialogues;
    }

    [System.Serializable]
    public class DialogueEntry
    {
        public string sceneName;
        public List<string> dialogueList;
    }
}