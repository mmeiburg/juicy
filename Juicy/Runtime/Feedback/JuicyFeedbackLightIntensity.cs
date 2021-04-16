using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Light/Intensity")][AddComponentMenu("")]
    public class JuicyFeedbackLightIntensity : JuicyFeedbackLightBase
    {
        [SerializeField] private FloatFromToValue intensity = new FloatFromToValue();
        [SerializeField] private FloatReset resetValue = new FloatReset();
        
        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }
        
        private void PlayDelayed()
        {   
            float duration = timing.duration;

            if (resetValue.resetType == ResetType.Yoyo) {
                duration /= 2;
            }
            
            tween = target.Value().DOBlendableIntensity(intensity.value, duration);
            
            if (intensity.isFrom) {
                tween.From();
            }
            
            if (resetValue.resetType == ResetType.ToValue) {
                tween.onComplete = () => { target.Value().intensity =
                    resetValue.resetValue; };
            }
            
            switch (resetValue.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(resetValue.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.SetLoops(resetValue.loop ? -1 : 1, LoopType.Restart);
                    break;
            }
            
            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }
    }
}