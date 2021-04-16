using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackObjectBase : JuicyFeedbackBase
    {
        [SerializeField, Timing] protected Timing timing = new Timing();
        [SerializeField] protected Ease ease = new Ease();
    }
}