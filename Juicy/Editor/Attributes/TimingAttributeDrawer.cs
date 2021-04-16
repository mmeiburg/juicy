using UnityEditor;
using UnityEngine;
using static TinyTools.Juicy.TimingAttribute.TimingStyle;

namespace TinyTools.Juicy
{
    [CustomPropertyDrawer(typeof(TimingAttribute))]
    public sealed class TimingAttributeDrawer : JuicyPropertyDrawerBase
    {
        private readonly PropertyData durationProperty = new PropertyData(HideDuration);
        private readonly PropertyData delayProperty = new PropertyData(HideDelay);
        private readonly PropertyData cooldownProperty = new PropertyData(HideCooldown);
        private readonly PropertyData ignoreTimeScaleProperty = new PropertyData(HideIgnoreTimeScale);

        private TimingAttribute TimingAttribute => (TimingAttribute)attribute;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            height += durationProperty.GetHeight(TimingAttribute) + spacing;
            height += delayProperty.GetHeight(TimingAttribute) + spacing;
            height += cooldownProperty.GetHeight(TimingAttribute) + spacing;
            height += ignoreTimeScaleProperty.GetHeight(TimingAttribute) + spacing;

            return height + spacing;
        }

        public override bool CanCacheInspectorGUI(SerializedProperty property)
        {
            return true;
        }

        private void Initialise(SerializedProperty property)
        {
            if (durationProperty.Property == null) {
                durationProperty.SetProperty(property.FindPropertyRelative("duration")); 
            }
            
            if (delayProperty.Property == null) {
                delayProperty.SetProperty(property.FindPropertyRelative("delay")); 
            }
            
            if (cooldownProperty.Property == null) {
                cooldownProperty.SetProperty(property.FindPropertyRelative("cooldown")); 
            }
            
            if (ignoreTimeScaleProperty.Property == null) {
                ignoreTimeScaleProperty.SetProperty(property.FindPropertyRelative("ignoreTimeScale")); 
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialise(property);
            
            float yPosition = 0f;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            using (new EditorGUI.PropertyScope(position, label, property)) {

                Rect duration = new Rect {
                    x = position.x,
                    y = position.y + yPosition,
                    width = position.width,
                    height = durationProperty.GetHeight(TimingAttribute) + spacing
                };

                if (durationProperty.Draw(duration, TimingAttribute)) {
                    yPosition += duration.height;
                }
                
                Rect delay = new Rect {
                    x = position.x,
                    y = position.y + yPosition,
                    width = position.width,
                    height = delayProperty.GetHeight(TimingAttribute) + spacing
                };
            
                if (delayProperty.Draw(delay, TimingAttribute)) {
                    yPosition += delay.height;
                }
            
                Rect cooldown = new Rect {
                    x = position.x,
                    y = position.y + yPosition,
                    width = position.width,
                    height = cooldownProperty.GetHeight(TimingAttribute) + spacing
                };
            
                if (cooldownProperty.Draw(cooldown, TimingAttribute)) {
                    yPosition += cooldown.height;
                }
            
                Rect ignoreTimeScale = new Rect {
                    x = position.x,
                    y = position.y + yPosition,
                    width = position.width,
                    height = ignoreTimeScaleProperty.GetHeight(TimingAttribute) + spacing
                };

                ignoreTimeScaleProperty.Draw(ignoreTimeScale, TimingAttribute);
            }
        }
        
        private sealed class PropertyData
        {
            public SerializedProperty Property { get; private set; }
            
            private readonly TimingAttribute.TimingStyle hidingFlag;
            private float height;

            public PropertyData(TimingAttribute.TimingStyle hidingFlag)
            {
                this.hidingFlag = hidingFlag;
            }

            public void SetProperty(SerializedProperty property)
            {
                Property = property;
                height = EditorGUI.GetPropertyHeight(property);
            }

            public float GetHeight(TimingAttribute attribute)
            {
                if (Property == null || attribute.Style.HasFlag(hidingFlag)) {
                    return 0;
                }
                
                return height;
            }

            public bool Draw(Rect position, TimingAttribute attribute)
            {
                if (attribute.Style.HasFlag(hidingFlag)) {
                    return false;
                }

                EditorGUI.PropertyField(position, Property);

                Property.serializedObject.ApplyModifiedProperties();
                
                return true;
            }
        }

    }
}