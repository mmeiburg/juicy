using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(JuicyFeedback))]
    public sealed class JuicyFeedbackDrawer : JuicyPropertyDrawerBase
    {
        private SerializedProperty feedbackList;
        private SerializedProperty feedbackName;

        private JuicyFeedbackList listReference;
        
        private float height = 0;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            height = 0;
            CacheProperty(ref feedbackList, property, nameof(feedbackList));

            Rect addRect = new Rect(position) {
                x = position.width - 6,
                width = 25
            };

            bool hasReference = feedbackList.objectReferenceValue != null;

            Rect referenceRect = new Rect(position) {
                width = position.width - (hasReference ? 0 : addRect.width)
            };

            string name = property.displayName;

            if (hasReference) {
                listReference = feedbackList.objectReferenceValue as JuicyFeedbackList;
                name = $"{name} ({listReference.displayName})";
            }

            EditorGUI.PropertyField(referenceRect, feedbackList,
                new GUIContent(name));

            if (hasReference) {
                return;
            }
            
            if (GUI.Button(addRect, new GUIContent(JuicyStyles.IconToolbarPlus.image,
                    "Adds a feedback component"),
                JuicyStyles.RlPreButton)) {
                AddNewList(property);
            }
        }

        private void AddNewList(SerializedProperty property)
        {
            if (feedbackList.objectReferenceValue != null) {
                return;
            }
            
            GameObject gameObject = (property.serializedObject.targetObject as Component)?
                .gameObject;
            
            JuicyFeedbackList feedback = Undo.AddComponent<JuicyFeedbackList>(gameObject);
            feedback.displayName = property.displayName;

            feedbackList.objectReferenceValue = feedback;

            property.serializedObject.ApplyModifiedProperties();
        }
    }

    /*private Editor editor;
    private GameObject gameObject;
    
    private Component component;
    
    private Object lastObjectReferenceValue;
        
    private SerializedProperty feedback;
    private JuicyFeedbackList feedbackList;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, editor) -
               SingleLineHeight;
        //+ (property.isExpanded ? SingleLineHeight + StandardSpacing : 0);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        using (new EditorGUI.PropertyScope(position, label, property)) {
            
            CacheProperty(ref feedback, property, "feedbackList");
            
            InitialiseFeedback(property);
            
            bool isExpanded = property.isExpanded;
            bool enabled = feedbackList.enabled;

            property.isExpanded = isExpanded;
            feedbackList.enabled = enabled;

            lastObjectReferenceValue = feedback.objectReferenceValue;
            DrawEditor();
            
            //if (isExpanded) {
            //}
        }
    }

    private void DrawEditor()
    {
        if (!editor) {
            Editor.CreateCachedEditor(
                feedback.objectReferenceValue, 
                typeof(JuicyFeedbackListEditor), ref editor);
        } else {
            editor.OnInspectorGUI();
        }
    }

    private void InitialiseFeedback(SerializedProperty property)
    {
        feedbackList = feedback.objectReferenceValue as JuicyFeedbackList;
        component = property.serializedObject.targetObject as Component;
        gameObject = component != null ? component.gameObject : null;

        // Everything works like normal, do nothing but hiding the component
        if (feedbackList != null) {
            feedbackList.hideFlags = feedbackList.debugView ? HideFlags.None : HideFlags.HideInInspector;
            return;
        }

        bool debugView = false;
        
        // Unity Reset Button got clicked, so compare with the last reference you had and
        // reset the last editor
        if (IsResetByUnity()) {
            Debug.Log("Reset");
            JuicyFeedbackList lastComponent = (JuicyFeedbackList) lastObjectReferenceValue;
            debugView = lastComponent.debugView;
            ((JuicyFeedbackListEditor)editor).ResetFeedbackByDefaultUnityFunction();
        }

        if (gameObject == null) {
            return;
        }
        
        // Component got destroyed, redo action is called and we are looking for
        // a JuicyFeedbackList Component which has the same id like our property combination
        if (feedbackList == null) {
            foreach (var c in gameObject.GetComponents<JuicyFeedbackList>()) {
                if (!GetId(property).Equals(c.id)) {
                    continue;
                }

                feedback.objectReferenceValue = c;
                lastObjectReferenceValue = c;
                return;
            }
        }
        
        feedbackList = Undo.AddComponent<JuicyFeedbackList>(gameObject);
        feedbackList.displayName = property.displayName;
        feedbackList.hideFlags = feedbackList.debugView ? HideFlags.None : HideFlags.HideInInspector;
        feedback.objectReferenceValue = feedbackList;
        feedbackList.debugView = debugView;
        feedbackList.id = GetId(property);
        feedbackList.callerComponent = component;

        if (editor == null || editor.target == feedback.objectReferenceValue) {
            return;
        }

        Editor.CreateCachedEditor(
            feedback.objectReferenceValue, 
            typeof(JuicyFeedbackListEditor), ref editor);
            
        editor.Repaint();
    }

    private string GetId(SerializedProperty property)
    {
        return $"{component.GetInstanceID()}{property.name}";
    }

    private bool IsResetByUnity()
    {
        return lastObjectReferenceValue != null && feedback.objectReferenceValue == null;
    }
    }*/
}