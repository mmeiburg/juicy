using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Light/Color")][AddComponentMenu("")]
    public class JuicyFeedbackLightColor : JuicyFeedbackLightBase
    {
        [SerializeField] private ColorFromToValue color = new ColorFromToValue();
        [SerializeField] private ColorReset resetValue = new ColorReset();

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
            
            tween = target.Value().DOBlendableColor(color.value, duration);
            
            if (color.isFrom) {
                tween.From();
            }

            if (resetValue.resetType == ResetType.ToValue) {
                tween.onComplete += () => target.Value().color = resetValue.resetValue;
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