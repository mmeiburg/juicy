using System;
using DG.Tweening;

namespace TinyTools.Juicy
{
    [Serializable]
    public sealed class JuicyFeedback
    {
        public JuicyFeedbackList feedbackList;

        public void Play()
        {
            if (feedbackList != null) {
                feedbackList.Play();
            }
        }

        public void Stop()
        {
            if (feedbackList != null) {
                feedbackList.Stop();
            }
        }

        public void Pause()
        {
            if (feedbackList != null) {
                feedbackList.Pause();
            }
        }
    }
}