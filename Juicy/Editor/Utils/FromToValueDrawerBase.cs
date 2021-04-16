using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    public class FromToValueDrawerBase<T> : JuicyPropertyDrawerBase
    {
        private SerializedProperty isFrom;
        private SerializedProperty value;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {
                
                CacheProperty(ref isFrom, property, nameof(isFrom));
                CacheProperty(ref value, property, nameof(value));

                float width = IsMinimalWidth ? 10 : 40;

                Rect valueRect = new Rect(position) {
                    width = position.width - width,
                    height = EditorGUIUtility.singleLineHeight
                };
                
                Rect buttonRect = new Rect(position) {
                    y = valueRect.y,
                    x = position.x + valueRect.width,
                    width = width,
                    height = EditorGUIUtility.singleLineHeight
                };
    
                DrawPropertyField(valueRect, property, value);
                
                if (GUI.Button(buttonRect, isFrom.boolValue ? "From" : "To")) {
                    isFrom.boolValue = !isFrom.boolValue;
                }
    
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        protected virtual void DrawPropertyField(Rect position,
            SerializedProperty property, SerializedProperty valueProperty)
        {
            EditorGUI.PropertyField(position, valueProperty,
                new GUIContent(property.displayName.Contains("Value") ? 
                    valueProperty.displayName : property.displayName));
        }
    }
    
    [CustomPropertyDrawer(typeof(BoolFromToValue))]
    public sealed class BoolFromToValueDrawer : FromToValueDrawerBase<bool> {}
    
    [CustomPropertyDrawer(typeof(FloatFromToValue))]
    public sealed class FloatFromToValueDrawer : FromToValueDrawerBase<float> {}

    [CustomPropertyDrawer(typeof(Vector4FromToValue))]
    public sealed class Vector4FromToValueDrawer : FromToValueDrawerBase<Vector4>
    {
        protected override void DrawPropertyField(Rect position, SerializedProperty property, SerializedProperty valueProperty)
        {
            valueProperty.vector4Value = EditorGUI
                .Vector4Field(position, valueProperty.displayName, valueProperty.vector4Value);
        }
    }
    
    [CustomPropertyDrawer(typeof(Vector3FromToValue))]
    public sealed class Vector3FromToValueDrawer : FromToValueDrawerBase<Vector3>
    {
        protected override void DrawPropertyField(Rect position, SerializedProperty property, SerializedProperty valueProperty)
        {
            valueProperty.vector3Value = EditorGUI
                .Vector3Field(position, valueProperty.displayName, valueProperty.vector3Value);
        }
    }
    
    [CustomPropertyDrawer(typeof(ColorFromToValue))]
    public sealed class ColorFromToValueDrawer : FromToValueDrawerBase<Color> {}

    [CustomPropertyDrawer(typeof(ColorChooserFromToValue))]
    public sealed class ColorChooserFromToValueDrawer : FromToValueDrawerBase<Color> {}

    [CustomPropertyDrawer(typeof(SliderFromToValue))]
    public sealed class SliderFromToValueDrawer : FromToValueDrawerBase<float>
    {
        private SerializedProperty range;

        protected override void DrawPropertyField(Rect position, 
            SerializedProperty property,
            SerializedProperty valueProperty)
        {
            CacheProperty(ref range, property, nameof(range));
            Vector3 rangeValue = range.vector3Value;
            
            EditorGUI.Slider(position,
                valueProperty, 
                rangeValue.y,
                rangeValue.z,
                new GUIContent(valueProperty.displayName));
        }
    }
}