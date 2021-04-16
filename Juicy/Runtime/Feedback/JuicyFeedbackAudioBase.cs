using System;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackAudioBase : JuicyFeedbackBase
    {
        [SerializeField, Timing(TimingAttribute.TimingStyle.HideDuration)] protected Timing timing = new Timing();
        [SerializeField] protected AudioClipList clips = new AudioClipList();
        [SerializeField, MinMaxRange(0, 1)] protected Vector2 volume = Vector2.one;
        [SerializeField, MinMaxRange(-3, 3)] protected Vector2 pitch = Vector2.one;
        [SerializeField, Range(0, 1)] protected float probability = 1f;
        [SerializeField] protected CustomPosition useCustomPosition = new CustomPosition();
        
        protected override void Play()
        {
            if (Random.value >= probability) {
                return;
            }

            if (clips.IsEmpty) {
                return;
            }
            
            timing.Invoke(this, PerformDelayed);
        }

        protected virtual void PlayClip(Vector3 position, AudioClip clip, float volume, float pitch)
        {
        }

        private void PerformDelayed()
        {
            AudioClip currentClip = clips.Random;

            if (currentClip == null) {
                return;
            }

            float v = Random.Range(volume.x, volume.y);
            float p = Random.Range(pitch.x, pitch.y);
            
            PlayClip(transform.position, currentClip, v, p);
        }
    }

    [Serializable]
    public class UseAudioSource : ToggleGroup
    {
        public AudioMixerGroup group = null;
        public AudioSourceTarget source;
    }
    
    [Serializable]
    public class AudioClipList
    {
        public AudioClip[] clips;

        public bool IsEmpty => Length == 0;
        public int Length => clips?.Length ?? 0;

        public AudioClip this[int index] => clips[index];

        public AudioClip Random => clips?.Length > 0 ? clips[UnityEngine.Random.Range(0, clips.Length)] : null;
        public AudioClip First => clips?.Length > 0 ? clips[0] : null;

    }
}