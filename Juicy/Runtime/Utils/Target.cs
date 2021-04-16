using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    public class Target<T> where T : class
    {
        public bool targetIsSelf = false;
        public T selfTarget;
        public T otherTarget;

        public bool IsValid => Value != null;

        public T Value => targetIsSelf ? selfTarget : otherTarget;
    }
    
    [Serializable]
    public sealed class RendererTarget : Target<Renderer> {}
    [Serializable]
    public sealed class AnimatorTarget : Target<Animator> {}
    [Serializable]
    public sealed class TransformTarget : Target<Transform> {}
    [Serializable]
    public sealed class AudioSourceTarget : Target<AudioSource> {}
    [Serializable]
    public sealed class LightTarget : Target<Light> {}
    [Serializable]
    public sealed class CameraTarget : Target<Camera> {}
}