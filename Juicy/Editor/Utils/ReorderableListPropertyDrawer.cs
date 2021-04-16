using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TinyTools.Juicy
{
    public abstract class ReorderableListPropertyDrawer<T> : JuicyPropertyDrawerBase where T : Object
    {
        private SerializedProperty listWrapperProperty;
        private SerializedObject serializedObject;

        private ReorderableList reorderableList;
        private Rect dragAndDropRect;
        private Object[] objects;

        protected abstract SerializedProperty GetListProperty(SerializedProperty property);
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Initialize(property);

            float height = SingleLineHeight;

            return listWrapperProperty.isExpanded ? reorderableList.GetHeight() + height +  StandardSpacing : height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {
                
                serializedObject.Update();
                
                if (EditorGUIUtility.hierarchyMode) {
                    int num = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
                    position.xMin += num - 4;
                }
            
                Rect foldoutRect = new Rect(position) {
                    height = SingleLineHeight + StandardSpacing,
                };

                listWrapperProperty.isExpanded = EditorGUI.Foldout(foldoutRect,listWrapperProperty.isExpanded,
                    listWrapperProperty.displayName, true);

                if (!listWrapperProperty.isExpanded) {
                    return;
                }

                position.y += SingleLineHeight + StandardSpacing;
                //position.x += 32;
                position.width -= 32;
            
                reorderableList.DoList(position);
            
                HandleDragAndDrop();

                serializedObject.ApplyModifiedProperties();
            }
        }

        private void Initialize(SerializedProperty property)
        {
            if (reorderableList != null) {
                return;
            }
            
            listWrapperProperty = GetListProperty(property);
            
            serializedObject = property.serializedObject;
            
            reorderableList = new ReorderableList(serializedObject, listWrapperProperty,  
                true, false, true, false);

            InitializeHeaderCallback();
            InitializeElementCallback();
            InitializeRemoveCallback();
            InitializeHeightCallback();

            reorderableList.headerHeight = 3;
            reorderableList.footerHeight += 3;
        }

        private void InitializeHeaderCallback()
        {
            reorderableList.drawHeaderCallback = rect =>
            {
                rect.y -= EditorGUIUtility.singleLineHeight;

                float height = EditorGUIUtility.singleLineHeight;
                
                rect.height = reorderableList.count == 0 ? height * 3: height;

                dragAndDropRect = rect;
                
            };
        }

        private void InitializeElementCallback()
        {
            const float xShift = 0;
            const float deleteButtonWidth = 20;
            
            reorderableList.drawElementCallback = (rect, index, active, focused) =>
            {
                if (index >= reorderableList.count) {
                    return;
                }
                
                SerializedProperty property = listWrapperProperty.GetArrayElementAtIndex(index);
                
                Rect propertyRect = new Rect(rect) {
                    x = rect.x - xShift,
                    width = rect.width + xShift - deleteButtonWidth
                };
                
                Rect buttonRect = new Rect(rect) {
                    x = propertyRect.x + propertyRect.width,
                    width = deleteButtonWidth
                };
                
                EditorGUI.PropertyField(propertyRect, property, GUIContent.none);
                serializedObject.ApplyModifiedProperties();

                if (GUI.Button(buttonRect,JuicyStyles.IconToolbarMinus, JuicyStyles.RlPreButton)) {
                    RemoveItem(index);
                }
            };
        }

        private void AddObject(T obj, int index)
        {
            listWrapperProperty.GetArrayElementAtIndex(index).objectReferenceValue = obj;
                
            serializedObject.ApplyModifiedProperties();
        }

        private void AddObjects()
        {
            foreach (var t in objects) {
                listWrapperProperty.arraySize++;
                AddObject((T)t, listWrapperProperty.arraySize - 1);
            }
        }

        private void InitializeRemoveCallback()
        {
            reorderableList.onRemoveCallback = list => { RemoveItem(list.index); };
        }

        private void RemoveItem(int index)
        {
            SerializedProperty property = listWrapperProperty
                .GetArrayElementAtIndex(index);

            if (property.objectReferenceValue != null) {
                listWrapperProperty.DeleteArrayElementAtIndex(index);
                listWrapperProperty.DeleteArrayElementAtIndex(index);
            } else {
                listWrapperProperty.DeleteArrayElementAtIndex(index);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
        
        private void InitializeHeightCallback()
        {
            reorderableList.elementHeightCallback = index =>
            {
                if (index >= reorderableList.count) {
                    return 0;
                }
                
                SerializedProperty property = listWrapperProperty.GetArrayElementAtIndex(index);

                if (property == null) {
                    return 0;
                }
                
                return EditorGUI.GetPropertyHeight(property) +
                    EditorGUIUtility.standardVerticalSpacing;
            };
        }

        private void HandleDragAndDrop()
        {
            var e = Event.current;
            
            if (!dragAndDropRect.Contains(e.mousePosition)) {
                return;
            }

            objects = DragAndDrop.objectReferences;

            if (objects == null || objects.Length <= 0) {
                return;
            }
            
            if (!(objects[0] is AudioClip)) {
                return;
            }
            
            EventType eventType = e.type;
            
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform){
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag();
                    
                    AddObjects();
                }
                e.Use();
            }
        }
    }

    [CustomPropertyDrawer(typeof(AudioClipList))]
    public sealed class ReorderableAudioClipListPropertyDrawer : ReorderableListPropertyDrawer<AudioClip>
    {
        protected override SerializedProperty GetListProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("clips");
        }
    }
}