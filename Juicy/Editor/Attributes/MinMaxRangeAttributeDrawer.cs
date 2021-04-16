using System;
using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(MinMaxRange))]
    public sealed class MinMaxRangeAttributeDrawer : JuicyPropertyDrawerBase
    {
        private MinMaxRange MinMaxAttribute => (MinMaxRange)attribute;
        
        private const float FieldWidth = 80;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {

                float min = property.vector2Value.x;
                float max = property.vector2Value.y;

                if (!IsMinimalWidth) {

                    Rect labelRect = new Rect(position) {
                        x = position.x,
                        width = EditorGUIUtility.labelWidth
                    };
                    
                    float width = EditorGUI.indentLevel > 1 ? EditorGUI.indentLevel * 16 - 5 : 16;
                    
                    Rect minRect = new Rect(position) {
                        x = position.x + EditorGUIUtility.labelWidth - width,
                        width = FieldWidth,
                    };
                        
                    Rect maxRect = new Rect(position) {
                        x = position.width - FieldWidth + width + 5,
                        width = FieldWidth
                    };

                    float sliderOffset = EditorGUI.indentLevel > 1 ? 20 : 8;
                    
                    Rect sliderRect = new Rect(position) {
                        x = minRect.x + minRect.width - sliderOffset,
                        xMax = maxRect.x + sliderOffset
                    };

                    EditorGUI.LabelField(labelRect, label);

                    min = (float) Math.Round(EditorGUI.FloatField(minRect, min), 3);
                    EditorGUI.MinMaxSlider(sliderRect, ref min,
                    ref max, MinMaxAttribute.min, MinMaxAttribute.max);
                    max = (float) Math.Round(EditorGUI.FloatField(maxRect, max), 3);
                    
                } else {
                    EditorGUI.MinMaxSlider(position, label, ref min, ref max,
                        MinMaxAttribute.min, MinMaxAttribute.max);
                }

                property.vector2Value = new Vector2(Mathf.Max(min, MinMaxAttribute.min),
                    Mathf.Min(max, MinMaxAttribute.max));
            }
        }
    }
}