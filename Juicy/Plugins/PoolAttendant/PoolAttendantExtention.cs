using UnityEngine;

namespace TinyTools.PoolAttendant
{
    public static class PoolAttendantExtentions
    {
        public static T GetPooledInstance<T>(this T mono) where T : Component
        {
            return mono.gameObject.GetPooledInstance<T>(Vector3.zero);
        }
        
        public static T GetPooledInstance<T>(this T mono, Vector3 position) where T : Component
        {
            return mono.gameObject.GetPooledInstance<T>(position, mono.transform.rotation);
        }
        
        public static T GetPooledInstance<T>(this T mono, Vector3 position, Quaternion rotation) where T : Component
        {
            return mono.gameObject.GetPooledInstance<T>(position, rotation, mono.transform.localScale);
        }
        
        public static T GetPooledInstance<T>(this T mono, Vector3 position, Quaternion rotation, Vector3 scale) where T : Component
        {
            return Pool.Instance.Get<T>(mono.gameObject, position, rotation, scale);
        }        
        
        public static GameObject GetPooledInstance(this GameObject prefab)
        {
            return prefab.GetPooledInstance(Vector3.zero);
        }
        
        public static GameObject GetPooledInstance(this GameObject prefab, Vector3 position)
        {
            return prefab.GetPooledInstance(position, prefab.transform.rotation);
        }
        
        public static GameObject GetPooledInstance(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return prefab.GetPooledInstance(position, rotation, prefab.transform.localScale);
        }
        
        public static GameObject GetPooledInstance(this GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            return Pool.Instance.Get(prefab, position, rotation, scale);
        }
        
        public static T GetPooledInstance<T>(this GameObject prefab) where T : Component
        {
            return prefab.GetPooledInstance<T>(Vector3.zero);
        }
        
        public static T GetPooledInstance<T>(this GameObject prefab, Vector3 position) where T : Component
        {
            return prefab.GetPooledInstance<T>(position, prefab.transform.rotation);
        }
        
        public static T GetPooledInstance<T>(this GameObject prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return prefab.GetPooledInstance<T>(position, rotation, prefab.transform.localScale);
        }
        
        public static T GetPooledInstance<T>(this GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale) where T : Component
        {
            return Pool.Instance.Get<T>(prefab, position, rotation, scale);
        }
    }
}