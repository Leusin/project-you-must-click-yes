using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace ProjectYouMustClickYes
{
    public class CRTTransition : MonoBehaviour
    {
        public Volume postVolume;
        public float duration = 1f;

        ChromaticAberration chromatic;
        Vignette vignette;
        LensDistortion distortion;

        void Start()
        {
            postVolume.profile.TryGet(out chromatic);
            postVolume.profile.TryGet(out vignette);
            postVolume.profile.TryGet(out distortion);
        }

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
            StartCoroutine(AnimateCRTIn());
        }

        IEnumerator AnimateCRTOut(System.Action onComplete)
        {
            float time = 0f;
            while (time < duration)
            {
                float t = time / duration;
                chromatic.intensity.value = Mathf.Lerp(0f, 1f, t);
                vignette.intensity.value = Mathf.Lerp(0.2f, 0.6f, t);
                distortion.intensity.value = Mathf.Lerp(0f, -0.7f, t);
                time += Time.deltaTime;
                yield return null;
            }

            onComplete?.Invoke();
        }

        IEnumerator AnimateCRTIn()
        {
            float time = 0f;
            while (time < duration)
            {
                float t = time / duration;
                chromatic.intensity.value = Mathf.Lerp(1f, 0f, t);
                vignette.intensity.value = Mathf.Lerp(0.6f, 0.2f, t);
                distortion.intensity.value = Mathf.Lerp(-0.7f, 0f, t);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}