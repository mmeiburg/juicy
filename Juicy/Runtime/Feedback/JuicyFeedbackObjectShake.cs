using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Object/Shake")][AddComponentMenu("")]
    public class JuicyFeedbackObjectShake : JuicyFeedbackObjectBase
    {
        [SerializeField] private TransformTarget target = new TransformTarget();
        [SerializeField] private Vector3 value = Vector3.one * 20f;
        [SerializeField] private int vibrato = 10;
        [Range(0,90), SerializeField] private float randomness = 45f;

        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }
        
        private void PlayDelayed()
        {
            if (tween != null && tween.IsActive() && tween.IsPlaying()) {
                return;
            }

            tween = target.Value
                .DOShakeRotation(timing.duration, value, vibrato, randomness)
                .SetEase(ease.curve)
                .SetUpdate(timing.ignoreTimeScale);
        }
    }
}