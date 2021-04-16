using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Time/Change")][AddComponentMenu("")]
    public class JuicyFeedbackTimeChange : JuicyFeedbackTimeBase
    {
        [SerializeField]
        private float amount = 0.5f;

        [SerializeField] private Ease ease = new Ease();

        protected override void Play()
        {
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            tween?.Kill();
            
            float value = Time.timeScale;
            
            tween = DOTween.To(() => value, x =>
                    {
                        Time.timeScale = Mathf.Clamp(x, 0, float.MaxValue);
                    }, amount, timing.duration / 2)
                .SetUpdate(true)
                //.OnUpdate(() => UpdateValue(value))
                .OnKill(ResetTimeScale)
                .OnComplete(ResetTimeScale)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(ease.curve);
        }
        
        private static void ResetTimeScale() => Time.timeScale = 1;
    }
}