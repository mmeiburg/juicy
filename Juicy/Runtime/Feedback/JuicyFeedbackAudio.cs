using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Audio/Simple")][AddComponentMenu("")]
    public class JuicyFeedbackAudio : JuicyFeedbackAudioBase
    {
        [SerializeField] private UseAudioSource useAudioSource = new UseAudioSource();

        protected override void PlayClip(Vector3 position, AudioClip clip, float volume, float pitch)
        {
            if (!useAudioSource.isActive) {
                if (useCustomPosition.isActive) {
                    if (useCustomPosition.spawnAt != null) {
                        position = useCustomPosition.spawnAt.position;
                    }
                    
                    position += useCustomPosition.offset;
                }

                AudioSource.PlayClipAtPoint(clip,
                    position, volume);
               
            } else {
                useAudioSource.source.Value.clip = clip;
                if (useAudioSource.group != null) {
                    useAudioSource.source.Value.outputAudioMixerGroup = useAudioSource.group;
                }

                useAudioSource.source.Value.volume = volume;
                useAudioSource.source.Value.pitch = pitch;
                useAudioSource.source.Value.Play();
            }
        }
    }
}