using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Serializable]
    public sealed class Ease
    {
        public AnimationCurve curve = new AnimationCurve(
            new Keyframe(0, 0), 
            new Keyframe(1, 1));
    }
}