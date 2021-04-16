using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Serializable]
    public sealed class Timing
    {
        [Tooltip("Total duration of the effect")]
        public float duration = 0.4f;
        [Tooltip("Duration till the effect starts")]
        public float delay = 0;
        [Tooltip("Duration after the effect can start again")] 
        public float cooldown = 0;
        [Tooltip("Marks if the effect should be played in realtime")]
        public bool ignoreTimeScale = false;

        private float currentTimestamp = 0;

        private bool HasDelay => delay > 0;
        private bool HasCooldown => cooldown > 0;

        public void Invoke(MonoBehaviour mono, Action callback)
        {
            if (!mono.gameObject.activeInHierarchy) {
                return;
            }
            
            if (HasCooldown && currentTimestamp > JuicyUtils.Time(ignoreTimeScale)) {
                return;
            }
            
            if (!HasDelay) {
                callback.Invoke();
                return;
            }
            
            currentTimestamp = JuicyUtils.Time(ignoreTimeScale) + cooldown;
            
            mono.InvokeDelayed(delay, callback, ignoreTimeScale);
        }

        public void Pause()
        {
        }

        public override string ToString()
        {
            return $"Timing: {duration} : {delay} : {cooldown} : {ignoreTimeScale}";
        }
    }
}