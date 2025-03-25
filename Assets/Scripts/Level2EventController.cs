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
        [SerializeField] private RectTransform YesbuttonRect;
        private Vector2 originalPosition; // 버튼의 원래 위치
        private int repeat = 0; // 이동 횟수

        [Header("Changne Position Button")]
        [SerializeField] private RectTransform nobuttonRect;
        //[Header("Check Box")]
        //[Header("Upside Down")]
        //[Header("Move Like Screensaver")]
        //[Header("Text Sake")]

        void Start()
        {
            originalPosition = YesbuttonRect.anchoredPosition;
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
                if (repeat == 0)
                {
                    popupMask.enabled = false;
                    popupAnimator.enabled = false;
                }

                // 마무리
                if (repeat == loop)
                {
                    popupMask.enabled = true;
                    popupAnimator.enabled = true;
                    YesbuttonRect.anchoredPosition = originalPosition;
                    popupUIController.yesButton.onClick.AddListener(popupUIController.ChangeText);
                }

                // 이상 이동
                if (repeat < loop)
                {
                    float randomX = Random.Range(-popupRect.rect.width / 2 + YesbuttonRect.rect.width / 2, popupRect.rect.width / 2 - YesbuttonRect.rect.width / 2);
                    float randomY = Random.Range(-popupRect.rect.height / 2 + YesbuttonRect.rect.height / 2, popupRect.rect.height / 2 - YesbuttonRect.rect.height / 2);
                    YesbuttonRect.anchoredPosition = new Vector2(randomX, randomY);

                    repeat++;
                }
            }
        }
    }
}