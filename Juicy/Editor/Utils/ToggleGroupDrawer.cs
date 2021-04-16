using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(ToggleGroup), true)]
    public sealed class ToggleGroupDrawer : JuicyPropertyDrawerBase
    {
        private SerializedProperty isActive;
        
        private float height;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheProperty(ref isActive, property, nameof(isActive));
            
            return SingleLineHeight +
                   (property.isExpanded ? height + StandardSpacing * 2 : 0);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {

                //position.x -= 4;

                Rect foldoutRect = new Rect(position) {
                    height = SingleLineHeight,
                };

                Rect labelRect = new Rect(position) {
                    x = position.x + 37f,
                    y = foldoutRect.y,
                    height = foldoutRect.height,
                    width = 200f
                };
                
                float width = EditorGUI.indentLevel > 1 ? 50 : 32;
                
                Rect toggleRect = new Rect(position) {
                    x = position.x + width,
                    y = foldoutRect.y,
                    width = labelRect.width + width,
                    height = foldoutRect.height
                };
                
                var e = Event.current;

                if(e.isMouse && e.button == 0 && toggleRect.Contains(e.mousePosition))
                {
                    if(e.type == EventType.MouseDown) {
                        isActive.boolValue = !isActive.boolValue;
                        e.Use();
                    }
                }
                
                bool hierarchyMode = EditorGUIUtility.hierarchyMode;
                EditorGUIUtility.hierarchyMode = false;
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none, true);
                EditorGUIUtility.hierarchyMode = hierarchyMode;
                
                using (var check = new EditorGUI.ChangeCheckScope()) {
                    
                    isActive.boolValue = GUI.Toggle(toggleRect, isActive.boolValue, string.Empty);
                    
                    if (check.changed && isActive.boolValue) {
                        property.isExpanded = true;
                    }
                }
                
                EditorGUI.LabelField(labelRect, property.displayName);
                
                if (property.isExpanded) {
                    DrawChildren(position, property);
                }
            }
        }

        private void DrawChildren(Rect position, SerializedProperty property)
        {
            height = 0;
            
            using (new EditorGUI.IndentLevelScope())
            using (new EditorGUI.DisabledScope(!isActive.boolValue))
            {
                float yPos = SingleLineHeight;

                foreach (SerializedProperty child in GetChildren(property)) {

                    if (child.name == nameof(isActive)) {
                        continue;
                    }

                    float h = EditorGUI.GetPropertyHeight(child);
                    Rect rect = new Rect(position) {
                        y = position.y + yPos + StandardSpacing,
                        height = h,
                        width = position.width + 2f
                    };
                    
                    if (child.type.Contains("UnityEvent")) {
                        rect.x += 32f;
                        rect.width -= 32f;
                        EditorGUI.PropertyField(rect, child);
                    }

                    EditorGUI.PropertyField(rect, child);

                    yPos += h;
                    height += h;
                }
            }
        }
    }
}