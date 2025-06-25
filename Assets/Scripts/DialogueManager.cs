using System.Collections.Generic;
using UnityEngine;

namespace ProjectYouMustClickYes
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
        public Language currentLang = Language.KR;

        void Awake()
        {
            // 싱글턴 패턴
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}