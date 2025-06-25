using UnityEngine;
using UnityEngine.UI;
using Leusin.Tools;

namespace ProjectYouMustClickYes
{
    public class Level2EventController : MonoBehaviour
    {
        [Header("Setting")]
        public string nextLevel = SceneName.Credit;
        public PopupUIController popupUIController;

        [Header("Teleport Button")]
        [SerializeField] private int indexButtonTeleport = 3;
        [SerializeField] private int loop = 4; // 이동 횟수
        [SerializeField] private Mask popupMask;
        [SerializeField] private Animator popupAnimator;
        [SerializeField] private RectTransform popupRect;
        [SerializeField] private RectTransform yesButtonRect;
        private Vector2 yesOriginalPosition; // 버튼의 원래 위치
        [SerializeField] private int _clicked = 0; // 이동 횟수

        [Header("Changne Buttons Position")]
        [SerializeField] private int[] _indexesChangePosition = { 0, 4 };
        readonly int _hashChagneButtonPos = Animator.StringToHash("event_chagne_buttons_pos");
        readonly int _hashEndCheckbox = Animator.StringToHash("evenet_end_checkbox");
        readonly int _hashShowReversed = Animator.StringToHash("event_show_reversed");

        [Header("Check Box")]
        [SerializeField] private int _indexCheckBox = 6;
        [SerializeField] private Toggle _toggle;

        [Header("Upside Down")]
        [SerializeField] private int _indexUpsideDown = 7;

        [Header("Move Like Screensaver")]
        [SerializeField] private int _indexOffScreenSaverMove = 8;
        [SerializeField] private ScreensaverMove _screenSaverMover;

        [Header("Text Sake")]
        [SerializeField] private int[] _indexesTextShake = { 10, 11, 12 };
        [SerializeField] private int[] _ShakeCurveScale = { 4, 8, 13 };
        [SerializeField] private VertexJitter _vertexJitter;

        void Start()
        {
            // 이벤트 관련 변수 초기화
            yesOriginalPosition = yesButtonRect.anchoredPosition;
            popupAnimator.applyRootMotion = false;

            // 씬 이동
            popupUIController.OnLoopEnd.AddListener(() => popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition(nextLevel))));
            popupUIController.noButton.onClick.AddListener(() => popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition("Start"))));

            // 체크박스 버튼 할당
            popupUIController.checkBox.onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.Lobby);
                    }
                });

            // 이벤트 할당
            popupUIController.yesButton.onClick.AddListener(TeleportButton);
            popupUIController.yesButton.onClick.AddListener(ChangneButtonsPosition);
            popupUIController.OnChangeTextEvnet.AddListener(Checkbox);
            popupUIController.OnChangeTextEvnet.AddListener(UpsideDown);
            popupUIController.OnChangeTextEvnet.AddListener(ScreensaverMove);
            popupUIController.OnChangeTextEvnet.AddListener(ShakeTexts);
        }

        public void TeleportButton()
        {
            // 조건 인덱스 진전 준비
            if (popupUIController.dialogueIndex == indexButtonTeleport - 1)
            {
                popupUIController.yesButton.onClick.RemoveListener(popupUIController.ChangeText);
                popupUIController.MoveYesButtonAboveNo();
            }

            // 조건 인덱스 때
            if (popupUIController.dialogueIndex == indexButtonTeleport)
            {
                if (_clicked == 0)
                {
                    popupMask.enabled = false;
                    popupAnimator.enabled = false;
                }

                // 마무리
                if (_clicked == loop)
                {
                    popupMask.enabled = true;
                    popupAnimator.enabled = true;
                    yesButtonRect.anchoredPosition = yesOriginalPosition;
                    popupUIController.yesButton.onClick.AddListener(popupUIController.ChangeText);
                    popupUIController.yesButton.onClick.RemoveListener(TeleportButton);

                    // 임시로 넣어봄. 어쩌면 그대로 둘 수도 있음
                    _screenSaverMover.enabled = true;
                }
                
                // 이상 이동
                if (_clicked < loop)
                {
                    float randomX = UnityEngine.Random.Range(-popupRect.rect.width / 1.5f + yesButtonRect.rect.width / 1.5f, popupRect.rect.width / 1.5f - yesButtonRect.rect.width / 2);
                    float randomY = UnityEngine.Random.Range(-popupRect.rect.height / 1.5f + yesButtonRect.rect.height / 1.5f, popupRect.rect.height / 1.5f - yesButtonRect.rect.height / 2);
                    yesButtonRect.anchoredPosition = new Vector2(randomX, randomY);
                }
                _clicked++;
            }
        }

        public void ChangneButtonsPosition()
        {
            foreach (int i in _indexesChangePosition)
            {
                if (popupUIController.dialogueIndex == i)
                {
                    popupAnimator.SetTrigger(_hashChagneButtonPos);
                }
            }
        }

        public void Checkbox()
        {
            if (popupUIController.dialogueIndex == _indexCheckBox)
            {
                //popupRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 236);
                _toggle.gameObject.SetActive(true);
            }

            if (popupUIController.dialogueIndex > _indexCheckBox)
            {
                popupUIController.yesButton.onClick.RemoveListener(Checkbox);

                //popupRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 220);
                _toggle.gameObject.SetActive(false);

            }
        }

        public void EndCheckbox()
        {
            popupAnimator.SetTrigger(_hashEndCheckbox);
            _toggle.interactable = false;
            popupUIController.StartCoroutine(CoroutineUtil.WaitFor(4.5f, () => SceneTransitionManager.Instance.LoadSceneWithTransition(SceneName.Lobby)));
        }

        public void UpsideDown()
        {
            if (popupUIController.dialogueIndex == _indexUpsideDown)
            {
                popupAnimator.SetTrigger(_hashShowReversed);
            }

            if (popupUIController.dialogueIndex > _indexUpsideDown)
            {
                popupUIController.yesButton.onClick.RemoveListener(UpsideDown);
            }
        }

        public void ScreensaverMove()
        {
            if (popupUIController.dialogueIndex == _indexOffScreenSaverMove)
            {
                _screenSaverMover.enabled = false;
                popupUIController.yesButton.onClick.RemoveListener(ScreensaverMove);
            }
            else if (popupUIController.dialogueIndex == _indexOffScreenSaverMove - 1)
            {
                _screenSaverMover.DoubleSpeed();
            }
        }

        public void ShakeTexts()
        {
            _vertexJitter.enabled = false;

            for (int i = 0; i < _indexesTextShake.Length; i++)
            {
                if (popupUIController.dialogueIndex == _indexesTextShake[i])
                {
                    _vertexJitter.enabled = true;
                    _vertexJitter.curveScale = _ShakeCurveScale[i];
                    break;
                }
            }
        }
    }
}