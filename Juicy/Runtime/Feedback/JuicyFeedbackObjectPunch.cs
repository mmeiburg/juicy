using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Object/Punch")][AddComponentMenu("")]
    public class JuicyFeedbackObjectPunch : JuicyFeedbackObjectBase
    {
        [SerializeField] private TransformTarget target = new TransformTarget();
        [SerializeField] private Vector3FromToValue value = new Vector3FromToValue();
        [SerializeField] private int vibrato = 1;
        [Range(0,1), SerializeField] private float elasticity = 0.2f;

        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            tween = target.Value
                .DOBlendablePunchScaleBy(value.value, timing.duration, vibrato, elasticity);

            tween.SetEase(ease.curve);
            tween.SetUpdate(timing.ignoreTimeScale);
        }
    }
}