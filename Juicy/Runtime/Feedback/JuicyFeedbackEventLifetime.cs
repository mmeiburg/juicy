using System;
using UnityEngine;
using UnityEngine.Events;
using static TinyTools.Juicy.TimingAttribute.TimingStyle;

namespace TinyTools.Juicy
{
    [Feedback("Event/Lifetime")][AddComponentMenu("")]
    public class JuicyFeedbackEventLifetime : JuicyFeedbackBase
    {
        [SerializeField] private LifetimeEvent onAwake = new LifetimeEvent();
        [SerializeField] private LifetimeEvent onStart = new LifetimeEvent();
        [SerializeField] private LifetimeEvent onEnable = new LifetimeEvent();
        [SerializeField] private LifetimeEvent onDisable = new LifetimeEvent();

        private void Awake()
        {
            onAwake.Invoke(this);
        }

        private void Start()
        {
            onStart.Invoke(this);
        }

        private void OnEnable()
        {
            onEnable.Invoke(this);
        }

        private void OnDisable()
        {
            onDisable.Invoke(this);
        }
    }

    [Serializable]
    public class LifetimeEvent : ToggleGroup
    {
        [SerializeField, Timing(HideDuration | HideCooldown | HideIgnoreTimeScale)] private Timing timing = new Timing();
        [SerializeField] private UnityEvent _event = new UnityEvent();

        public void Invoke(MonoBehaviour mono)
        {
            if (!isActive) {
                return;
            }
            timing.Invoke(mono, () => _event.Invoke());
        }
    }
}