using UnityEngine;
using UnityEngine.Events;
using static TinyTools.Juicy.TimingAttribute.TimingStyle;

namespace TinyTools.Juicy
{
    [Feedback("Event/Simple")][AddComponentMenu("")]
    public class JuicyFeedbackEventSimple : JuicyFeedbackBase
    {
        [SerializeField, Timing(HideDuration)] private Timing timing = new Timing();
        [SerializeField] private UnityEvent _event = new UnityEvent();
        
        protected override void Play()
        {
            timing.Invoke(this, () => _event.Invoke());
        }
    }
}