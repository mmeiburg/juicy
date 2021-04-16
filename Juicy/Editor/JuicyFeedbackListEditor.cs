using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomEditor(typeof(JuicyFeedbackList))]
    public sealed class JuicyFeedbackListEditor : JuicyEditorBase
    {
        private Dictionary<JuicyFeedbackBase, Editor> editors;
        private SerializedProperty displayName;
        private SerializedProperty feedbackListProp;
        private SerializedProperty debugView;
        private int dropDownSelection;
        private Type[] types;
        private bool showAdvancedSettings;
        
        private JuicyFeedbackList juicyFeedbackList;
        private GenericMenu contextMenu;
        private GenericMenu addFeedbackMenu;
                
        private Action<int> showElementContextMenu;
        private int currentDraggedIndex = -1;
        private int currentHoveredIndex = -1;
        private readonly List<int> nonDragTargetIndices = new List<int>();
        
        private bool toggleAll = true;

        protected override void Initialize()
        {
            juicyFeedbackList = target as JuicyFeedbackList;
            
            editors = new Dictionary<JuicyFeedbackBase, Editor>();

            CacheProperty(ref displayName, "displayName");
            CacheProperty(ref feedbackListProp, "feedbackList");
            CacheProperty(ref debugView, "debugView");
            
            for (int i = 0; i < feedbackListProp.arraySize; i++) {
                AddEditor(feedbackListProp.GetArrayElementAtIndex(i)
                    .objectReferenceValue as JuicyFeedbackBase);
            }
            
            CreateContextMenu();
            CreateAddFeedbackMenu();

            showElementContextMenu = ShowElementContextMenu;
        }

        private void CreateAddFeedbackMenu()
        {
            addFeedbackMenu = new GenericMenu();
            types = JuicyEditorUtils.GetAllSubtypes(typeof(JuicyFeedbackBase));

            foreach (var type in types) {
                addFeedbackMenu.AddItem(JuicyEditorUtils.GetContent(
                        FeedbackAttribute.GetPath(type)
                        ),
                    false, () =>
                    {
                        AddFeedback(type);
                        serializedObject.ApplyModifiedProperties();
                    });
            }
        }
        
        private void CreateContextMenu()
        {
            contextMenu = new GenericMenu();
            contextMenu.AddItem(JuicyEditorUtils.GetContent("Reset"), 
                false, 
                ResetFeedback);
            contextMenu.AddSeparator(string.Empty);
            contextMenu.AddItem(JuicyEditorUtils.GetContent("Copy"), 
                false,
                CopyFeedback);

            if (FeedbackListCopyHelper.HasCopy) {
                contextMenu.AddItem(JuicyEditorUtils.GetContent("Paste"), 
                    false, 
                    PasteFeedback);
            } else {
                contextMenu.AddDisabledItem(JuicyEditorUtils.GetContent("Paste"));
            }
            contextMenu.AddSeparator(string.Empty);
            contextMenu.AddItem(JuicyEditorUtils.GetContent("Debug"), 
                debugView.boolValue, DebugView);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawName();
            
            HandleElementHiding();
            
            Undo.RecordObject(target, "Modified Feedback");
            PrefabUtility.RecordPrefabInstancePropertyModifications(target);

            DrawFeedbackHeader();
            DrawFeedbackElements();
            DrawFeedbackFooter();
            DrawAdvancedSettings();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawName()
        {
            EditorGUILayout.PropertyField(displayName, new GUIContent("Name"));
        }

        private void DrawFeedbackHeader()
        {
            feedbackListProp.isExpanded = JuicyEditorUtils.FoldoutHeader(feedbackListProp.isExpanded,
                $"Feedbacks ({feedbackListProp.arraySize})", () =>
                {
                    GUILayout.Space(8);
                    using (new EditorGUILayout.HorizontalScope(JuicyStyles.RlHeaderBackground)) {

                        GUILayout.Space(37);

                        using (var check = new EditorGUI.ChangeCheckScope()) {
                            toggleAll = GUILayout.Toggle(toggleAll, string.Empty);
                            if (check.changed) {
                                Toggle();
                            }
                        }
                        
                        GUILayout.Space(18);
                        GUILayout.Label("Type", EditorStyles.boldLabel);
                        GUILayout.Space(27);
                        GUILayout.Label("Description", EditorStyles.boldLabel);
                        GUILayout.FlexibleSpace();
                        if (EditorGUILayout.DropdownButton(new GUIContent(JuicyStyles.PaneOptionsIcon),
                            FocusType.Passive, JuicyStyles.IconButtonStyle)) {
                            ShowListContextMenu();
                        }
                        GUILayout.Space(2);
                    }
                });
        }

        private void Toggle()
        {
            for (int i = 0; i < feedbackListProp.arraySize; i++) {
                JuicyFeedbackBase feedback = feedbackListProp.GetArrayElementAtIndex(i)
                    .objectReferenceValue as JuicyFeedbackBase;

                if (feedback != null) {
                    feedback.isActive = toggleAll;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawFeedbackFooter()
        {
            if (!feedbackListProp.isExpanded) {
                return;
            }
            using (var r = new EditorGUILayout.VerticalScope()) {
                GUILayout.Space(EditorGUIUtility.singleLineHeight + 10);

                float rightEdge = r.rect.xMax - 10f;
                float leftEdge = rightEdge - 36f;
                
                Rect buttonBackgroundRect =
                    new Rect(leftEdge, r.rect.y, rightEdge - leftEdge, r.rect.height);
                
                Rect addButtonRect =
                    new Rect(leftEdge + 2, r.rect.y, 
                        rightEdge - leftEdge - 5, r.rect.height);

                if (Event.current.type == EventType.Repaint)
                {
                    JuicyStyles.RlFooterBackground.Draw(buttonBackgroundRect, 
                        false, false, 
                        false, false);
                }
                
                if (GUI.Button(addButtonRect, JuicyStyles.IconToolbarPlusMore,
                    JuicyStyles.RlPreButton)) {
                        
                    addFeedbackMenu.DropDown(
                        new Rect(Event.current.mousePosition, Vector2.zero));
                }
            }
        }

        private void HandleElementHiding()
        {
            for (int i = 0; i < feedbackListProp.arraySize; i++) {
                SerializedProperty property = feedbackListProp.GetArrayElementAtIndex(i);
                JuicyFeedbackBase feedback = property.objectReferenceValue as JuicyFeedbackBase;

                if (feedback == null) {
                    continue;
                }
                
                feedback.hideFlags = debugView.boolValue ? HideFlags.None : HideFlags.HideInInspector;
            }
        }

        private void DebugView()
        {
            debugView.boolValue = !debugView.boolValue;
            serializedObject.ApplyModifiedProperties();
        }

        private void CopyFeedback()
        {
            FeedbackListCopyHelper.Copy(feedbackListProp);
        }

        private void PasteFeedback()
        {
            FeedbackListCopyHelper.Paste(feedbackListProp,
                RemoveFeedbackItems, 
                AddFeedback);

            serializedObject.ApplyModifiedProperties();
            
            editors.Clear();
        }

        private void ResetFeedback()
        {
            if (EditorUtility.DisplayDialog(
                $"Reset Feedback", 
                $"Do you really want to reset the feedback?", 
                "Yes", "No")) {
                
                RemoveFeedbackItems();
            }
        }

        private void DrawFeedbackElements()
        {
            if (!feedbackListProp.isExpanded) {
                return;
            }
            using (new EditorGUILayout.VerticalScope(JuicyStyles.RlBoxBackground)) {

                if (feedbackListProp.arraySize == 0) {
                    GUILayout.Space(6);
                    EditorGUILayout.LabelField(JuicyStyles.ListIsEmpty);
                    GUILayout.Space(6);
                    return;
                }
                
                nonDragTargetIndices.Clear();
                
                for (var i = 0; i < feedbackListProp.arraySize; i++)
                {
                    if (i != currentDraggedIndex) {
                        nonDragTargetIndices.Add(i);
                    }
                }

                if (currentDraggedIndex != -1 && currentHoveredIndex != -1) {
                    nonDragTargetIndices.Insert(currentHoveredIndex, currentDraggedIndex);
                }
                
                foreach (int i in nonDragTargetIndices) {
                    DrawFeedbackElement(i);
                }
            }
        }

        private void DrawAdvancedSettings()
        {
            EditorGUILayout.EditorToolbar();
            showAdvancedSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showAdvancedSettings, "Advanced",
                JuicyStyles.HeaderFoldout);
            
            if (showAdvancedSettings) {
                EditorGUILayout.PropertyField(CacheProperty("playOnEnable"));

                using(new EditorGUI.DisabledScope(!Application.isPlaying))
                using (new EditorGUILayout.HorizontalScope()) {
                    
                    if (GUILayout.Button("Play")) {
                        juicyFeedbackList.Play();
                    }
                    
                    /*if (GUILayout.Button("Pause")) {
                        juicyFeedbackList.Pause();
                    }*/
    
                    if (GUILayout.Button("Stop")) {
                        juicyFeedbackList.Stop();
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void DrawFeedbackElement(int index)
        {
            SerializedProperty property = feedbackListProp.GetArrayElementAtIndex(index);
            JuicyFeedbackBase feedback = property.objectReferenceValue as JuicyFeedbackBase;
            
            if (feedback == null) {
                return;
            }

            Undo.RecordObject(feedback, "Modified Feedback");
            PrefabUtility.RecordPrefabInstancePropertyModifications(feedback);
            Rect contentRect;
            
            using (new EditorGUILayout.VerticalScope()) {
                string label = feedback.label;
                
                GUILayout.Space(3);

                property.isExpanded = JuicyEditorUtils.DrawHeader(
                    index,
                    ref label,
                    property.isExpanded,
                    ref feedback.isActive,
                    feedback, 
                    out contentRect,
                    showElementContextMenu,
                    currentDraggedIndex);

                GUILayout.Space(3);

                feedback.label = label;

                using (new EditorGUI.DisabledScope(!feedback.enabled)) {
                    DrawEditor(feedback, property.isExpanded); 
                }
            }
            
            var e = Event.current;
            
            if (contentRect.Contains(e.mousePosition)) {

                if (currentDraggedIndex != index) {
                    currentHoveredIndex = index;

                    if (currentDraggedIndex != -1) {
                        property.isExpanded = false;
                    }
                }
                
                EventType eventType = e.type;
                
                switch (eventType) {
                    case EventType.MouseDown when currentDraggedIndex == -1:
                        currentDraggedIndex = index;
                        property.isExpanded = false;
                    
                        e.Use();
                        break;
                    case EventType.MouseUp:
                        ReorderDraggedElement();
                
                        currentDraggedIndex = -1;
                        currentHoveredIndex = -1;
            
                        e.Use();
                        break;
                }
            }
        }

        private void ReorderDraggedElement()
        {
            if (currentDraggedIndex == -1) {
                return;
            }

            if (!feedbackListProp.MoveArrayElement(currentDraggedIndex, currentHoveredIndex)) {
                return;
            }

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }

        public override bool RequiresConstantRepaint()
        {
            return currentDraggedIndex != -1;
        }

        private void ShowListContextMenu()
        {
            contextMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
        }

        private void ShowElementContextMenu(int index)
        {
            SerializedProperty property = feedbackListProp.GetArrayElementAtIndex(index);
            JuicyFeedbackBase feedback = property.objectReferenceValue as JuicyFeedbackBase;
                
            var itemMenu = new GenericMenu();
            itemMenu.AddItem(JuicyEditorUtils.GetContent("Remove|Remove feedback from List"), 
                false, () => Remove(property, feedback, index));
            itemMenu.AddItem(JuicyEditorUtils.GetContent("Reset|Resets the feedback to default"), 
                false, () => ResetEditor(property, feedback));
            itemMenu.AddSeparator(string.Empty);
            itemMenu.AddItem(JuicyEditorUtils.GetContent("Copy|Copy feedback values"), 
                false, () =>
                {
                    FeedbackCopyHelper.CopyReference(feedback);
                    JuicyEditorUtils.CopyFeedback(property, feedback);
                });
            
            if (FeedbackCopyHelper.HasCopy) {
                itemMenu.AddItem(JuicyEditorUtils.GetContent("Paste|Paste feedback values"), 
                    false, () => 
                    JuicyEditorUtils.PasteFeedback(property, feedback));
                itemMenu.AddItem(JuicyEditorUtils.GetContent("Paste as new|Paste copied feedback values as a new feedback"), 
                    false, PasteAsNew);

                itemMenu.AddItem(JuicyEditorUtils.GetContent("Paste Reference|Paste feedback reference"),
                    false, () =>
                    {
                        FeedbackCopyHelper.PasteReference(juicyFeedbackList.feedbackList);
                    });

            } else {
                itemMenu.AddDisabledItem(JuicyEditorUtils.GetContent("Paste"));
            }

            Rect menuRect = new Rect(Event.current.mousePosition, Vector2.zero);
            menuRect.y -= 10;
            
            itemMenu.DropDown(menuRect);
        }

        private void ResetEditor(SerializedProperty property, JuicyFeedbackBase feedback)
        {
            int referenceCount = feedback.referenceCount;
            
            if (PrefabUtility.IsPartOfAnyPrefab(property.objectReferenceValue)) {
                PrefabUtility.RevertObjectOverride(feedback, InteractionMode.UserAction);
                feedback.referenceCount = referenceCount;
                return;
            }
            
            // Create a temporary feedback and copy the values
            JuicyFeedbackBase tmpFeedback = AddFeedback(feedback.GetType());
            int index = feedbackListProp.arraySize - 1;
            
            tmpFeedback.referenceCount = referenceCount;
            
            SerializedProperty tmp = feedbackListProp
                .GetArrayElementAtIndex(index);
            
            JuicyEditorUtils.CopyFeedback(tmp, tmpFeedback);
            JuicyEditorUtils.PasteFeedback(property, feedback);
            FeedbackCopyHelper.ClearCache();
            
            RemoveFeedback(tmp, tmpFeedback, index);

            serializedObject.ApplyModifiedProperties();
        }
        
        private void RemoveFeedbackItems()
        {
            for (int i = 0; i < feedbackListProp.arraySize; i++) {
                SerializedProperty item = feedbackListProp.GetArrayElementAtIndex(i);

                JuicyFeedbackBase feedback = item.objectReferenceValue as JuicyFeedbackBase;
                if (feedback == null) {
                    continue;
                }
                
                Undo.DestroyObjectImmediate(feedback);
                
                if (item.objectReferenceValue != null) {
                    item.objectReferenceValue = null;
                }
                
                feedbackListProp.DeleteArrayElementAtIndex(i);
            }
            
            for (int i = feedbackListProp.arraySize - 1; i >= 0; i--) {
                if (feedbackListProp.GetArrayElementAtIndex(i).objectReferenceValue == null) {
                    feedbackListProp.DeleteArrayElementAtIndex(i);
                }
            }
            
            juicyFeedbackList.feedbackList.Clear();
        }

        private void Remove(SerializedProperty property,
            JuicyFeedbackBase feedback, int index)
        {
            /*if (EditorUtility.DisplayDialog(
                $"Remove {label} Feedback", 
                $"Do you really want to remove the {label} Feedback?", 
                "Yes", "No")) {
            }*/

            if (feedback.referenceCount == 0) {
                RemoveFeedback(property, feedback, index);
            } else {
                FeedbackCopyHelper.RemoveCopyReference(feedback,
                    juicyFeedbackList.feedbackList);
            }
        }
        
        private void PasteAsNew()
        {
            JuicyFeedbackBase newFeedback = AddFeedback(FeedbackCopyHelper.Type);

            SerializedObject obj = new SerializedObject(newFeedback);
            obj.Update();
            FeedbackCopyHelper.Paste(obj);
            obj.ApplyModifiedProperties();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEditor(JuicyFeedbackBase feedback, bool isExpanded)
        {
            if (!isExpanded) {
                return;
            }

            using (new EditorGUI.DisabledScope(!feedback.isActive)) {
                using (new EditorGUILayout.VerticalScope(JuicyStyles.FeedbackEditorStyle)) {
                    if (!editors.ContainsKey(feedback)) {
                        AddEditor(feedback);
                    }

                    Editor editor = editors[feedback];
                    
                    CreateCachedEditor(feedback, feedback.GetType(), ref editor);
                    editor.OnInspectorGUI();
                }
            }
        }
        
        private void RemoveFeedback(SerializedProperty property, JuicyFeedbackBase feedback, int index)
        {
            Undo.DestroyObjectImmediate(feedback);
            
            if (property.objectReferenceValue == null) {
                feedbackListProp.DeleteArrayElementAtIndex(index);
                juicyFeedbackList.Remove(feedback);
            }
            
            for (int i = feedbackListProp.arraySize - 1; i >= 0; i--) {
                if (feedbackListProp.GetArrayElementAtIndex(i).objectReferenceValue == null) {
                    feedbackListProp.DeleteArrayElementAtIndex(i);
                }
            }
            
            editors.Remove(feedback);
        }

        private JuicyFeedbackBase AddFeedback(Type type)
        {
            GameObject gameObject = ((JuicyFeedbackList) target).gameObject;

            JuicyFeedbackBase newFeedback = Undo.AddComponent(gameObject, type)
                as JuicyFeedbackBase;
            
            if (newFeedback == null) {
                return null;
            }
            
            newFeedback.label = FeedbackAttribute.GetName(newFeedback.GetType());

            newFeedback.hideFlags = debugView.boolValue ? HideFlags.None : HideFlags.HideInInspector;

            AddEditor(newFeedback);

            feedbackListProp.arraySize++;

            feedbackListProp
                .GetArrayElementAtIndex(feedbackListProp.arraySize - 1)
                .objectReferenceValue = newFeedback;

            return newFeedback;
        }
        
        private void AddEditor(JuicyFeedbackBase feedback)
        {
            if (feedback == null)
                return;

            if (editors.ContainsKey(feedback)) {
                return;
            }

            Editor editor = null;
            CreateCachedEditor(feedback, null, ref editor);

            editors.Add(feedback, editor);
        }
    }
}