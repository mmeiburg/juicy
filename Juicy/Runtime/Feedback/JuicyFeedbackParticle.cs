using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Particle/Simple")][AddComponentMenu("")]
    public class JuicyFeedbackParticle : JuicyFeedbackObjectCreator<ParticleSystem>
    {
        internal override void Stop()
        {
            base.Stop();
            
            if (objectReference == null) {
                return;
            }
            
            objectReference.Stop();
        }
    }
}