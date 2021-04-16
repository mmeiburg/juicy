using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class JuicyFeedbackCameraBase : JuicyFeedbackBase
    {
        [SerializeField] protected new CameraReferenceTarget camera = new CameraReferenceTarget();
    }
}