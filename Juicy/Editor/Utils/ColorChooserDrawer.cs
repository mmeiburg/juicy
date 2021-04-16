using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(ColorChooser))]
    public sealed class ColorChooserDrawer : JuicyPropertyDrawerBase
    {
        private SerializedProperty useHdr;
        private SerializedProperty hdrColor;
        private SerializedProperty rgbColor;
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return IsMinimalWidth ? EditorGUI.GetPropertyHeight(property) + 
                                    SingleLineHeight +
                                    StandardSpacing
                : EditorGUI.GetPropertyHeight(property);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {

                CacheProperty(ref useHdr, property, nameof(useHdr));
                CacheProperty(ref hdrColor, property, nameof(hdrColor));
                CacheProperty(ref rgbColor, property, nameof(rgbColor));
                
                float width = IsMinimalWidth ? 10 : 40;
                
                Rect valueRect = new Rect(position) {
                    width = position.width - width,
                    height = EditorGUIUtility.singleLineHeight
                };
                
                Rect buttonRect = new Rect(position) {
                    y = valueRect.y,
                    x = position.x + valueRect.width,
                    width = width,
                    height = EditorGUIUtility.singleLineHeight
                };
                
                EditorGUI.PropertyField(valueRect,
                    useHdr.boolValue
                        ? hdrColor : rgbColor, 
                    new GUIContent(property.displayName));
                
                if (GUI.Button(buttonRect, useHdr.boolValue ? "HDR" : "RGB")) {
                    useHdr.boolValue = !useHdr.boolValue;
                }
            }
        }
    }
}