using DG.Tweening;
using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackBase : MonoBehaviour
    {
        [HideInInspector] public string label;
        [HideInInspector] public bool isActive = true;
        [HideInInspector] public bool isExpanded = true;
        [HideInInspector] public int referenceCount;

        protected Tweener tween;

        internal void PlayFeedback()
        {
            if (!isActive) {
                return;
            }

            if (IsTweenPaused()) {
                Resume();
            } else {
                KillTweenIfInfinityLoop();
                Play();
            }

            tween?.SetAutoKill(true);
        }

        protected virtual void Play() {}

        internal virtual void Pause()
        {
            tween?.Pause();
        }

        internal virtual void Stop()
        {
            StopAllCoroutines();
            
            if (tween == null) {
                return;
            }

            tween.Pause();
            tween.Rewind();
            tween.Kill();
        }

        private void OnDestroy()
        {
            Stop();
        }

        private void Resume()
        {
            tween?.Play();
        }

        private bool IsTweenPaused()
        {
            return tween != null && tween.IsActive() && !tween.IsPlaying();
        }

        private void KillTweenIfInfinityLoop()
        {
            // Kill the last tween if loop was infinity
            if (tween == null || !tween.IsActive() || tween.Loops() != -1) {
                return;
            }

            tween.Rewind();
            tween.Kill();
        }
    }
}