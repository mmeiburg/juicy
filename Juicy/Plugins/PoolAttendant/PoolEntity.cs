using UnityEngine;

namespace TinyTools.PoolAttendant
{
    public class PoolEntity : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefab = null;
        
        public GameObject Prefab {
            get => prefab;
            set => prefab = value;
        }
        
        private void OnEnable()
        {
            if (prefab == null) {
                return;
            }
            
            Pool.Instance.Reparent(gameObject, prefab.GetInstanceID());
        }

        private void OnDestroy()
        {
            if (prefab == null) {
                return;
            }
            
            Pool.Instance.Remove(gameObject, prefab.GetInstanceID());
        }
    }
}