using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(Target<>))]
    public class TargetDrawer<T> : JuicyPropertyDrawerBase where T : Object
    {
        private SerializedProperty targetIsSelf;
        private SerializedProperty selfTarget;
        private SerializedProperty otherTarget;
        
        private T targetComponent;

        private bool isValid;
        private bool isSelf;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return SingleLineHeight +
                   (isValid ? 0 : SingleLineHeight * 2 + StandardSpacing * 2);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {

                if (Event.current.type == EventType.Layout) {
                    return;
                }
                
                targetComponent = null;
                
                CacheProperty(ref targetIsSelf, property, nameof(targetIsSelf));
                CacheProperty(ref selfTarget, property, nameof(selfTarget));
                CacheProperty(ref otherTarget, property, nameof(otherTarget));
                
                float yPosition = 0f;

                if (isSelf != targetIsSelf.boolValue) {
                    GUI.changed = true;
                }
                
                isSelf = targetIsSelf.boolValue;
                
                if (isSelf) {
                    targetComponent = (property.serializedObject.targetObject as Component)?
                        .GetComponent<T>();

                    isValid = targetComponent != null && selfTarget.objectReferenceValue
                              == targetComponent;

                } else {
                    isValid = otherTarget.objectReferenceValue != null;
                }

                Rect targetRect = new Rect(position) {
                    width = position.width - JuicyStyles.PaneOptionsIcon.width,
                    height = EditorGUIUtility.singleLineHeight
                };

                yPosition += EditorGUIUtility.singleLineHeight;
            
                Rect menuRect = new Rect(position) {
                    x = position.x + targetRect.width,
                    y = targetRect.y,
                    width = JuicyStyles.PaneOptionsIcon.width,
                    height = JuicyStyles.PaneOptionsIcon.height,
                };

                if (EditorGUI.DropdownButton(menuRect, new GUIContent(JuicyStyles.PaneOptionsIcon),
                    FocusType.Passive, JuicyStyles.IconButtonStyle)) {
                    
                    CreateContextMenu();  
                }

                if (isSelf) {
                    selfTarget.objectReferenceValue = targetComponent;

                    Rect labelRect = new Rect(position) {
                        width = EditorGUIUtility.labelWidth,
                        height = SingleLineHeight
                    };

                    EditorGUI.LabelField(labelRect, new GUIContent("Target (This)", "Look for a valid component on this GameObject"));
                    using (new EditorGUI.DisabledScope(true)) {

                        float width = (EditorGUI.indentLevel * 16 - 5);
                        
                        Rect r = new Rect(position) {
                            x = position.x + EditorGUIUtility.labelWidth - width,
                            width = position.width - labelRect.width - menuRect.width + width,
                            height = SingleLineHeight
                        };
                        
                        EditorGUI.PropertyField(r, selfTarget, GUIContent.none);
                    }

                } else {
                    EditorGUI.PropertyField(targetRect, otherTarget,
                        new GUIContent("Target (Other)", "Look for a valid component on the referenced GameObject"));

                    otherTarget.serializedObject.ApplyModifiedProperties();
                }

                if (isValid) {
                    return;
                }

                Rect helpBox = new Rect(position) {
                    x = position.x,
                    y = position.y + yPosition + StandardSpacing * 2,
                    width = position.width,
                    height = SingleLineHeight * 2
                };
                    
                // Workaround to fix weird padding
                if (EditorGUIUtility.hierarchyMode) {
                    int num = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
                    helpBox.xMin += num;
                }
                    
                EditorGUI.HelpBox(helpBox, $"No {typeof(T).Name} on {(isSelf ? "this" : "target")} GameObject found", 
                    MessageType.Error);

            }
        }
        
        private void CreateContextMenu()
        {
            var e = Event.current;
            Vector2 position = e.mousePosition;
            
            var menu = new GenericMenu();
            bool isActive = targetIsSelf.boolValue;
            
            menu.AddItem(
                    JuicyEditorUtils.GetContent("This"), 
                    isActive, 
                    SetTargetIsSelf,
                    true);
            
            menu.AddItem(
                JuicyEditorUtils.GetContent("Other"), 
                !isActive, 
                SetTargetIsSelf,
                false);

            menu.DropDown(new Rect(position, Vector2.zero));
        }
        
        private void SetTargetIsSelf(object flag)
        {
            targetIsSelf.boolValue = (bool)flag;
            targetIsSelf.serializedObject.ApplyModifiedProperties();
        }
    }

    [CustomPropertyDrawer(typeof(RendererTarget))]
    public sealed class RendererTargetDrawer : TargetDrawer<Renderer> {}
    [CustomPropertyDrawer(typeof(AnimatorTarget))]
    public sealed class AnimatorTargetDrawer : TargetDrawer<Animator> {}
    [CustomPropertyDrawer(typeof(TransformTarget))]
    public sealed class TransformTargetDrawer : TargetDrawer<Transform> {}
    [CustomPropertyDrawer(typeof(AudioSourceTarget))]
    public sealed class AudioSourceTargetDrawer : TargetDrawer<AudioSource> {}
    [CustomPropertyDrawer(typeof(LightTarget))]
    public sealed class LightTargetDrawer : TargetDrawer<Light> {}
    [CustomPropertyDrawer(typeof(CameraTarget))]
    public sealed class CameraTargetDrawer : TargetDrawer<Camera> {}
}