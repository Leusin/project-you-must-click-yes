using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace ProjectYouMustClickYes
{
    public class CRTTransitionController : MonoBehaviour
    {
        public Volume postVolume;
        public float duration = 0.3f;

        ChromaticAberration chromatic;
        Vignette vignette;
        LensDistortion distortion;
        
        void Awake()
        {
            postVolume.profile.TryGet(out chromatic);
            postVolume.profile.TryGet(out vignette);
            postVolume.profile.TryGet(out distortion);
        }

        // Test 용
        public void PlayOut()
        {
            StartCoroutine(AnimateCRTOut(null));
        }

        public void PlayOut(System.Action onComplete = null)
        {
            StartCoroutine(AnimateCRTOut(onComplete));
        }

        public void PlayIn()
        {
            StartCoroutine(AnimateCRTIn(null));
        }

        public void PlayIn(System.Action onComplete = null)
        {
            StartCoroutine(AnimateCRTIn(null));
        }

        IEnumerator AnimateCRTOut(System.Action onComplete)
        {
            float time = 0f;

            while (time < duration)
            {
                float t = time / duration;

                chromatic.intensity.value = Mathf.Lerp(0.183f, 1f, t);
                vignette.intensity.value = Mathf.Lerp(0.45f, 1f, t);
                distortion.intensity.value = Mathf.Lerp(0.25f, -1f, t);;
                distortion.scale.value = Mathf.Lerp(1f, 0.01f, t);

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
                chromatic.intensity.value = Mathf.Lerp(1f, 0.183f, t);
                vignette.intensity.value = Mathf.Lerp(1f, 0.45f, t);
                distortion.intensity.value = Mathf.Lerp(-1f, 0.25f, t);
                distortion.scale.value = Mathf.Lerp(0.01f, 1f, t);

                time += Time.deltaTime;
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}