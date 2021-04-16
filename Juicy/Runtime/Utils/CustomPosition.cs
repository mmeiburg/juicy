using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Serializable]
    public class CustomPosition : ToggleGroup
    {
        [Tooltip("Relative offset of the new particle effect")]
        public Vector3 offset;
        [Tooltip("Spawns the particle at a specific transform")]
        public Transform spawnAt;
    }
}