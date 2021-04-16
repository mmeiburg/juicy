using UnityEditor;
using UnityEngine;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(HeaderAttribute))]
    public sealed class HeaderDecoratorDrawer : DecoratorDrawer
    {
        private HeaderAttribute Attribute => (HeaderAttribute) attribute;
        
        public override float GetHeight()
        {
            return EditorGUIUtility.singleLineHeight + 6;
        }

        public override bool CanCacheInspectorGUI()
        {
            return true;
        }

        public override void OnGUI(Rect position)
        {
            EditorGUI.LabelField(new Rect(position) {
                height = EditorGUIUtility.singleLineHeight
            }, Attribute.Title, JuicyStyles.HeaderStyle);
            
            JuicyEditorUtils.DrawLine(position, EditorGUIUtility.singleLineHeight);
        }
    }
}