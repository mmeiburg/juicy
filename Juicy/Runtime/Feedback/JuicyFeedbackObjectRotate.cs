using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Object/Rotate")][AddComponentMenu("")]
    public class JuicyFeedbackObjectRotate : JuicyFeedbackObjectBase
    {
        [SerializeField] private TransformTarget target = new TransformTarget();
        [SerializeField] private Vector3FromToValue value = new Vector3FromToValue();
        [SerializeField] private Vector3Reset resetValue = new Vector3Reset();
        [SerializeField] private bool relative = false;
        [SerializeField] private RotateMode mode = RotateMode.Fast;

        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            Vector3 rotation = value.value;
            
            float duration = timing.duration;

            if (resetValue.resetType == ResetType.Yoyo) {
                duration /= 2;
            }
            
            tween = target.Value.DOBlendableRotateBy(rotation, duration, mode);
            
            if (value.isFrom) {
                tween.From();
            } else {
                tween.SetRelative(relative);
            }    

            switch (resetValue.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(resetValue.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.onComplete += () => target.Value.rotation =
                        Quaternion.Euler(resetValue.resetValue);
                    tween.SetLoops(resetValue.loop ? -1 : 1, LoopType.Incremental);
                    break;
                
            }

            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }
    }
}