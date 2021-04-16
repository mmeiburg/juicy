using System;
using System.Linq;

namespace TinyTools.Juicy
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class FeedbackAttribute : Attribute
    {
        private readonly string path;
        private readonly string name;
        private readonly string icon;

        public FeedbackAttribute(string path)
        {
            this.path = path;
            var split = path.Split('/');
            name = split.Last();

            icon = $"img_feedback_{split.First().ToLower()}";
        }

        public FeedbackAttribute(string path, string icon) : this(path)
        {
            this.icon = icon;
        }

        public static string GetIcon(Type type)
        {
            var attribute = type.GetCustomAttributes(false)
                .OfType<FeedbackAttribute>().FirstOrDefault();
            return attribute?.icon;
        }

        public static string GetName(Type type)
        {
            var attribute = type.GetCustomAttributes(false)
                .OfType<FeedbackAttribute>().FirstOrDefault();
            return attribute != null ? attribute.name : "Custom";
        }

        public static string GetPath(Type type)
        {
            var attribute = type.GetCustomAttributes(false)
                .OfType<FeedbackAttribute>().FirstOrDefault();
            return attribute != null ? attribute.path : $"Custom/{type.Name}";
        }
    }
}