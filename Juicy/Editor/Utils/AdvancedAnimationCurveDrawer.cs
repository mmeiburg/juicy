using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(AdvancedAnimationCurve))]
    public class AdvancedAnimationCurveDrawer : JuicyPropertyDrawerBase
    {
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
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {

                Initialize(property);

                Rect curveRect = new Rect(position) {
                    width = position.width - JuicyStyles.PaneOptionsIcon.width,
                    height = SingleLineHeight
                };

                curve.animationCurveValue = EditorGUI.CurveField(
                    curveRect, label, curve.animationCurveValue);

                Rect menuRect = new Rect(position) {
                    x = position.x + curveRect.width,
                    y = position.y + 2,
                    width = JuicyStyles.PaneOptionsIcon.width,
                    height = JuicyStyles.PaneOptionsIcon.height,
                };

                if (EditorGUI.DropdownButton(menuRect, new GUIContent(JuicyStyles.PaneOptionsIcon),
                    FocusType.Passive, JuicyStyles.IconButtonStyle)) {
                    CreateContextMenu();
                }
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