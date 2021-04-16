using UnityEngine;

namespace TinyTools.Juicy
{
    public sealed class MinMaxRange : PropertyAttribute
    {
        public float min = 0f;
        public float max = 1f;
        
        public MinMaxRange() {}

        internal MinMaxRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}