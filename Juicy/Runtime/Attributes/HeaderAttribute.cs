using UnityEngine;

namespace TinyTools.Juicy
{
    public class HeaderAttribute : PropertyAttribute
    {
        public string Title { get; }

        public HeaderAttribute(string title)
        {
            this.Title = title;
        }
    }
}