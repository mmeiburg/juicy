using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Renderer/Tint")][AddComponentMenu("")]
    public class JuicyFeedbackObjectTint : JuicyFeedbackObjectBase
    {
        [SerializeField] private RendererTarget target = new RendererTarget();
        [SerializeField] private ColorFromToValue color = new ColorFromToValue();
        [SerializeField] private ColorReset resetColor = new ColorReset();
        
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

            if (resetColor.resetType == ResetType.Yoyo) {
                duration /= 2;
            }
            
            if (target.Value is SpriteRenderer r) {
                CreateSpriteRendererTween(r, duration);

            } else {
                CreateDefaultRendererTween(duration);
            }
            
            if (color.isFrom) {
                tween.From();
            }
            
            switch (resetColor.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(resetColor.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.SetLoops(resetColor.loop ? -1 : 1, LoopType.Restart);
                    break;
            }
            
            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }

        private void CreateSpriteRendererTween(SpriteRenderer r, float duration)
        {
            tween = r.DOBlendableColor(color.value, duration);
                
            if (resetColor.resetType == ResetType.ToValue) {
                tween.onComplete = () => { r.color =
                    resetColor.resetValue; };
            }
        }

        private void CreateDefaultRendererTween(float duration)
        {
            material = target.Value.material;

            if (material == null) {
                return;
            }

            tween = material.DOBlendableColor(color.value, duration);
                    
            if (resetColor.resetType == ResetType.ToValue) {
                tween.onComplete = () => { material.color =
                    resetColor.resetValue; };
            }
        }
    }
}