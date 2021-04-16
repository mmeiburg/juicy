using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomEditor(typeof(JuicyFeedbackBase), true)]
    public class JuicyFeedbackBaseEditor : JuicyEditorBase
    {
        private const string Script = "m_Script";

        private SerializedProperty timing;
        private SerializedProperty ease;
        
        private int childrenCount;

        protected override void Initialize()
        {
            CacheProperty(ref timing, nameof(timing));
            CacheProperty(ref ease, nameof(ease));

            childrenCount = GetChildren().ToList().Count;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            if (timing != null) {
                timing.isExpanded = JuicyEditorUtils.FoldoutHeader(timing.isExpanded, "Timing", () =>
                {
                    EditorGUILayout.PropertyField(timing);
                });  
            }
            
            if (ContainsOnlyScriptAndTimingProperty()) {
                
                serializedObject.ApplyModifiedProperties();
                return;
            }
            
            isExpanded.boolValue = JuicyEditorUtils.FoldoutHeader(isExpanded.boolValue, "Properties", () =>
            {
                foreach (SerializedProperty child in GetChildren()) {
                    if (FilterProperty(child)) {
                        continue;
                    }
                    
                    if (child.type.Contains("UnityEvent")) {

                        using (new EditorGUILayout.HorizontalScope()) {
                            GUILayout.Space(35);
                            EditorGUILayout.PropertyField(child);
                        }
                    } else {
                        if (SpecialFilterProperty(child)) {
                            SpecialDraw(child);
                        } else {
                            EditorGUILayout.PropertyField(child);
                        }
                    }
                }

                // Draw ease always at the end
                if (ease != null) {
                    EditorGUILayout.PropertyField(ease);
                }
            });

            serializedObject.ApplyModifiedProperties();
        }

        private bool ContainsOnlyScriptAndTimingProperty()
        {
            return childrenCount == 2;
        }

        protected virtual bool FilterProperty(SerializedProperty property)
        {
            return property.name.Equals(Script) ||
                   property.name.Equals(nameof(timing)) ||
                   property.name.Equals(nameof(ease));
        }

        protected virtual bool SpecialFilterProperty(SerializedProperty property)
        {
            return false;
        }

        protected virtual void SpecialDraw(SerializedProperty child) {}
    }
    
    [CustomEditor(typeof(JuicyFeedbackLightBase), true)]
    public sealed class JuicyFeedbackLightBaseEditor : JuicyFeedbackBaseEditor {}
    
    [CustomEditor(typeof(JuicyFeedbackObjectBase), true)]
    public sealed class JuicyFeedbackObjectBaseEditor : JuicyFeedbackBaseEditor {}
    
    [CustomEditor(typeof(JuicyFeedbackTimeBase), true)]
    public sealed class JuicyFeedbackTimeBaseEditor : JuicyFeedbackBaseEditor {}
}