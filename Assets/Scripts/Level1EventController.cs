using UnityEngine;
using UnityEngine.UI;

namespace ProjectYouMustClickYes
{
    // 첫 번째 레벨의 모든 이벤트를 관리하는 하드코딩용 클래스
    public class Level1EventController : MonoBehaviour
    {
        [Header("Setting")]
        public PopupUIController popupUIController;

        [Header("Emerge No Button")]
        [SerializeField] private int showNoAtIndex = 8;

        private void Start()
        {
            popupUIController.OnChangeTextEvnet.AddListener(ChangeTextEvnet);
        }

        private void OnDestroy()
        {
            popupUIController.OnChangeTextEvnet.RemoveListener(ChangeTextEvnet);
        }

        public void ChangeTextEvnet()
        {
            
            if (popupUIController.dialogueIndex >= showNoAtIndex)
            {
                popupUIController.noButton.gameObject.SetActive(true);
            }
            else
            {
                popupUIController.noButton.gameObject.SetActive(false);
            }
            Canvas.ForceUpdateCanvases();
        }
    }
}