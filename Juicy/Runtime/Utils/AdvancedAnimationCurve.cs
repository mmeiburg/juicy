using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Serializable]
    public class AdvancedAnimationCurve
    {
        public AnimationCurve curve = new AnimationCurve(
            new Keyframe(0, 0), 
            new Keyframe(1, 1));
    }
}