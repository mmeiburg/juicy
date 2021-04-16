using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackObjectCreator<T> : JuicyFeedbackBase where T : Component
    {
        [SerializeField, Timing(TimingAttribute.TimingStyle.HideDuration)] private Timing timing = new Timing();

        [Tooltip("Prefab to instantiate")] [SerializeField]
        protected T prefab;
        [Tooltip("Spawn probability")]
        [SerializeField, Range(0, 1)] private float probability = 1f;
        [Tooltip("Set a custom position")]
        [SerializeField] private CustomPosition customPosition = new CustomPosition();
        [Tooltip("Set a custom rotation")]
        [SerializeField] private CustomRotation customRotation = new CustomRotation();
        [Tooltip("Parent the particle system")]
        [SerializeField] private CustomParent parent = new CustomParent();

        protected T objectReference;
        protected Transform cachedTransform;

        private void Awake()
        {
            cachedTransform = transform;
        }

        protected override void Play()
        {
            if (Random.value >= probability) {
                return;
            }
            
            timing.Invoke(this, PlayDelayed);
        }

        internal override void Pause()
        {
            base.Pause();
            timing.Pause();
        }

        protected virtual T Instantiate()
        {
            return Instantiate(prefab.gameObject).GetComponent<T>();
        }

        private void PlayDelayed()
        {
            objectReference = Instantiate();
            var systemTransform = objectReference.transform;
            
            Vector3 spawnPosition = cachedTransform.position;
            Vector3 offset = customPosition.offset;

            if (customPosition.isActive) {
                if (customPosition.spawnAt != null) {
                    spawnPosition = customPosition.spawnAt.position;
                } else {
                    var worldOffset = cachedTransform.rotation * offset;
                    spawnPosition = cachedTransform.position + worldOffset;
                }
                
                systemTransform.position = spawnPosition;
                systemTransform.localPosition += offset;
            }
            
            systemTransform.position = spawnPosition;
            
            if (parent.isActive) {
                objectReference.transform.SetParent(parent.parentTransform.Value);
            }
            
            if (customRotation.isActive) {
                systemTransform.localRotation = customRotation.rotateForward ?
                    systemTransform.rotation : Quaternion.Euler(customRotation.rotation);
            }
        }
    }
    
    [Serializable]
    public class CustomRotation : ToggleGroup
    {
        [Tooltip("Rotation value relative to the caller")]
        public Vector3 rotation;
        [Tooltip("Rotates in the caller forward direction")]
        public bool rotateForward;
    }
    
    [Serializable]
    public class CustomParent : ToggleGroup
    {
        [Tooltip("Parents to a specific transform")]
        public TransformTarget parentTransform;
    }
}