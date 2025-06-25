using UnityEngine;

using System;
using System.Collections;

namespace Leusin.Tools
{
    public static class CoroutineUtil
    {
        public static IEnumerator WaitFor(float seconds, Action onComplete = null)
        {
            yield return new WaitForSeconds(seconds);
            onComplete?.Invoke();
        }
    }
}