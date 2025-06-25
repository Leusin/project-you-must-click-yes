using UnityEngine;

namespace ProjectYouMustClickYes
{
    public class Level0EventController : MonoBehaviour
    {
        void Start()
        {
            SoundManager.Instance.AmbientBGM();
            SoundManager.Instance.StopBGM();
        }
    }
}