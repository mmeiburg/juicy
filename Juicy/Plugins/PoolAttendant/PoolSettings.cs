using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinyTools.PoolAttendant
{
    public class PoolSettings : ScriptableObject
    {
        public DefaultPoolItemList list = new DefaultPoolItemList();
    }

    [Serializable]
    public class DefaultPoolItemList
    {
        public List<DefaultPoolItem> items = new List<DefaultPoolItem>();
    }
    
    [Serializable]
    public class DefaultPoolItem
    {
        public GameObject prefab;
        public int size = 5;
    }
}