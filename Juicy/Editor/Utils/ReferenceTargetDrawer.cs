using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(ReferenceTarget<,>), true)]
    public sealed class ReferenceTargetDrawer : JuicyPropertyDrawerBase
    {
        private SerializedProperty type;
        private SerializedProperty name;
        private SerializedProperty target;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheProperty(ref type, property, nameof(type));
            CacheProperty(ref name, property, nameof(name));
            CacheProperty(ref target, property, nameof(target));

            float height = EditorGUI.GetPropertyHeight(type);

            if (type.enumValueIndex == 0) {
                height += EditorGUI.GetPropertyHeight(name);
            } else {
                height += EditorGUI.GetPropertyHeight(target);
            }

            return height + StandardSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {
                Rect typeRect = new Rect(position) {
                    height = EditorGUI.GetPropertyHeight(type)
                };

                EditorGUI.PropertyField(typeRect, type, new GUIContent("Target", "Target of the effect"));

                EditorGUI.indentLevel++;
            
                if (type.enumValueIndex == 0) {
                    EditorGUI.PropertyField(GetPropertyRect(position, name), name);
                } else {
                    EditorGUI.PropertyField(GetPropertyRect(position, target), target);
                }

                EditorGUI.indentLevel--;
            }
        }

        private Rect GetPropertyRect(Rect position, SerializedProperty property)
        {
            return new Rect(position) {
                y = position.y + EditorGUI.GetPropertyHeight(type) + StandardSpacing,
                height = EditorGUI.GetPropertyHeight(property),
                width = position.width - EditorGUI.indentLevel * 4
            };
        }
    }
}