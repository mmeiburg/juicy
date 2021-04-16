using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomEditor(typeof(JuicyFeedbackAnimator))]
    public sealed class JuicyFeedbackAnimatorEditor : JuicyEditorBase
    {
        private Animator animator;
        private readonly GUIContent valueLabelContent = new GUIContent("Value");
        
        private IEnumerable<AnimatorControllerParameter> AnimatorControllerParameters => 
            animator == null ? null : animator.parameters;
        
        private SerializedProperty timing;
        private SerializedProperty targetProperty;
        private SerializedProperty selfTarget;
        private SerializedProperty otherTarget;
        private SerializedProperty targetIsSelf;
        private SerializedProperty type;
        private SerializedProperty selectedIndex;
        private SerializedProperty hash;
        private SerializedProperty boolValue;
        private SerializedProperty intValue;
        private SerializedProperty floatValue;
        private SerializedProperty ease;

        private ParameterData[] parameters;
        
        protected override void Initialize()
        {
            CacheProperty(ref timing, nameof(timing));
            CacheProperty(ref targetProperty, "target");
            
            CacheProperty(ref targetIsSelf, targetProperty, nameof(targetIsSelf));
            CacheProperty(ref selfTarget, targetProperty, nameof(selfTarget));
            CacheProperty(ref otherTarget, targetProperty, nameof(otherTarget));
            
            CacheProperty(ref type, nameof(type));
            CacheProperty(ref selectedIndex, nameof(selectedIndex));
            CacheProperty(ref hash, nameof(hash));
            
            CacheProperty(ref boolValue, nameof(boolValue));
            CacheProperty(ref intValue, nameof(intValue));
            CacheProperty(ref floatValue, nameof(floatValue));
            
            CacheProperty(ref ease, nameof(ease));
            
            SetupAnimator();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            SetupAnimator();
            
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
                    SetupAnimator();
                }
            }

            bool isValid = false;
            
            if (targetIsSelf.boolValue) {
                isValid = (serializedObject.targetObject as Component)?
                    .GetComponent<Animator>() != null;

            } else {
                isValid = otherTarget.objectReferenceValue != null;
            }

            if (!isValid) {
                return;
            }

            if (animator != null && animator.runtimeAnimatorController == null) {
                EditorGUILayout.HelpBox("Controller of Animator Component is missing!",
                    MessageType.Error);
                return;
            }

            if (parameters == null || parameters.Length == 0) {
                EditorGUILayout.HelpBox("No parameters found",
                    MessageType.Info);
                return;
            }

            ParameterData data;
            
            using (var check = new EditorGUI.ChangeCheckScope()) {
                selectedIndex.intValue = 
                    EditorGUILayout.Popup("Parameter", selectedIndex.intValue,
                        parameters.Select(x => x.MenuItem).ToArray());

                selectedIndex.serializedObject.ApplyModifiedProperties();

                data = parameters[selectedIndex.intValue];
                
                hash.intValue = data.hash;
            
                if (check.changed) {
                    type.enumValueIndex = Array.IndexOf(Enum.GetValues(
                        typeof(AnimatorControllerParameterType)),
                        data.type);
                }
            }

            switch (data.type) {
                case AnimatorControllerParameterType.Float:
                    EditorGUILayout.PropertyField(floatValue, valueLabelContent);
                    break;
                case AnimatorControllerParameterType.Int:
                    EditorGUILayout.PropertyField(intValue, valueLabelContent);
                    break;
                case AnimatorControllerParameterType.Bool:
                    boolValue.boolValue = EditorGUILayout.Toggle(valueLabelContent, boolValue.boolValue);
                    break;
            }

            EditorGUILayout.PropertyField(ease);
        }

        private void SetupAnimator()
        {
            animator = (Animator) (targetIsSelf.boolValue 
                ? selfTarget.objectReferenceValue 
                : otherTarget.objectReferenceValue);

            if (animator == null) {
                return;
            }
            
            if (animator.runtimeAnimatorController == null) {
                return;
            }

            animator.keepAnimatorControllerStateOnDisable = true;
            
            // Workaround to prevent unity to throw a warning
            animator.enabled = false;
            animator.enabled = true;

            parameters =
                AnimatorControllerParameters.Select(x =>
                new ParameterData {
                        type = x.type,
                        name = x.name,
                        hash = x.nameHash
                        
                    }
                ).ToArray();

            if (selectedIndex.intValue > parameters.Length  || parameters.Length == 0) {
                selectedIndex.intValue = 0;
            }
        }

        private struct ParameterData
        {
            public AnimatorControllerParameterType type;
            public string name;
            public int hash;

            public string MenuItem => $"{type}/{name}";
        }
    }
}