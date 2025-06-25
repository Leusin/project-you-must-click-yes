using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ProjectYouMustClickYes
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        public AudioMixer bgmMixer;

        [Header("Audio Sources")]
        public AudioSource bgmSource;
        public AudioSource ambientSource;
        public AudioSource sfxSource;

        [Header("Audio Clips")]
        public AudioClip mouseClickClip;
        public AudioClip mouseReleaseClip;
        public AudioClip bgmClip;
        public AudioClip ambientClip;
        public AudioClip ClickNoClip;
        public AudioClip ClickYesClip;
        public AudioClip TransitionClip;
        
        [Header("Mute")]
        public Image muteButtonImage;        // 버튼 이미지 컴포넌트
        public Sprite muteSprite;             // 사운드 꺼짐 아이콘
        public Sprite unmuteSprite;           // 사운드 켜짐 아이콘
        private bool isMuted = false;

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

        void Start()
        {
            bgmSource.outputAudioMixerGroup = bgmMixer.FindMatchingGroups("BGM")[0];

            bgmSource.volume = 0.1f;
            ambientSource.volume = 0.1f;
            sfxSource.volume = 0.1f;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 누름
            {
                PlayMouseClick();
            }

            if (Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼 뗌
            {
                PlayMouseRelease();
            }
        }

        public void PlayBGM()
        {
            if (bgmClip != null)
            {
                bgmSource.clip = bgmClip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }

        public void StopBGM()
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }
        }

        public void AmbientBGM()
        {
            if (bgmClip != null)
            {
                ambientSource.clip = ambientClip;
                ambientSource.loop = true;
                ambientSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        // 특정 사운드 전용 호출
        public void PlayMouseClick() => PlaySFX(mouseClickClip);
        public void PlayMouseRelease() => PlaySFX(mouseReleaseClip);
        public void PlayTransition() => PlaySFX(TransitionClip);
        public void PlayYes() => PlaySFX(ClickYesClip);
        public void PlayNo() => PlaySFX(ClickNoClip);

        public bool IsMuted() => isMuted;

        public void ToggleMute()
        {
            isMuted = !isMuted;

            bgmSource.mute = isMuted;
            ambientSource.mute = isMuted;
            sfxSource.mute = isMuted;

            UpdateButtonImage();
        }

        private void UpdateButtonImage()
        {
            if (muteButtonImage == null)
            {
                return;
            }

            muteButtonImage.sprite = isMuted ? muteSprite : unmuteSprite;
        }
    }
}