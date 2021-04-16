using System.Collections.Generic;
using UnityEngine;

namespace TinyTools.Juicy
{
    public class JuicyFeedbackMasterList : MonoBehaviour
    {
        public List<JuicyFeedbackList> feedbackList;

        public void Play(JuicyFeedbackList list)
        {
            list.Play();
        }

        public void PlayAll()
        {
            foreach (var juicyFeedbackList in feedbackList) {
                Play(juicyFeedbackList);
            }
        }
    }
}