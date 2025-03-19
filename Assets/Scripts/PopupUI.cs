using System.Collections;
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
        public Button noButton;
        public Button yesButton;
        private int dialogueIndex = 0;
        private List<string> dialogues = new List<string>();
        private Animator _animator;

        readonly int _hashLoopYes = Animator.StringToHash("loop_yes");
        readonly int _hashEndNo = Animator.StringToHash("end_no");
        readonly int _hashEndYes = Animator.StringToHash("end_yes");

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            LoadDialogues();
            
            yesButton.onClick.AddListener(ChangeText);
            noButton.onClick.AddListener(() => { _animator.SetTrigger(_hashEndNo); });

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

                StartCoroutine(ChangeTextAfterAnimation());
            }
            else
            {
                // 마무리 애니메이션 실행
                _animator.SetTrigger(_hashEndYes);
            }
        }

        IEnumerator ChangeTextAfterAnimation()
        {
            // 애니메이션 실행
            _animator.SetTrigger(_hashLoopYes);

            // 애니메이션이 끝날 때까지 대기 (애니메이션 클립 길이에 맞게 설정)
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);

            // 텍스트 변경
            dialogueText.text = dialogues[dialogueIndex];
        }

        // 예 버튼을 위로 바꾸는 함수
        public void MoveYesButtonAboveNo()
        {
            yesButton.transform.SetAsLastSibling();
            noButton.transform.SetAsFirstSibling();
        }

        // 아니오 버튼을 위로 바꾸는 함수
        public void MoveNoButtonAboveYes()
        {
            yesButton.transform.SetAsFirstSibling();
            noButton.transform.SetAsLastSibling();

        }
    }
}