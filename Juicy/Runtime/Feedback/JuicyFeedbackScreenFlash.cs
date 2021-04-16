using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TinyTools.Juicy
{
    [Feedback("Screen/Flash")][AddComponentMenu("")]
    public class JuicyFeedbackScreenFlash : JuicyFeedbackBase
    {
        [SerializeField] private ColorFromToValue value = new ColorFromToValue();
        [SerializeField, Timing] protected Timing timing = new Timing();
        [SerializeField] protected Ease ease = new Ease();
        [SerializeField] protected ColorReset resetValue = new ColorReset();

        private Image image;
        
        protected override void Play()
        {
            image = JuicyScreenFlasher.Instance.Image;
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            tween?.Kill();
            image.color = new Color(0,0,0,0);

            float duration = timing.duration;
            
            if (resetValue.resetType == ResetType.Yoyo) {
                duration /= 2;
            }

            tween = image
                .DOColor(value.value, duration)
                .SetEase(ease.curve)
                .SetUpdate(timing.ignoreTimeScale);
            
            if (value.isFrom) {
                tween.From();
            }
            
            switch (resetValue.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(resetValue.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.SetLoops(resetValue.loop ? -1 : 1, LoopType.Restart);
                    break;
            }
        }
    }
}