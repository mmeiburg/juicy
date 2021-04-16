using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace TinyTools.Juicy
{
    internal static class FeedbackListCopyHelper
    {
        private static readonly List<FeedbackCopyData> CopiedFeedbacks =
            new List<FeedbackCopyData>();

        private sealed class FeedbackCopyData
        {
            public Type type;
            public readonly List<SerializedProperty> properties =
                new List<SerializedProperty>();
        }

        public static bool HasCopy => CopiedFeedbacks.Count > 0;

        public static void Copy(SerializedProperty feedbackList)
        {
            if (feedbackList == null) {
                return;
            }
            
            CopiedFeedbacks.Clear();
            
            for (int i = 0; i < feedbackList.arraySize; i++) {
                SerializedProperty prop = feedbackList.GetArrayElementAtIndex(i);

                FeedbackCopyData data = new FeedbackCopyData {
                    type = prop.objectReferenceValue.GetType(),
                };
                
                SerializedProperty property = new SerializedObject(prop
                    .objectReferenceValue).GetIterator();
                property.Next(true);
                
                do {
                    if (!FeedbackCopyHelper.IgnoreList.Contains(property.name)) {
                        data.properties.Add(property.Copy());
                    }
                } while (property.Next(false));
                
                CopiedFeedbacks.Add(data);
            }
        }

        public static void Paste(SerializedProperty feedbacks, Action removeAll,
            Func<Type, JuicyFeedbackBase> addAction)
        {
            if (feedbacks == null) {
                return;
            }
            
            removeAll.Invoke();
            
            foreach (FeedbackCopyData data in CopiedFeedbacks) {
                addAction.Invoke(data.type);
            }

            for (int i = 0; i < feedbacks.arraySize; i++) {
                
                SerializedProperty feedback = feedbacks.GetArrayElementAtIndex(i);
                JuicyFeedbackBase f = feedback.objectReferenceValue as JuicyFeedbackBase;
                FeedbackCopyData data = CopiedFeedbacks[i];
                
                JuicyEditorUtils.PasteFeedback(feedback, f);
                
                foreach (var property in data.properties) {
                    SerializedObject obj = new SerializedObject(f);
                    obj.CopyFromSerializedProperty(property);
                    obj.ApplyModifiedProperties();
                }
            }
            
            CopiedFeedbacks.Clear();
        }
    }

    internal static class FeedbackCopyHelper
    {
        internal static Type Type { get; private set; }
        private static readonly List<SerializedProperty> Properties =
            new List<SerializedProperty>();
        
        private static JuicyFeedbackBase copyReference;

        public static readonly string[] IgnoreList =
        {
            "m_ObjectHideFlags",
            "m_CorrespondingSourceObject",
            "m_PrefabInstance",
            "m_PrefabAsset",
            "m_GameObject",
            "m_Enabled",
            "m_EditorHideFlags",
            "m_Script",
            "m_Name",
            "m_EditorClassIdentifier"
        };

        public static void RemoveCopyReference(
            JuicyFeedbackBase feedback,
            List<JuicyFeedbackBase> feedbackList)
        {
            if(feedback.referenceCount > 0)
                feedback.referenceCount--;
            
            feedbackList.Remove(feedback);
        }

        public static void CopyReference(JuicyFeedbackBase feedback)
        {
            copyReference = feedback;
        }

        public static void PasteReference(List<JuicyFeedbackBase> list)
        {
            Properties.Clear();
            list.Add(copyReference);
            copyReference.referenceCount++;
            copyReference = null;
        }

        public static void Copy(SerializedObject serializedObject)
        {
            copyReference = (JuicyFeedbackBase) serializedObject.targetObject;
            
            Type = serializedObject.targetObject.GetType();
            Properties.Clear();

            SerializedProperty property = serializedObject.GetIterator();
            property.Next(true);
                
            do {
                CacheCopy(property);
            } while (property.Next(false));
        }

        private static void CacheCopy(SerializedProperty property)
        {
            if (!IgnoreList.Contains(property.name)) {
                Properties.Add(property.Copy());
            }
        }

        public static void Paste(SerializedObject target)
        {
            if (target.targetObject.GetType() != Type) {
                return;
            }

            foreach (var property in Properties) {
                target.CopyFromSerializedProperty(property);
            }
        }

        public static void ClearCache()
        {
            Properties.Clear();
        }

        public static bool HasCopy => Properties != null && Properties.Count > 0;
    }
}