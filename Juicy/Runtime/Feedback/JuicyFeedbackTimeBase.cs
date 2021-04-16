using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackTimeBase : JuicyFeedbackBase
    {
        [SerializeField, Timing(TimingAttribute.TimingStyle.HideCooldown |
                                TimingAttribute.TimingStyle.HideIgnoreTimeScale)] 
        protected Timing timing = new Timing();
    }
}