using TinyTools.PoolAttendant;
using UnityEngine;

namespace TinyTools.Juicy
{
    [Feedback("Particle/Pooled")][AddComponentMenu("")]
    public class JuicyFeedbackParticlePooled : JuicyFeedbackParticle
    {
        protected override ParticleSystem Instantiate()
        {
            return prefab.GetPooledInstance();
        }
    }
}