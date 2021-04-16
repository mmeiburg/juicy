using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(Ease))]
    public sealed class EaseDrawer : JuicyPropertyDrawerBase
    {
        private const float ExpandedCurveHeight = 60f;
        
        private SerializedProperty presets;
        private SerializedProperty curve;
        private AnimationCurve[] curves;
        
        private readonly string[] prefixes = {
            "Quad",
            "Cubic",
            "Expo",
            "Quart",
            "Quint",
            "Circ",
            "Sine",
            "Elastic",
            "Bounce",
            "Back",
            "Custom"
        };
        
        private string[] names;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (property.isExpanded ? ExpandedCurveHeight : SingleLineHeight) + StandardSpacing;
        }

        private void Initialize(SerializedProperty property)
        {
            if (curve != null) {
                return;
            }
            
            string path = JuicyEditorUtils.GetPluginRootPath() + "Editor/EasingCurves.curves";

            CacheProperty(ref curve, property, nameof(curve));
            
            Object presetObject = AssetDatabase
                .LoadAssetAtPath<Object>(path);

            presets = new SerializedObject(presetObject)
                .FindProperty("m_Presets");
            
            curves = new AnimationCurve[presets.arraySize];
            names = new string[presets.arraySize];
            
            for (int i = 0; i < presets.arraySize; i++) {

                SerializedProperty present = presets.GetArrayElementAtIndex(i);
                
                names[i] = BuildEaseMenu(present
                    .FindPropertyRelative("m_Name").stringValue);
                
                curves[i] = new AnimationCurve(present
                    .FindPropertyRelative("m_Curve").animationCurveValue.keys);
            }
        }
        
        private string BuildEaseMenu(string name)
        {
            foreach (var t in prefixes) {
                if (name.StartsWith(t)) {
                    return $"{t}/{name.Substring(t.Length)}";
                }
            }

            return name;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {
                
                if (EditorGUIUtility.hierarchyMode) {
                    int num = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
                    position.xMin += (float)num - 4;
                }
                
                Initialize(property);

                Rect foldoutRect = new Rect(position) {
                    height = SingleLineHeight,
                    width = EditorGUIUtility.labelWidth - 25
                };

                property.isExpanded = EditorGUI.Foldout(
                    foldoutRect,
                    property.isExpanded, label, true);
            
                DrawCurve(position, foldoutRect, property);
            }
        }

        private void DrawCurve(Rect position, Rect foldoutRect, SerializedProperty property)
        {
            Rect curveRect = new Rect(position) {
                x = position.x + foldoutRect.width,
                y = foldoutRect.y + StandardSpacing,
                width = position.width - foldoutRect.width - JuicyStyles.PaneOptionsIcon.width,
                height = property.isExpanded ? ExpandedCurveHeight : SingleLineHeight
            };

            curve.animationCurveValue = EditorGUI.CurveField(curveRect, curve.animationCurveValue);
            
            Rect menuRect = new Rect(position) {
                x = curveRect.x + curveRect.width,
                y = curveRect.y,
                width = JuicyStyles.PaneOptionsIcon.width,
                height = JuicyStyles.PaneOptionsIcon.height,
            };

            if (EditorGUI.DropdownButton(menuRect, new GUIContent(JuicyStyles.PaneOptionsIcon),
                FocusType.Passive, JuicyStyles.IconButtonStyle)) {
                CreateContextMenu();  
            }
        }

        private void CreateContextMenu()
        {
            var e = Event.current;
            Vector2 position = e.mousePosition;
            var menu = new GenericMenu();

            for (int i = 0; i < names.Length; i++) {
                string name = names[i];
                
                menu.AddItem(
                    JuicyEditorUtils.GetContent(name), 
                    false, 
                    ChangeCurve,
                    i);
            }

            menu.DropDown(new Rect(position, Vector2.zero));
            e.Use();
        }

        private void ChangeCurve(object index)
        {
            curve.animationCurveValue = new AnimationCurve(curves[(int) index].keys);
            curve.serializedObject.ApplyModifiedProperties();
        }
    }
}