using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TinyTools.PoolAttendant
{
    [CustomPropertyDrawer(typeof(DefaultPoolItem))]
    public class DefaultPoolItemDrawer : PropertyDrawer
    {
        private SerializedProperty prefab;
        private SerializedProperty size;

        private const int SizeSize = 30;
        private const int Spacing = 4;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {
                
                prefab = property.FindPropertyRelative("prefab");
                size = property.FindPropertyRelative("size");
                
                var textDimensions = GUIStyle.none.CalcSize(new GUIContent("text"));
                
                EditorGUI.PropertyField(
                    new Rect(position.x, position.y, SizeSize, position.height), size, GUIContent.none);

                prefab.objectReferenceValue = EditorGUI.ObjectField(
                    new Rect(position.x + SizeSize + Spacing, position.y, position.width - SizeSize - Spacing, position.height), GUIContent.none, 
                    prefab.objectReferenceValue, typeof(GameObject), false);
            }
        }
    }
}