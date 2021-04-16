using TinyTools.PoolAttendant;
using UnityEngine;
using UnityEngine.Audio;

namespace TinyTools.Juicy
{
    [Feedback("Audio/Pooled")][AddComponentMenu("")]
    public class JuicyFeedbackAudioPooled : JuicyFeedbackAudioBase
    {
        [SerializeField] private AudioInstance prefab = null;
        [SerializeField] private AudioMixerGroup group = null;

        private AudioInstance instance;
        
        protected override void PlayClip(Vector3 position, AudioClip clip, float volume, float pitch)
        {
            if (prefab == null) {
                return;
            }
            
            instance = prefab.GetPooledInstance();
            instance.Play(position, clip, group, volume, pitch);
        }
    }
}