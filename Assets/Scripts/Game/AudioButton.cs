using UnityEngine;
using UnityEngine.UI;

namespace ProjectYouMustClickYes
{
    public class AudioButton : MonoBehaviour
    {
        private Button button;

        void Awake()
        {
            button = GetComponent<Button>();
        }

        void Start()
        {
            SoundManager.Instance.muteButtonImage = button.image;

            if (button)
            {
                button.onClick.AddListener(SoundManager.Instance.ToggleMute);
            }
        }
    }
}
