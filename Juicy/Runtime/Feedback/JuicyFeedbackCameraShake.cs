using System;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Camera/Shake")][AddComponentMenu("")]
    public class JuicyFeedbackCameraShake : JuicyFeedbackCameraBase
    {
        [SerializeField, Timing] private Timing timing = new Timing();
                
        /// <summary>
        /// Maximum distance in each direction the transform
        /// with translate during shaking.
        /// </summary>
        [SerializeField]
        private Vector3 maximumStrength = Vector3.one;
    
        /// <summary>
        /// Frequency of the Perlin noise function. Higher values
        /// will result in faster shaking.
        /// </summary>
        [SerializeField]
        private float frequency = 25;
    
        /// <summary>
        /// Higher values will result in a smoother shake falloff.
        /// </summary>
        [SerializeField]
        private float smoothness = 1;

        [SerializeField] private float recoverySpeed = 1f;
        
        [SerializeField] private UseRangeMultiplier useRangeMultiplier = new UseRangeMultiplier();
        [SerializeField] private Ease ease = new Ease();

        protected override void Play()
        {
            if (camera.Value() == null) {
                return;
            }
            timing.Invoke(this, PerformShakeDelayed);
        }

        private void PerformShakeDelayed()
        {
            float shakePower = 1f;
            
            if (useRangeMultiplier.isActive) {
                float distance = Vector3.Distance(transform.position, camera.Value().transform.position);
                float distance01 = Mathf.Clamp01(distance / useRangeMultiplier.range);
                shakePower = (1 - Mathf.Pow(distance01, 2)) * useRangeMultiplier.maximumPower;
            }

            JuicyCameraShaker.Instance(camera.Value()).Shake(new JuicyCameraShaker.ShakeProperties {
                power = shakePower,
                ignoreTimeScale = timing.ignoreTimeScale,
                smoothness = smoothness,
                duration = timing.duration,
                maximumStrength = maximumStrength,
                frequency = frequency,
                falloffCurve = ease.curve,
                recoverySpeed = recoverySpeed
            });
        }
    }

    [Serializable]
    public class UseRangeMultiplier : ToggleGroup
    {
        /// <summary>
        /// Range to calculate the minimum and maximum shake amount by distance
        /// </summary>
        public float range = 45;
        /// <summary>
        /// Maximum possible power that will be inflicted
        /// on the target when it is immediately beside the
        /// effect.
        /// </summary>
        public float maximumPower = 0.6f;
    }
}