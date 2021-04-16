using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Time/Freeze")][AddComponentMenu("")]
    public class JuicyFeedbackTimeFreeze : JuicyFeedbackTimeBase
    {
        protected override void Play()
        {
            timing.Invoke(this, PlayDelayed);
        }
        
        private void PlayDelayed()
        {
            tween?.Kill();
            
            float value = 0;
            tween = DOTween.To(() => value, x => value = x, 1, 
                    timing.duration)
                .OnStart(() => Time.timeScale = 0f)
                .OnComplete(ResetTimeScale)
                .OnKill(ResetTimeScale)
                .SetUpdate(true);
        }
        
        private static void ResetTimeScale() => Time.timeScale = 1;
    }
}