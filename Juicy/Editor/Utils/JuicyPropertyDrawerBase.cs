using System.Collections.Generic;
using UnityEditor;
namespace TinyTools.Juicy
{
    public abstract class JuicyPropertyDrawerBase : PropertyDrawer
    {
        protected static readonly float StandardSpacing = EditorGUIUtility.standardVerticalSpacing;
        protected static readonly float SingleLineHeight = EditorGUIUtility.singleLineHeight;
        
        protected bool IsMinimalWidth => EditorGUIUtility.currentViewWidth < 333;

        protected void CacheProperty(ref SerializedProperty property, SerializedProperty parent, string name)
        {
            if (property == null) {
                property = parent.FindPropertyRelative(name);
            }
        }
        
        protected IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
        {
            SerializedProperty copy = property.Copy();
            
            copy.NextVisible(true);
            bool hasNext = true;
            
            while (hasNext) {
                if (SerializedProperty.EqualContents(copy, property.GetEndProperty())) {
                    yield break;
                }
 
                yield return copy;
 
                hasNext = copy.NextVisible(false);
            }
        }
    }
}