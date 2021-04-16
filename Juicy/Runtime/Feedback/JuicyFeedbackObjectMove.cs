using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Object/Move")][AddComponentMenu("")]
    public class JuicyFeedbackObjectMove : JuicyFeedbackObjectBase
    {
        [SerializeField] private TransformTarget target = new TransformTarget();
        [SerializeField] private Vector3Reset reset = new Vector3Reset();
        [SerializeField] private Vector3FromToValue value = new Vector3FromToValue();
        [SerializeField] private bool relative = false;
        [SerializeField] private bool snapping = false;
        
        private Transform targetTransform;
        
        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            targetTransform = target.Value;

            float duration = timing.duration;

            if (reset.resetType == ResetType.Yoyo) {
                duration /= 2;
            }
            
            if (relative) {
                tween = targetTransform.DOBlendableMoveBy(
                    value.value,
                    duration,
                    snapping
                );
            } else {
                tween = targetTransform.DOMove(
                    value.value,
                    duration,
                    snapping);
            }
            
            if (value.isFrom) {
                tween.From();
            } else {
                tween.SetRelative(relative);
            }    

            switch (reset.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(reset.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.onComplete += () => targetTransform.position = reset.resetValue;
                    tween.SetLoops(reset.loop ? -1 : 1, LoopType.Restart);
                    break;
            }

            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }
    }
}