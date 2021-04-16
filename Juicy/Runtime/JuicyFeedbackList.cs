using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace TinyTools.Juicy
{
    [ExecuteInEditMode]
    public sealed class JuicyFeedbackList : MonoBehaviour
    {
        public List<JuicyFeedbackBase> feedbackList = new List<JuicyFeedbackBase>();
        
        /// <summary>
        /// Name which can be modified in the editor to name individual feedback elements
        /// </summary>
        public string displayName = "";
        
        /// <summary>
        /// If play on enable is true, the feedback list will play all feedback elements on enable
        /// </summary>
        public bool playOnEnable = false;
        /// <summary>
        /// If enabled all hidden components are visible
        /// </summary>
        public bool debugView;

        private void OnEnable()
        {
            if (playOnEnable) {
                Play();
            }
        }

        [ContextMenu("Jausdnjaksndjkasd")]
        public void Debug()
        {
            for(var i = feedbackList.Count - 1; i > -1; i--)
            {
                if (feedbackList[i] == null)
                    feedbackList.RemoveAt(i);
            }
        }

        /// <summary>
        /// If active, loop through the feedback list and start playing every feedback
        /// </summary>
        public void Play()
        {
            if (!enabled) return;
            
            foreach (JuicyFeedbackBase feedback in feedbackList) 
            {
                feedback.PlayFeedback();
            }
        }

        /// <summary>
        /// If active, loop through the feedback list and stops every feedback
        /// </summary>
        public void Stop()
        {
            if (!enabled) return;
            
            foreach (JuicyFeedbackBase feedback in feedbackList) {
                feedback.Stop();
            }
        }
        
        /// <summary>
        /// If active, loop through the feedback list and pause every feedback
        /// </summary>
        public void Pause()
        {
            if (!enabled) return;
            
            foreach (JuicyFeedbackBase feedback in feedbackList) {
                feedback.Pause();
            }
        }

        /// <summary>
        /// Removes a feedback from the list of feedbacks
        /// </summary>
        /// <param name="feedback"></param>
        public void Remove(JuicyFeedbackBase feedback)
        {
            feedbackList.Remove(feedback);
        }

#if UNITY_EDITOR
        private void OnDestroy()
        {
            if (Application.isPlaying) {
                return;
            }

            EditorApplication.delayCall += Destroy;
        }

        private void Destroy()
        {
            foreach (JuicyFeedbackBase feedback in feedbackList) {
                DestroyImmediate(feedback);    
            }
            
            feedbackList.Clear();
        }
#endif
    }
}