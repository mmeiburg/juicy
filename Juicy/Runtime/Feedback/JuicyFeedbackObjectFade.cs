using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Renderer/Fade")][AddComponentMenu("")]
    public class JuicyFeedbackObjectFade : JuicyFeedbackObjectBase
    {
        [SerializeField] private RendererTarget target = new RendererTarget();
        [SerializeField] private SliderFromToValue value = new SliderFromToValue();
        [SerializeField] private FloatReset resetValue = new FloatReset();
        
        private Material material;

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
            
            if (target.Value is SpriteRenderer r) {
                tween = r.DOBlendableFade(value.value, duration);
                
                if (resetValue.resetType == ResetType.ToValue) {
                    tween.onComplete = () => { r.DOFade(resetValue.resetValue, 0f);};
                }
                
            } else {

                material = target.Value.material;

                if (material != null) {
                    tween = material.DOBlendableFade(value.value, duration);
                    
                    if (resetValue.resetType == ResetType.ToValue) {
                        tween.onComplete = () => { material.DOFade(resetValue.resetValue, 0); };
                    }
                }
            }
            
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
            
            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }
    }
}