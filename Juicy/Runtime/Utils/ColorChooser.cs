using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Serializable]
    public sealed class ColorChooser
    {
        public bool useHdr = false;
        [ColorUsage(true, true)]
        public Color hdrColor = Color.white;
        public Color rgbColor = Color.white;

        public Color Value => useHdr ? hdrColor : rgbColor;
    }
}