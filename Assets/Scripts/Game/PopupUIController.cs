using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace ProjectYouMustClickYes
{
    public class PopupUIController : MonoBehaviour
    {
        public TextMeshProUGUI dialogueText;
        public Button noButton;
        public Button yesButton;
        public Toggle checkBox;
        public int dialogueIndex = 0;
        public UnityEvent changeIndex;
        public UnityEvent OnLoopEnd;
        public List<string> dialogues = new List<string>();

        public UnityEvent OnChangeTextEvnet;
        private Animator _animator;

        readonly int _hashLoopYes = Animator.StringToHash("loop_yes");
        readonly int _hashEndNo = Animator.StringToHash("end_no");
        readonly int _hashEndYes = Animator.StringToHash("end_yes");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            DialogueEntry dialogueData = DialogueManager.Instance.LoadDialogueEntry();
            dialogues = dialogueData.dialogueList.Count > 2 ? dialogueData.dialogueList.Skip(2).ToList() : new List<string>();
            
            string sceneName = SceneManager.GetActiveScene().name;

            TMP_Text yesText = yesButton.GetComponentInChildren<TMP_Text>();
            TMP_Text noText = noButton.GetComponentInChildren<TMP_Text>();

            yesText.text = dialogueData.dialogueList[0];
            noText.text = dialogueData.dialogueList[1];

            yesButton.onClick.AddListener(ChangeText);

            noButton.onClick.AddListener(SoundManager.Instance.PlayNo);
            noButton.onClick.AddListener(MoveNoButtonAboveYes);
            noButton.onClick.AddListener(() => { _animator.SetTrigger(_hashEndNo); });

            if (dialogues.Count > 0)
            {
                dialogueText.text = dialogues[0];
            }

            StartCoroutine(PlayAfterDelay(1.5f, "Show Popup"));
        }
        
        private void OnDestroy()
        {
            yesButton.onClick.RemoveListener(ChangeText);
            noButton.onClick.RemoveAllListeners();
        }

        // Yes 버튼을 눌렀을 경우
        public void ChangeText()
        {
            if (dialogueIndex < dialogues.Count - 1)
            {
                StartCoroutine(ChangeTextAfterAnimation());
            }
            else
            {
                // 마무리 애니메이션 실행
                SoundManager.Instance.PlayYes();
                _animator.SetTrigger(_hashEndYes);
                OnLoopEnd?.Invoke();
            }
        }

        IEnumerator ChangeTextAfterAnimation()
        {
            // 애니메이션 실행
            _animator.SetTrigger(_hashLoopYes);

            // 애니메이션이 끝날 때까지 대기 (애니메이션 클립 길이에 맞게 설정)
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length + 0.25f);

            dialogueIndex++;
            changeIndex?.Invoke();

            // 텍스트 변경
            OnChangeTextEvnet?.Invoke();
            dialogueText.text = dialogues[dialogueIndex];

            // 버튼 선택 해제
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

        // 예 버튼을 위로 바꾸는 함수
        public void MoveYesButtonAboveNo()
        {
            yesButton.transform.SetAsLastSibling();
        }

        // 아니오 버튼을 위로 바꾸는 함수
        public void MoveNoButtonAboveYes()
        {
            noButton.transform.SetAsLastSibling();
        }

        IEnumerator PlayAfterDelay(float delay, string stateName)
        {
            yield return new WaitForSeconds(delay);
            _animator.Play(stateName);
        }
    }
}