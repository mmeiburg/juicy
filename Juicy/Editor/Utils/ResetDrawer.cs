using System;
using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(Reset<>), true)]
    public sealed class ResetDrawer : JuicyPropertyDrawerBase
    {
        private SerializedProperty resetType;
        private SerializedProperty loop;
        private SerializedProperty resetValue;

        private ResetType type;
        private bool ShowLoop => type == ResetType.ToValue || type == ResetType.Yoyo;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheProperty(ref resetType, property, nameof(resetType));
            CacheProperty(ref loop, property, nameof(loop));
            CacheProperty(ref resetValue, property, nameof(resetValue));

            type = 
                (ResetType) Enum.GetValues(typeof(ResetType))
                    .GetValue(resetType?.enumValueIndex ?? 0);

            float height = ShowLoop
                ? EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing
                : 0;
            
            height += type == ResetType.ToValue ? EditorGUIUtility.standardVerticalSpacing +
                                                       EditorGUIUtility.singleLineHeight : 0;

            return EditorGUIUtility.singleLineHeight + height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {
                Rect resetRect = new Rect(position) {
                    height = EditorGUIUtility.singleLineHeight
                };
            
                Rect loopRect = new Rect(position) {
                    y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    height = EditorGUIUtility.singleLineHeight
                };
            
                Rect valueRect = new Rect(position) {
                    y = position.y + EditorGUIUtility.singleLineHeight * 2 +
                        EditorGUIUtility.standardVerticalSpacing * 2,
                    height = EditorGUIUtility.singleLineHeight
                };

                EditorGUI.PropertyField(resetRect, resetType);

                if (ShowLoop) {
                    EditorGUI.PropertyField(loopRect, loop);
                }

                if (type == ResetType.ToValue) {
                    EditorGUI.PropertyField(valueRect, resetValue);
                }
            }
        }
    }
}