using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using UnityEngine;

namespace TinyTools.Juicy
{
    public static class DOTweenShortcutExtentions
    {
        public static Tweener DOBlendableIntensity(this Light target, float value, float duration)
        {
            float to = 0;
            
            return DOTween.To(() => to, x =>
                {
                    float alpha = x - to;
                    to = x;

                    target.intensity += alpha;

                }, value, duration)
                
                .Blendable()
                .SetTarget(target);
        }
        
        public static Tweener DOBlendableFieldOfView(this Camera target, float value, float duration)
        {
            float to = 0;
            
            return DOTween.To(() => to, x =>
                {
                    float alpha = x - to;
                    to = x;

                    target.fieldOfView += alpha;

                }, value, duration)
                
                .Blendable()
                .SetTarget(target);
        }
        
        public static Tweener DOBlendablePunchScaleBy(this Transform target, Vector3 punch, float duration,
            int vibrato = 10,
            float elasticity = 1f)
        {
            if (duration <= 0.0) {
                if (Debugger.logPriority > 0) {
                    Debug.LogWarning("DOBlendablePunchScaleBy: duration can't be 0, returning NULL without creating a tween");
                }
                return null;
            }
            
            Vector3 to = Vector3.zero;
            
            return DG.Tweening.DOTween.Punch(() => to, v =>
            {
                Vector3 vector3 = v - to;
                to = v;
                target.localScale += vector3;
                
            }, punch, duration, vibrato, elasticity).Blendable().SetTarget(target);
        }
        
        /// <summary>Punches a Transform's localPosition towards the given direction and then back to the starting one
        /// as if it was connected to the starting position via an elastic.</summary>
        /// <param name="punch">The direction and strength of the punch (added to the Transform's current position)</param>
        /// <param name="duration">The duration of the tween</param>
        /// <param name="vibrato">Indicates how much will the punch vibrate</param>
        /// <param name="elasticity">Represents how much (0 to 1) the vector will go beyond the starting position when bouncing backwards.
        /// 1 creates a full oscillation between the punch direction and the opposite direction,
        /// while 0 oscillates only between the punch and the start position</param>
        /// <param name="snapping">If TRUE the tween will smoothly snap all values to integers</param>
        public static Tweener DOBlendablePunchPosition(
            this Transform target,
            Vector3 punch,
            float duration,
            int vibrato = 10,
            float elasticity = 1f,
            bool snapping = false)
        {
            if (duration <= 0.0) {
                if (Debugger.logPriority > 0) {
                    Debug.LogWarning("DOBlendablePunchPosition: duration can't be 0, returning NULL without creating a tween");
                }

                return null;
            }

            Vector3 to = Vector3.zero;
            
            return DG.Tweening.DOTween.Punch(() => to, v =>
            {
                Vector3 vector3 = v - to;
                to = v;
                target.localPosition += vector3;
            
            }, punch, duration, vibrato, elasticity)
                .Blendable()
                .SetTarget(target)
                .SetOptions(snapping);
        }
        
        public static Tweener DOBlendableShakePosition(
            this Transform target,
            float duration,
            Vector3 strength,
            int vibrato = 10,
            float randomness = 90f,
            bool snapping = false,
            bool fadeOut = false)
        {
            if (duration <= 0.0) {
                if (Debugger.logPriority > 0) {
                    Debug.LogWarning("DOBlendablePunchPosition: duration can't be 0, returning NULL without creating a tween");
                }

                return null;
            }

            Vector3 to = Vector3.zero;

            return DG.Tweening.DOTween.Shake(() => to, v =>
                {
                    Vector3 vector3 = v - to;
                    to = v;
                    target.position += vector3;

                }, duration, strength, vibrato, randomness, fadeOut)
                .Blendable()
                .SetTarget(target)
                .SetSpecialStartupMode(SpecialStartupMode.SetShake)
                .SetOptions(snapping);
        }
        
        public static Tweener DOBlendableShakeRotation(
            this Transform target,
            float duration,
            Vector3 strength,
            int vibrato = 10,
            float randomness = 90f,
            bool fadeOut = true)
        {
            if (duration <= 0.0) {
                if (Debugger.logPriority > 0)
                    Debug.LogWarning("DOBlendablePunchRotation: duration can't be 0, returning NULL without creating a tween");
                return null;
            }

            Vector3 to = Vector3.zero;

            return DG.Tweening.DOTween.Shake(() => to, v =>
                {
                    //QTransition = QFinal * QInitial^{-1}
                    
                    Quaternion rotation = Quaternion.Euler(to.x, to.y, to.z);
                    Quaternion quaternion = Quaternion.Euler(v.x, v.y, v.z) * Quaternion.Inverse(rotation);
                    to = v;
                    target.rotation = target.rotation * quaternion * target.rotation;
                    
                    //Quaternion rotation = Quaternion.Euler(to).Sub(target.rotation);
                    //to = rotation.eulerAngles;
                    //target.rotation = target.rotation.Add(rotation);

                }, duration, strength, vibrato, randomness, fadeOut)
                .Blendable()
                .SetTarget(target)
                .SetSpecialStartupMode(SpecialStartupMode.SetShake);
        }

        public static Tweener DOBlendableVector4(this Material target, string property, Vector4 vector4, float duration)
        {
            if(!target.HasProperty(property)) {
                if (Debugger.logPriority > 0) {
                    Debugger.LogMissingMaterialProperty(property);
                }
                return null;
            }
            
            Vector4 to = Vector4.zero;
            
            return DOTween.To(() => to, x =>
                {
                    Vector4 v4 = x - to;
                    to = x;

                    Vector4 t = target.GetVector(property) + v4;
                    
                    target.SetVector(property, t);

                }, vector4, duration)
                
                .Blendable()
                .SetTarget(target);
        }

        public static Tweener DOBlendableFade(this Material target, float value, float duration)
        {
            float to = 0;
            
            return DOTween.To(() => to, x =>
                {
                    float alpha = x - to;
                    to = x;

                    Color c = target.color;
                        
                    c.a += alpha;
                    target.color = c;

                }, value, duration)
                
                .Blendable()
                .SetTarget(target);
        }
        
        public static Tweener DOBlendableFade(this SpriteRenderer target, float value, float duration)
        {
            float to = 0;
            
            return DOTween.To(() => to, x =>
                {
                    float alpha = x - to;
                    to = x;
                    
                    Color c = target.color;
                        
                    c.a += alpha;
                    target.color = c;

                }, value, duration)
                
                .Blendable()
                .SetTarget(target);
        }
    }
}