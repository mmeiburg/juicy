using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class Reset<T>
    {
        public ResetType resetType = ResetType.Stay;
        public bool loop = false;
        public T resetValue;
    }
    
    [Serializable]
    public class Vector3Reset : Reset<Vector3> {}
    [Serializable]
    public class Vector4Reset : Reset<Vector4> {}
    [Serializable]
    public class ColorReset : Reset<Color> {}
    [Serializable]
    public class ColorChooserReset : Reset<ColorChooser> {}
    [Serializable]
    public class FloatReset : Reset<float> {}
    
    public enum ResetType
    {
        Stay,
        ToValue,
        Yoyo
    }
    
    
}