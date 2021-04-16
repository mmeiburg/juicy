using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackLightBase : JuicyFeedbackBase
    {
        [SerializeField, Timing] protected Timing timing = new Timing();
        [SerializeField] protected LightReferenceTarget target = new LightReferenceTarget();
        [SerializeField] protected Ease ease = new Ease();
    }
}