using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.ShaderUtil.ShaderPropertyType;

namespace TinyTools.Juicy
{
    [CustomEditor(typeof(JuicyFeedbackShader))]
    public sealed class JuicyFeedbackShaderEditor : JuicyEditorBase
    {
        private readonly GUIContent valueLabelContent = new GUIContent("Value");

        private SerializedProperty timing;
        private SerializedProperty floatValue;
        private SerializedProperty vectorValue;
        private SerializedProperty colorValue;
        private SerializedProperty sliderValue;
        private SerializedProperty floatResetValue;
        private SerializedProperty vectorResetValue;
        private SerializedProperty colorResetValue;
        private SerializedProperty sliderResetValue;
        private SerializedProperty ease;
        private SerializedProperty selected;
        private SerializedProperty propertyName;
        private SerializedProperty propertyType;
        private SerializedProperty targetProperty;
        private SerializedProperty shaderKeyword;
        
        private readonly List<ShaderPropertyData> properties = 
            new List<ShaderPropertyData>();
        
        private Renderer renderer;
        private Shader shader;

        private struct ShaderPropertyData
        {
            public readonly string name;
            public readonly string type;
            public readonly string path;
            public Vector3 range;

            public ShaderPropertyData(string name, string type)
            {
                this.name = name;
                this.type = type;
                path = $"{type}/{this.name}";
                range = Vector3.zero;
            }
        }

        protected override void Initialize()
        {
            CacheProperty(ref timing, nameof(timing));
            CacheProperty(ref floatValue, nameof(floatValue));
            CacheProperty(ref vectorValue, nameof(vectorValue));
            CacheProperty(ref colorValue, nameof(colorValue));
            CacheProperty(ref sliderValue, nameof(sliderValue));
                
            CacheProperty(ref floatResetValue, nameof(floatResetValue));
            CacheProperty(ref vectorResetValue, nameof(vectorResetValue));
            CacheProperty(ref colorResetValue, nameof(colorResetValue));
            CacheProperty(ref sliderResetValue, nameof(sliderResetValue));
                
            CacheProperty(ref selected, nameof(selected));
            CacheProperty(ref propertyName, nameof(propertyName));
            CacheProperty(ref propertyType, nameof(propertyType));
            CacheProperty(ref targetProperty, "target");
            CacheProperty(ref shaderKeyword, nameof(shaderKeyword));
            CacheProperty(ref ease, nameof(ease));
            
            SetupShaderDropdown();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            timing.isExpanded =
                JuicyEditorUtils.FoldoutHeader(timing.isExpanded, "Timing", () =>
                {
                    EditorGUILayout.PropertyField(timing);
                });
            
            isExpanded.boolValue =
                JuicyEditorUtils.FoldoutHeader(isExpanded.boolValue, "Properties", DrawProperties);
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawProperties()
        {
            using (var check = new EditorGUI.ChangeCheckScope()) {
                EditorGUILayout.PropertyField(targetProperty);

                if (check.changed) {
                    SetupShaderDropdown();
                }
            }

            if (renderer == null) {
                return;
            }

            if (renderer != null && renderer.sharedMaterial == null) {
                EditorGUILayout.HelpBox("Material of Renderer Component is missing!",
                    MessageType.Error);
                return;
            }

            selected.intValue = EditorGUILayout.Popup("Property", selected.intValue,
                properties.Select(x => x.path).ToArray());

            if (selected.intValue < properties.Count) {
                propertyName.stringValue =
                    properties[selected.intValue].name;
                propertyType.stringValue =
                    properties[selected.intValue].type;
            }
            
            string type = propertyType.stringValue;
            
            if (type.Equals(JuicyFeedbackShader.Float)) {
                EditorGUILayout.PropertyField(floatValue, valueLabelContent);
                EditorGUILayout.PropertyField(floatResetValue);

            } else if (type.Equals(JuicyFeedbackShader.Vector)) {
                EditorGUILayout.PropertyField(vectorValue, valueLabelContent);
                EditorGUILayout.PropertyField(vectorResetValue);
                
            } else if (type.Equals(JuicyFeedbackShader.Color)) {
                EditorGUILayout.PropertyField(colorValue, valueLabelContent);
                EditorGUILayout.PropertyField(colorResetValue);
                EditorGUILayout.PropertyField(shaderKeyword);

            } else if (type.Equals(JuicyFeedbackShader.Range)) {

                Vector3 range = properties[selected.intValue].range;

                sliderValue.FindPropertyRelative("range").vector3Value = range;
                EditorGUILayout.PropertyField(sliderValue, valueLabelContent);
                EditorGUILayout.PropertyField(sliderResetValue);
            }
            
            EditorGUILayout.PropertyField(ease);
        }

        private void SetupShaderDropdown()
        {
            properties.Clear();

            bool isSelf = targetProperty.FindPropertyRelative("targetIsSelf").boolValue;

            if (isSelf) {
                renderer = targetProperty.FindPropertyRelative("selfTarget").objectReferenceValue as Renderer;
            } else {
                renderer = targetProperty.FindPropertyRelative("otherTarget").objectReferenceValue as Renderer;
            }

            if (renderer == null) {
                return;
            }

            if (renderer.sharedMaterial == null) {
                return;
            }

            shader = renderer.sharedMaterial.shader;
            int propertyCount = ShaderUtil.GetPropertyCount(shader);

            for (int i = 0; i < propertyCount; i++) {

                if (ShaderUtil.IsShaderPropertyHidden(shader, i)) {
                    continue;
                }

                ShaderUtil.ShaderPropertyType shaderPropertyType =
                    ShaderUtil.GetPropertyType(shader, i);

                if (shaderPropertyType == TexEnv) {
                    continue;
                }

                ShaderPropertyData data =
                    new ShaderPropertyData(
                        ShaderUtil.GetPropertyName(shader, i),
                        shaderPropertyType.ToString());

                if (shaderPropertyType == Range) {
                    data.range = GetLimits(shader, i);
                }

                properties.Add(data);
            }

            if (selected.intValue >= properties.Count) {
                selected.intValue = 0;
            }
        }
        
        private Vector3 GetLimits(Shader shader, int index)
        {
            return new Vector3(
                ShaderUtil.GetRangeLimits(shader, index, 0),
                ShaderUtil.GetRangeLimits(shader, index, 1),
                ShaderUtil.GetRangeLimits(shader, index, 2));
        }
    }
}