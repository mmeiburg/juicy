using UnityEngine;

namespace TinyTools.Juicy
{
    public sealed class JuicyCameraShaker : MonoBehaviour
    {
        private static JuicyCameraShaker instance;

        private Camera target = null;
        private Vector3 maximumStrength = Vector3.one;
        private float frequency = 25;
        private float fallOff = 0f;
        private float fallOffSmoothness = 1;
        private bool ignoreTimeScale = false;
        private float seed = 0f;
        private float elapsedTimePercentage = 0;
        private float recoveryRate = 0;
        private float recoverySpeed = 1f;
        private float stopTime;

        private Vector3 originalPosition;

        private AnimationCurve ease = AnimationCurve.Linear(0,0,1,1);
        private bool shaking = false;
        
        public static JuicyCameraShaker Instance(Camera camera)
        {
            if (instance != null) {
                return instance;
            }

            if (camera.TryGetComponent(out JuicyCameraShaker shaker)) {
                return instance;
            }

            instance = camera.gameObject.AddComponent<JuicyCameraShaker>();
            instance.target = camera;
            instance.originalPosition = camera.transform.position;

            return instance;
        }
        
        private void Update()
        {
            if (!shaking) {
                return;
            }
            
            float deltaTime = JuicyUtils.DeltaTime(ignoreTimeScale);
            float time = JuicyUtils.Time(ignoreTimeScale);

            if (time > stopTime) {
                fallOff = Mathf.Clamp01(fallOff - recoverySpeed * deltaTime);
            }
            
            if (fallOff > 1) {
                target.transform.localPosition = originalPosition;
                shaking = false;
                return;
            }
            
            float shake = Mathf.Pow(fallOff, fallOffSmoothness);
            float easeMultiplier = ease.Evaluate(elapsedTimePercentage);
            
            target.transform.localPosition = originalPosition + new Vector3(
                maximumStrength.x * (Mathf.PerlinNoise(seed, time * frequency) * 2 - 1),
                maximumStrength.y * (Mathf.PerlinNoise(seed + 1, time * frequency) * 2 - 1),
                maximumStrength.z * (Mathf.PerlinNoise(seed + 2, time * frequency) * 2 - 1)
            ) * shake * easeMultiplier;
            
            elapsedTimePercentage += recoveryRate * deltaTime;
        }
    
        public void Shake(ShakeProperties properties)
        {
            if (Mathf.Approximately(properties.duration, 0)) {
                return;
            }
            maximumStrength = properties.maximumStrength;
            frequency = properties.frequency;
            fallOffSmoothness = properties.smoothness;
            recoveryRate = 1 / properties.duration;
            stopTime = JuicyUtils.Time(properties.ignoreTimeScale) + properties.duration;
            
            ease = properties.falloffCurve;
            ignoreTimeScale = properties.ignoreTimeScale;
            recoverySpeed = Mathf.Max(0.1f, properties.recoverySpeed);

            fallOff = properties.power;
            seed = Random.value;

            shaking = true;
        }

        public struct ShakeProperties
        {
            public float power;
            
            public bool ignoreTimeScale;
            public float frequency;
            public Vector3 maximumStrength;
            public float smoothness;
            public float duration;
            public AnimationCurve falloffCurve;
            public float recoverySpeed;
        }
    }
}