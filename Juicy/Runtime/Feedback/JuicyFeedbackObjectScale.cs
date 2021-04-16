using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Object/Scale")][AddComponentMenu("")]
    public class JuicyFeedbackObjectScale : JuicyFeedbackObjectBase
    {
        [SerializeField] private TransformTarget target = new TransformTarget();
        [SerializeField] private Vector3FromToValue value = new Vector3FromToValue();
        [SerializeField] private Vector3Reset reset = new Vector3Reset();
        //[SerializeField] private float resetUniformValue = 1;
        
        //[SerializeField] private bool uniformScale;
        [SerializeField] private bool relative = false;

        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            /*Vector3 endScale = uniformScale
                ? Vector3.one * uniformValue.value
                : value.value;
*/
            Vector3 endScale = value.value;
            
            float duration = timing.duration;

            if (reset.resetType == ResetType.Yoyo) {
                duration /= 2;
            }
            
            tween = relative ? target.Value.DOBlendableScaleBy(endScale, duration) :
                target.Value.DOScale(endScale, duration);
            
            switch (reset.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(reset.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.onComplete += () => target.Value.localScale = reset.resetValue;
                    tween.SetLoops(reset.loop ? -1 : 1, LoopType.Restart);
                    break;
            }
            
            if (value.isFrom) {
                tween.From();
            } else {
                tween.SetRelative(relative);
            }

            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }
    }
}