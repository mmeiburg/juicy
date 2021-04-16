using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class FromToValue<T>
    {
        public bool isFrom = false;
        public T value;
    }
    
    [Serializable] public sealed class BoolFromToValue : FromToValue<bool> {}

    [Serializable]
    public sealed class FloatFromToValue : FromToValue<float>
    {
        public FloatFromToValue() {}
        public FloatFromToValue(float value)
        {
            this.value = value;
        }
    }
    [Serializable] public sealed class ColorChooserFromToValue : FromToValue<ColorChooser> {}
    [Serializable] public sealed class ColorFromToValue : FromToValue<Color> {}
    [Serializable] public sealed class Vector3FromToValue : FromToValue<Vector3> {}
    [Serializable] public sealed class Vector4FromToValue : FromToValue<Vector4> {}

    [Serializable]
    public sealed class SliderFromToValue : FromToValue<float>
    {
        public Vector3 range;

        public SliderFromToValue()
        {
            range = new Vector3(0,0,1);
        }

        public SliderFromToValue(float from, float to)
        {
            range = new Vector3(from, 0, to);
        }
    }
}