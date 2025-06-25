using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace ProjectYouMustClickYes
{
    public class CRTTransitionController : MonoBehaviour
    {
        public Volume _crtVolume;
        public float duration = 0.3f;

        // 효과 컴포넌트
        ChromaticAberration chromatic;
        Vignette vignette;
        LensDistortion distortion;

        // Lerp 범위 값들
        [Header("Chromatic Aberration")]
        public float chromaticStart = 0.183f;
        public float chromaticEnd = 1f;

        [Header("Vignette")]
        public float vignetteStart = 0.45f;
        public float vignetteEnd = 1f;

        [Header("Lens Distortion - Intensity")]
        public float distortionStart = 0.25f;
        public float distortionEnd = -1f;

        [Header("Lens Distortion - Scale")]
        public float distortionScaleStart = 1f;
        public float distortionScaleEnd = 0.01f;

        void Awake()
        {
            if (_crtVolume == null)
            {
                GameObject obj = new GameObject(nameof(_crtVolume));
                _crtVolume = obj.AddComponent<Volume>();
                DontDestroyOnLoad(obj);

                VolumeProfile volumeProfileAsset = Resources.Load<VolumeProfile>(ResourcePaths.MainVolume);
                if (volumeProfileAsset != null)
                {
                    _crtVolume.profile = volumeProfileAsset;
                }
            }

            _crtVolume.profile.TryGet(out chromatic);
            _crtVolume.profile.TryGet(out vignette);
            _crtVolume.profile.TryGet(out distortion);
        }

        void Start()
        {
            ResetEffectToStartValues();
        }

        void ResetEffectToStartValues()
        {
            if (chromatic != null)
            {
                chromatic.intensity.value = chromaticStart;
            }
            if (vignette != null)
            {
                vignette.intensity.value = vignetteStart;
            }
            if (distortion != null)
            {
                distortion.intensity.value = distortionStart;
                distortion.scale.value = distortionScaleStart;
            }
        }

        public void PlayOut(System.Action onComplete = null)
        {
            StartCoroutine(AnimateCRTOut(onComplete));
        }

        public void PlayIn(System.Action onComplete = null)
        {
            StartCoroutine(AnimateCRTIn(onComplete));
        }

        IEnumerator AnimateCRTOut(System.Action onComplete)
        {
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                chromatic.intensity.value = Mathf.Lerp(chromaticStart, chromaticEnd, t);
                vignette.intensity.value = Mathf.Lerp(vignetteStart, vignetteEnd, t);
                distortion.intensity.value = Mathf.Lerp(distortionStart, distortionEnd, t);
                distortion.scale.value = Mathf.Lerp(distortionScaleStart, distortionScaleEnd, t);

                time += Time.deltaTime;
                yield return null;
            }

            onComplete?.Invoke();
        }

        IEnumerator AnimateCRTIn(System.Action onComplete)
        {
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                chromatic.intensity.value = Mathf.Lerp(chromaticEnd, chromaticStart, t);
                vignette.intensity.value = Mathf.Lerp(vignetteEnd, vignetteStart, t);
                distortion.intensity.value = Mathf.Lerp(distortionEnd, distortionStart, t);
                distortion.scale.value = Mathf.Lerp(distortionScaleEnd, distortionScaleStart, t);

                time += Time.deltaTime;
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}
