
using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class ReferenceTarget<T, TTarget> 
        where T : Component 
        where TTarget : Target<T>, new()
    {
        [SerializeField] private Type type = Type.ByReference;
        [SerializeField] private string name = string.Empty;
        [SerializeField] private T reference = null;
        
        [SerializeField] private TTarget target = new TTarget();

        private T cachedReference;

        public bool IsValid => type == Type.ByName ?
            !name.Equals(string.Empty) && target.IsValid :
            target.IsValid;
        
        private enum Type
        {
            ByName,
            ByReference,
        }
        
        public T Value() {
            
            switch (type) {
                case Type.ByName:

                    if (name.Equals(string.Empty)) {
                        reference = null;
                    }
                    
                    #if !UNITY_EDITOR
                    if (cachedReference != null) {
                        reference = cachedReference;
                    }
                    #endif
                    
                    GameObject obj = GameObject.Find(name);

                    if (obj == null) {
                        throw new UnityException($"Could not find an object with name '{name}'");
                    }

                    if (obj.TryGetComponent(out T component)) {
                        reference = cachedReference = component;
                    } else {
                        reference = null;
                    }
                    
                    break;
                case Type.ByReference:
                    reference = target.Value;
                    break;
                    
            }

            return reference;
        }
    }
    
    [Serializable]
    public sealed class LightReferenceTarget : ReferenceTarget<Light, LightTarget> {}
    [Serializable]
    public sealed class CameraReferenceTarget : ReferenceTarget<Camera, CameraTarget> {}
}