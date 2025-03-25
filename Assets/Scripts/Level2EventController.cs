using UnityEngine;
using UnityEngine.UI;

namespace ProjectYouMustClickYes
{
    public class Level2EventController : MonoBehaviour
    {
        [Header("Setting")]
        public PopupUIController popupUIController;

        [Header("Teleport Button")]
        [SerializeField] private int indexButtonTeleport = 2;
        [SerializeField] private int loop = 3; // 이동 횟수
        [SerializeField] private Mask popupMask;
        [SerializeField] private Animator popupAnimator;
        [SerializeField] private RectTransform popupRect;
        [SerializeField] private RectTransform yesButtonRect;
        private Vector2 yesOriginalPosition; // 버튼의 원래 위치
        [SerializeField] private int _cliked = 0; // 이동 횟수

        [Header("Changne Buttons Position")]
        [SerializeField] private int indexChangePosition = 7;
        readonly int _hashChagneButtonPos = Animator.StringToHash("chagne_button_pos");

        //[Header("Check Box")]
        //[Header("Upside Down")]
        //[Header("Move Like Screensaver")]
        //[Header("Text Sake")]

        void Start()
        {
            yesOriginalPosition = yesButtonRect.anchoredPosition;
        }

        public void TeleportButton()
        {
            // 조건 인덱스 진전 준비
            if (popupUIController.dialogueIndex == indexButtonTeleport - 1)
            {
                popupUIController.yesButton.onClick.RemoveListener(popupUIController.ChangeText);
            }

            // 조건 인덱스 때
            if (popupUIController.dialogueIndex == indexButtonTeleport)
            {
                if (_cliked == 0)
                {
                    popupMask.enabled = false;
                    popupAnimator.enabled = false;
                }

                // 마무리
                if (_cliked == loop)
                {
                    popupMask.enabled = true;
                    popupAnimator.enabled = true;
                    yesButtonRect.anchoredPosition = yesOriginalPosition;
                    popupUIController.yesButton.onClick.AddListener(popupUIController.ChangeText);
                    popupUIController.yesButton.onClick.RemoveListener(TeleportButton);
                }

                // 이상 이동
                if (_cliked < loop)
                {
                    float randomX = Random.Range(-popupRect.rect.width / 2 + yesButtonRect.rect.width / 2, popupRect.rect.width / 2 - yesButtonRect.rect.width / 2);
                    float randomY = Random.Range(-popupRect.rect.height / 2 + yesButtonRect.rect.height / 2, popupRect.rect.height / 2 - yesButtonRect.rect.height / 2);
                    yesButtonRect.anchoredPosition = new Vector2(randomX, randomY);

                }
                _cliked++;
            }
        }

        public void ChangneButtonsPosition()
        {
            if (popupUIController.dialogueIndex == indexChangePosition)
            {
                Debug.Log("자리를 바꿔라");
                popupAnimator.SetTrigger(_hashChagneButtonPos);
            }
        }
    }
}