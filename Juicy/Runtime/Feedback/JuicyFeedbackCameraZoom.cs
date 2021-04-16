using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Camera/Zoom")][AddComponentMenu("")]
    public class JuicyFeedbackCameraZoom : JuicyFeedbackCameraBase
    {
        [SerializeField, Timing] private Timing timing = new Timing();
        [SerializeField] private FloatFromToValue fieldOfView =
            new FloatFromToValue(30f);

        [SerializeField] private FloatReset reset = new FloatReset();
        [SerializeField] private bool relative = false;
        [SerializeField] private Ease ease = new Ease();
        
        protected override void Play()
        {
            if (camera.Value() == null) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }
        
        private void PlayDelayed()
        {
            float duration = timing.duration;

            if (reset.resetType == ResetType.Yoyo) {
                duration /= 2;
            }

            if (relative) {
                tween = camera.Value().DOBlendableFieldOfView(
                    fieldOfView.value, duration);
            } else {
                tween = camera.Value().DOFieldOfView(
                    fieldOfView.value, duration);
            }
            
            if (fieldOfView.isFrom) {
                tween.From();
            } else {
                tween.SetRelative(relative);
            }   
            
            switch (reset.resetType) {
                case ResetType.Yoyo:
                    tween.SetLoops(reset.loop ? -1 : 2, LoopType.Yoyo);
                    break;
                case ResetType.ToValue:
                    tween.onComplete += () => camera.Value().fieldOfView = reset.resetValue;
                    tween.SetLoops(reset.loop ? -1 : 1, LoopType.Restart);
                    break;
            }

            tween.SetUpdate(timing.ignoreTimeScale);
            tween.SetEase(ease.curve);
        }
    }
}