using System;
using System.Collections;
using UnityEngine;

namespace TinyTools.Juicy
{
    public static class MonoBehaviourExtensions
    {
        public static Coroutine InvokeDelayed(this MonoBehaviour mono, float delay, Action callback, bool ignoreTimeScale = false)
        {
            return mono.StartCoroutine(Delay(callback, delay, ignoreTimeScale));
        }

        private static IEnumerator Delay(Action action, float delay, bool ignoreTimeScale)
        {
            if (ignoreTimeScale) {
                yield return new WaitForSecondsRealtime(delay); 
            } else {
                yield return new WaitForSeconds(delay);
            }
            
            action.Invoke();
        }
    }
}