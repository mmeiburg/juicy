using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    public static class JuicyEditorUtils
    {
        private static readonly Dictionary<string, GUIContent> MenuContentCache;
        
        private const string PluginName = "Juicy";
        private const string AssetsName = "Assets";

        private static string rootPluginPath = "";
        
        static JuicyEditorUtils()
        {
            MenuContentCache = new Dictionary<string, GUIContent>();
        }
        
        public static Texture GetIcon(string name)
        {
            Texture texture = AssetDatabase
                .LoadAssetAtPath<Texture>(GetPluginRootPath() + $"Images/{name}.png");

            return texture == null ? JuicyStyles.DefaultFeedbackIcon : texture;
        }

        public static string GetPluginRootPath()
        {
            if (!rootPluginPath.Equals(string.Empty)) {
                return rootPluginPath;
            }
            
            var res = Directory.GetFiles(Application.dataPath, 
                typeof(JuicyEditorUtils).Name + ".cs", SearchOption.AllDirectories);
            
            if (res.Length == 0) {
                return null;
            }

            string path = res[0];
            int index = path.IndexOf(AssetsName, StringComparison.Ordinal);
            string assetPath = path.Substring(index);
            
            int pluginNameIndex = assetPath.IndexOf(PluginName, StringComparison.Ordinal);
            rootPluginPath = assetPath.Substring(0, pluginNameIndex + PluginName.Length + 1);
            
            return rootPluginPath;
        }
        
        internal static bool DrawHeader(
            int index,
            ref string title,
            bool isExpanded, ref bool isActive, 
            JuicyFeedbackBase target,
            out Rect contentRect,
            Action<int> showMenu,
            int draggedIndex)
        {
            var position = GUILayoutUtility.GetRect(1f,
                EditorGUIUtility.singleLineHeight);

            position.x += 5;

            if (index == draggedIndex) {
                position.y = Event.current.mousePosition.y;
            }
            
            Texture2D menuIcon = JuicyStyles.PaneOptionsIcon;

            Rect dragRect = new Rect(position) {
                y = position.y + menuIcon.height / 2f - 1,
                width = 12,
                height = 12
            };
            Rect foldoutRect = new Rect(position) {
                x = dragRect.x + dragRect.width + 4,
                width = 16
            };
            Rect enabledRect = new Rect(position) {
                x = foldoutRect.x + foldoutRect.width,
                width = 16
            };
            
            Rect textureRect = new Rect(position) {
                x = enabledRect.x + enabledRect.width + 4,
                y = position.y + 2,
                width = 14,
                height = 14
            };

            Rect labelRect = new Rect(position) {
                x = textureRect.x + textureRect.width + 6,
                y = position.y + 1,
                width = 60,
            };
            
            Rect menuRect = new Rect(position) {
                x = position.width,
                y = position.y + 1,
                width = menuIcon.width
            };
            
            Rect textfieldRect = new Rect(position) {
                xMin = labelRect.x + labelRect.width,
                xMax = menuRect.x - 4
            };
            
            contentRect = position;
            contentRect.x = 23f;
            contentRect.width -= 23f;
            
            // Background
            //EditorGUI.DrawRect(labelRect, Color.red);
            //EditorGUI.DrawRect(contentRect, new Color(1,1,1, 0.5f));

            if (Event.current.type == EventType.Repaint) {
                JuicyStyles.RlDraggingHandle.Draw(dragRect, 
                    false, false, false, false);
            }

            using (new EditorGUI.DisabledScope(!isActive)) {
                Texture t = GetIcon(FeedbackAttribute.GetIcon(target.GetType()));
                if (t != null) {
                    GUI.DrawTexture(textureRect, t, ScaleMode.ScaleAndCrop);
                }

                bool isReference = target.referenceCount > 0;

                EditorGUI.LabelField(labelRect, 
                    new GUIContent(
                    $"{FeedbackAttribute.GetName(target.GetType())} {(isReference ? "*" : string.Empty)}",
                    $"{(isReference ? "Is a reference" : "")}"),
                    new GUIStyle(EditorStyles.label) {
                    padding = new RectOffset(0,0,0,3)
                });

                Color c = GUI.backgroundColor;
                if (isReference)
                    GUI.backgroundColor = JuicyStyles.ReferenceColor;
                
                title = EditorGUI.TextField(textfieldRect, GUIContent.none, title);
                GUI.backgroundColor = c;
            }

            isExpanded = GUI.Toggle(foldoutRect, isExpanded, GUIContent.none, EditorStyles.foldout);
            isActive = GUI.Toggle(enabledRect, isActive, GUIContent.none);

            if (EditorGUI.DropdownButton(menuRect, new GUIContent(JuicyStyles.PaneOptionsIcon),
                FocusType.Passive, JuicyStyles.IconButtonStyle)) {
                
                if (Event.current.type != EventType.Repaint) {
                    showMenu.Invoke(index);
                    Event.current.Use();
                }
            }

            return isExpanded;
        }
        
        public static void DrawLine(Rect position, float yOffset)
        {
            EditorGUI.DrawRect(new Rect {
                x = position.x + 5,
                y = position.y + yOffset + 1,
                height = 1,
                width = position.width - 5
            }, new Color ( 0.3f,0.3f,0.3f, 1 ) );
        }

        public static GUIContent GetContent(string textAndTooltip)
        {
            if (string.IsNullOrEmpty(textAndTooltip))
                return GUIContent.none;

            if (MenuContentCache.TryGetValue(textAndTooltip, out var content)) {
                return content;
            }

            var s = textAndTooltip.Split('|');

            content = new GUIContent(s[0]);


            if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
                content.tooltip = s[1];

            MenuContentCache.Add(textAndTooltip, content);

            return content;
        }

        public static bool FoldoutHeader(bool isExpanded, string name, Action drawAction)
        {
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;
            
            isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(isExpanded, name, JuicyStyles.HeaderFoldout);
            
            if (isExpanded) {
                drawAction?.Invoke();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUI.indentLevel = indentLevel;

            return isExpanded;
        }

        public static void HorizontalLine()
        {
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
        }
     
        public static Type[] GetAllSubtypes(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => type.IsAssignableFrom(x) && x != type && !x.IsAbstract)
                .OrderBy(x => x.Name)
                .ToArray();
        }
        
        public static void CopyFeedback(SerializedProperty property, 
            JuicyFeedbackBase feedback)
        {
            FeedbackCopyHelper.Copy(new SerializedObject(feedback));
        }

        public static void PasteFeedback(SerializedProperty property,
            JuicyFeedbackBase feedback)
        {
            SerializedObject serialized = new SerializedObject(feedback);

            FeedbackCopyHelper.Paste(serialized);
            serialized.ApplyModifiedProperties();
        }
        
                
        public static IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
        {
            SerializedProperty copy = property.Copy();
            
            copy.NextVisible(true);
            bool hasNext = true;
            
            while (hasNext) {
                if (SerializedProperty.EqualContents(copy, property.GetEndProperty())) {
                    yield break;
                }
 
                yield return copy;
 
                hasNext = copy.NextVisible(false);
            }
        }
    }
}