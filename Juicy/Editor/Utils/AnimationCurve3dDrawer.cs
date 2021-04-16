namespace TinyTools.Juicy
{
    /*[CustomPropertyDrawer(typeof(AnimationCurve3d))]
    public class AnimationCurve3dDrawer : JuicyPropertyDrawerBase
    {
        private SerializedProperty xCurve;
        private SerializedProperty yCurve;
        private SerializedProperty zCurve;
        private SerializedProperty type;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheProperty(ref xCurve, property, nameof(xCurve));
            CacheProperty(ref yCurve, property, nameof(yCurve));
            CacheProperty(ref zCurve, property, nameof(zCurve));
            CacheProperty(ref type, property, nameof(type));

            float height = (type.enumValueIndex + 1) * SingleLineHeight * StandardSpacing;
            
            return !property.isExpanded ? 
                SingleLineHeight : SingleLineHeight + height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property)) {

                int indent = 0;
                if (EditorGUIUtility.hierarchyMode) {
                    indent = (EditorStyles.foldout.padding.left - EditorStyles.label.padding.left) - 4;
                    position.xMin += indent;
                }

                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth -= indent;
                
                Rect foldoutRect = new Rect(position) {
                    height = SingleLineHeight,
                };

                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

                if (!property.isExpanded) {
                    return;
                }
                
                using (new EditorGUI.IndentLevelScope()) {

                    if (type.enumValueIndex >= 0) {
                        Rect xRect = new Rect(position) {
                            y = position.y + SingleLineHeight + StandardSpacing,
                            height = EditorGUI.GetPropertyHeight(xCurve)
                        };
                        EditorGUI.PropertyField(xRect, xCurve, 
                            new GUIContent(type.enumValueIndex == 0 ? "Curve" : "X"));

                        if (type.enumValueIndex > 0) {
                            Rect yRect = new Rect(position) {
                                y = xRect.y + SingleLineHeight + StandardSpacing,
                                height = EditorGUI.GetPropertyHeight(yCurve)
                            };
                            EditorGUI.PropertyField(yRect, yCurve, new GUIContent("Y"));

                            if (type.enumValueIndex > 1) {
                                Rect zRect = new Rect(position) {
                                    y = yRect.y + SingleLineHeight + StandardSpacing,
                                    height = EditorGUI.GetPropertyHeight(zCurve)
                                };
                                EditorGUI.PropertyField(zRect, zCurve, new GUIContent("Z"));
                            }
                        }
                    }
    
                }

                EditorGUIUtility.labelWidth = labelWidth;
            }
        }
    }*/
}