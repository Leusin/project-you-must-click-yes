using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        public string nextLevel;
        public int dialogueIndex = 0;
        public UnityEvent changeIndex;
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
            LoadDialogues();

            yesButton.onClick.AddListener(ChangeText);
            noButton.onClick.AddListener(MoveNoButtonAboveYes);
            noButton.onClick.AddListener(() => { _animator.SetTrigger(_hashEndNo); });
            noButton.onClick.AddListener(() => { StartCoroutine(WaitForAnimationAndLoadScene("Start")); });

            if (dialogues.Count > 0)
            {
                dialogueText.text = dialogues[0];
            }
        }

        private void OnDestroy()
        {
            yesButton.onClick.RemoveListener(ChangeText);
            noButton.onClick.RemoveAllListeners();
        }

        void LoadDialogues()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "dialogues.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                DialogueData data = JsonUtility.FromJson<DialogueData>(json);

                DialogueEntry entry = data.dialogues.Find(d => d.sceneName == SceneManager.GetActiveScene().name);

                if (entry != null)
                {
                    dialogues = entry.dialogueList;
                }
                else
                {
                    //dialogues.Add(SceneManager.GetActiveScene().name);
                    Debug.LogError("해당 씬에 대한 대화 데이터가 없습니다.");
                }
            }
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
                _animator.SetTrigger(_hashEndYes);
                StartCoroutine(WaitForAnimationAndLoadScene(nextLevel));
            }
        }

        // 마무리 애니메이션이 끝날 때까지 기다린 후 레벨 이동
        public IEnumerator WaitForAnimationAndLoadScene(string sceneName)
        {
            yield return new WaitForSeconds(4.4f);

            SceneManager.LoadScene(sceneName);
        }

        IEnumerator ChangeTextAfterAnimation()
        {
            // 애니메이션 실행
            _animator.SetTrigger(_hashLoopYes);

            // 애니메이션이 끝날 때까지 대기 (애니메이션 클립 길이에 맞게 설정)
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length + 0.2f);

            dialogueIndex++;
            changeIndex?.Invoke();
            
            // 텍스트 변경
            OnChangeTextEvnet?.Invoke();
            dialogueText.text = dialogues[dialogueIndex];
        }

        // 예 버튼을 위로 바꾸는 함수
        public void MoveYesButtonAboveNo()
        {
            yesButton.transform.SetAsLastSibling();
            //noButton.transform.SetAsFirstSibling();
        }

        // 아니오 버튼을 위로 바꾸는 함수
        public void MoveNoButtonAboveYes()
        {
            //yesButton.transform.SetAsFirstSibling();
            noButton.transform.SetAsLastSibling();

        }
    }
}