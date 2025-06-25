using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Leusin.Tools;

namespace ProjectYouMustClickYes
{
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        private AudioMixer _audioMixer;

        private AudioSource _bgmSource;
        private AudioSource _ambientSource;
        private AudioSource _sfxSource;

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

        protected override void OnAwake()
        {
            _audioMixer =  Resources.Load<AudioMixer>(ResourcePaths.MainAudioMixer);

            _bgmSource = gameObject.AddComponent<AudioSource>();
            _ambientSource = gameObject.AddComponent<AudioSource>();
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }
        
        void Start()
        {
            _bgmSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups(ResourcePaths.BgmGroup)[0];

            _bgmSource.volume = 0.1f;
            _ambientSource.volume = 0.1f;
            _sfxSource.volume = 0.1f;
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
                _bgmSource.clip = bgmClip;
                _bgmSource.loop = true;
                _bgmSource.Play();
            }
        }

        public void StopBGM()
        {
            if (_bgmSource.isPlaying)
            {
                _bgmSource.Stop();
            }
        }

        public void AmbientBGM()
        {
            if (bgmClip != null)
            {
                _ambientSource.clip = ambientClip;
                _ambientSource.loop = true;
                _ambientSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip != null)
            {
                _sfxSource.PlayOneShot(clip);
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

            _bgmSource.mute = isMuted;
            _ambientSource.mute = isMuted;
            _sfxSource.mute = isMuted;

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