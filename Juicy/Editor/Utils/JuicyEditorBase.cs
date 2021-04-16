using System;
using System.Collections.Generic;
using UnityEditor;

namespace TinyTools.Juicy
{
    public abstract class JuicyEditorBase : Editor
    {
        protected SerializedProperty isExpanded;
        
        private readonly Dictionary<string, SerializedProperty> cachedProperties =
            new Dictionary<string, SerializedProperty>();

        private void OnEnable()
        {
            try
            {
                CacheProperty(ref isExpanded, nameof(isExpanded));
                Initialize();
            }
            catch (Exception) {
                // ignored
            }
        }

        private void OnDisable()
        {
            cachedProperties.Clear();
        }

        private void Reset()
        {
            cachedProperties.Clear();
        }

        protected virtual void Initialize() {}

        protected void CacheProperty(ref SerializedProperty property, string name)
        {
            if (property == null) {
                property = serializedObject.FindProperty(name);
            }
        }
        
        protected void CacheProperty(ref SerializedProperty property, SerializedProperty parent, string name)
        {
            if (property == null && parent != null) {
                property = parent.FindPropertyRelative(name);
            }
        }
        
        protected SerializedProperty CacheProperty(string name)
        {
            if (cachedProperties.TryGetValue(name, out var cachedProperty)) {
                return cachedProperty;
            }

            SerializedProperty property = serializedObject.FindProperty(name);
            cachedProperties.Add(name, property);

            return property;
        }

        protected IEnumerable<SerializedProperty> GetChildren(SerializedObject parent)
        {
            SerializedProperty property = parent.GetIterator();
            
            property = property.Copy();
 
            property.NextVisible(true);
            bool hasNext = true;
            
            while (hasNext)
            {
                if (SerializedProperty.EqualContents(property, property.GetEndProperty()))
                {
                    yield break;
                }
 
                yield return property;
                               
 
                hasNext = property.NextVisible(false);
            }
        }
        
        protected IEnumerable<SerializedProperty> GetChildren()
        {
            return GetChildren(serializedObject);
        }
    }
}