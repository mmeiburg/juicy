using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    public static class JuicyStyles
    {
        public static GUIStyle IconButtonStyle { get; }
        public static GUIStyle BoxStyle { get; }
        public static GUIStyle FeedbackItemBoxStyle { get; }
        public static GUIStyle IconJuicyStyle { get; }
        public static GUIStyle FeedbackIconStyle { get; }
        public static GUIStyle FoldoutStyle { get; }
        public static GUIStyle FeedbackEditorStyle { get; }
        public static GUIStyle HeaderStyle { get; }
        public static GUIStyle HeaderToggleStyle { get; }
        public static GUIStyle SmallToggle { get; }
        public static GUIStyle HeaderFoldout { get; }
        public static GUIStyle ToggleGroupStyle { get; }
        public static GUIStyle ToggleGroupLabelStyle { get; }
        public static GUIStyle ToggleGroupLabelSelectedStyle { get; }
        
        public static Texture IconJuicy { get; }
        public static Texture DefaultFeedbackIcon { get; }
        public static Texture ValidIcon { get; }
        public static Texture InvalidIcon { get; }
        
        public static readonly GUIStyle RlDraggingHandle = "RL DragHandle";
        public static readonly GUIStyle RlPreButton = "RL FooterButton";
        public static readonly GUIStyle RlHeaderBackground = "RL Header";
        public static readonly GUIStyle RlEmptyHeaderBackground = "RL Empty Header";
        public static readonly GUIStyle RlFooterBackground = "RL Footer";
        public static readonly GUIStyle RlBoxBackground = "RL Background";
        public static readonly GUIStyle RlElementBackground = "RL Element";
        public static GUIContent ListIsEmpty = EditorGUIUtility.TrTextContent("List is Empty");
        
        public static GUIContent IconToolbarPlus = EditorGUIUtility
            .TrIconContent("Toolbar Plus");
        public static GUIContent IconToolbarPlusMore = EditorGUIUtility.TrIconContent("Toolbar Plus More", "Add item to list");

        private static readonly Texture2D PaneOptionsIconDark = (Texture2D) EditorGUIUtility
            .Load("Builtin Skins/DarkSkin/Images/pane options.png");
        
        private static readonly Texture2D PaneOptionsIconLight = (Texture2D) EditorGUIUtility
            .Load("Builtin Skins/LightSkin/Images/pane options.png");

        public static Texture2D PaneOptionsIcon =>
            EditorGUIUtility.isProSkin ? PaneOptionsIconDark : PaneOptionsIconLight;

        public static Color HeaderBackground =>
            EditorGUIUtility.isProSkin ? HeaderBackgroundDark : HeaderBackgroundLight;
        
        public static Color ReferenceColor => 
            EditorGUIUtility.isProSkin ? ReferenceHighlightedColorDark : ReferenceHighlightedColorLight;

        private static readonly Color HeaderBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
        private static readonly Color HeaderBackgroundLight = new Color(1f, 1f, 1f, 0.2f);
        private static readonly Color ReferenceHighlightedColorDark = new Color(0f,0.5f,1f, 1f);
        private static readonly Color ReferenceHighlightedColorLight = new Color(0f,0.5f,1f, 0.3f);
        
        public static readonly GUIContent IconToolbarMinus =
            EditorGUIUtility.TrIconContent("Toolbar Minus", "Remove item from list");

        static JuicyStyles()
        {
            IconButtonStyle = new GUIStyle(GUI.skin.FindStyle("IconButton") ?? EditorGUIUtility
                                               .GetBuiltinSkin(EditorSkin.Inspector)
                                               .FindStyle("IconButton")) {
                margin = new RectOffset(0, 0, 2, 0)
            };

            FeedbackEditorStyle = new GUIStyle {
                margin = new RectOffset(0, 17, 5, 5)
            };

            BoxStyle = new GUIStyle(EditorStyles.toolbar) {
                padding = new RectOffset(3, 3, 4, 4),
                margin = new RectOffset(0, 3, 0, 0),
                stretchHeight = true,
                stretchWidth = true,
                fixedHeight = 0.0f
            };
            
            FeedbackItemBoxStyle = new GUIStyle(EditorStyles.toolbar) {
                padding = new RectOffset(3, 3, 4, 4),
                margin = new RectOffset(0, 0, 0, 0),
                stretchHeight = true,
                stretchWidth = true,
                fixedHeight = 0.0f,
            };

            IconJuicyStyle = new GUIStyle {
                margin = new RectOffset(10, 0, 0, 0),
                fixedWidth = EditorGUIUtility.singleLineHeight + 10,
                fixedHeight = EditorGUIUtility.singleLineHeight
            };
            
            FeedbackIconStyle = new GUIStyle {
                margin = new RectOffset(0, 0, 4, 0),
                fixedWidth = EditorGUIUtility.singleLineHeight - 5,
                fixedHeight = EditorGUIUtility.singleLineHeight - 5,
            };

            FoldoutStyle = new GUIStyle(EditorStyles.foldout) {
                //alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
                //fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            SmallToggle = new GUIStyle("ShurikenToggle") {
                fixedWidth = EditorGUIUtility.singleLineHeight,
                fixedHeight = EditorGUIUtility.singleLineHeight
            };

            HeaderStyle = new GUIStyle(EditorStyles.boldLabel);

            HeaderToggleStyle = new GUIStyle(EditorStyles.toggle) {
                fontStyle = FontStyle.Bold
            };

            HeaderFoldout = new GUIStyle(EditorStyles.foldoutHeader) {
                margin = new RectOffset(16, 7, 0, 0),
                fixedHeight = 25,
                fontSize = 11,
                fontStyle = FontStyle.Bold
            };
            
            ToggleGroupStyle = new GUIStyle(EditorStyles.foldoutHeader) {
                fontSize = 10,
                fontStyle = FontStyle.Normal,
            };
            
            ToggleGroupLabelStyle = new GUIStyle(EditorStyles.boldLabel) {
                padding = new RectOffset(-10,0,0,0),
                fontSize = 10
            };

            ToggleGroupLabelSelectedStyle = new GUIStyle(ToggleGroupLabelStyle) {
                normal = {textColor = EditorStyles.linkLabel.normal.textColor},
            };

            IconJuicy = AssetDatabase
                .LoadAssetAtPath<Texture>(JuicyEditorUtils.GetPluginRootPath() + "Images/img_juicy.png");

            DefaultFeedbackIcon = AssetDatabase
                .LoadAssetAtPath<Texture>(JuicyEditorUtils.GetPluginRootPath() + $"Images/img_feedback_default.png");

            InvalidIcon = EditorGUIUtility.IconContent("console.erroricon.sml").image;
            ValidIcon = EditorGUIUtility.IconContent("Collab").image;
        }
    }
}