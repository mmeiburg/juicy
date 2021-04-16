using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    public sealed class TimingAttribute : PropertyAttribute
    {
        public TimingAttribute()
        {
            Style = TimingStyle.None;
        }

        public TimingAttribute(TimingStyle style)
        {
            Style |= style;
        }

        public TimingStyle Style { get; } = TimingStyle.None;

        [Flags]
        public enum TimingStyle
        {
            None = 0,
            HideDuration = 1,
            HideCooldown = 2,
            HideDelay = 4,
            HideIgnoreTimeScale = 8
        }
    }
}