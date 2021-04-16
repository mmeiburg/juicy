using UnityEngine;
using UnityEngine.Audio;

namespace TinyTools.Juicy
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioInstance : MonoBehaviour
    {
        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            source.Stop();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void Play(AudioClip clip, float volume)
        {
            Play(clip, null, volume);
        }

        public void Play(AudioClip clip, AudioMixerGroup group = null, float volume = 1f, float pitch = 1f)
        {
            Play(Vector3.zero, clip, group, volume, pitch);
        }
        
        public void Play(AudioClip clip, float volume, float pitch)
        {
            Play(Vector3.zero, clip, volume, pitch);
        }

        public void Play(Vector3 position, AudioClip clip, float volume, float pitch = 1f)
        {
            Play(position, clip, null, volume, pitch);
        }

        public void Play(Vector3 position, AudioClip clip, AudioMixerGroup group, float volume, float pitch)
        {
            transform.position = position;
            source.clip = clip;
            source.outputAudioMixerGroup = group;
            source.volume = volume;
            source.pitch = pitch;
            
            source.Play();

            this.InvokeDelayed(clip.length, () => gameObject.SetActive(false));
        }
    }
}