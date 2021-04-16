using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Animator")][AddComponentMenu("")]
    public class JuicyFeedbackAnimator : JuicyFeedbackBase
    {
        [SerializeField, Timing] private Timing timing = new Timing();

        [SerializeField] private AnimatorTarget target = new AnimatorTarget();
        
        [SerializeField] private bool boolValue = false;
        [SerializeField] private int intValue = 0;
        [SerializeField] private float floatValue = 0;
        
        [SerializeField] private int hash = 0;
        [SerializeField] private AnimatorControllerParameterType type =
            AnimatorControllerParameterType.Float;
#pragma warning disable 0414  
        [SerializeField] private int selectedIndex = 0;
#pragma warning restore 0414  

        [SerializeField] private Ease ease = new Ease();

        protected override void Play()
        {
            if (!target.IsValid) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        private void PlayDelayed()
        {
            /*if (tween != null && tween.IsPlaying()) {
                tween.ChangeValues(0, 10,
                    timing.duration);
            }*/
            switch (type) {
                case AnimatorControllerParameterType.Int:
                    
                    tween = DOTween.To(() => target.Value.GetInteger(hash),
                        x => target.Value.SetInteger(hash, x),
                        intValue, timing.duration
                    );
                    
                    break;
                case AnimatorControllerParameterType.Float:

                    tween = DOTween.To(() => target.Value.GetFloat(hash),
                        x => target.Value.SetFloat(hash, x),
                        floatValue, timing.duration
                    );
                    
                    //target.Value.SetFloat(hash, floatValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    target.Value.SetTrigger(hash);
                    break;
                case AnimatorControllerParameterType.Bool:
                    target.Value.SetBool(hash, boolValue);
                    break;
            }

            tween.SetEase(ease.curve);
        }
    }
}