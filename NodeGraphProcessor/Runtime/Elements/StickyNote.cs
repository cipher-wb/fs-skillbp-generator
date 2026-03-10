using System;
using UnityEngine;

namespace GraphProcessor
{
    /// <summary>
    /// Serializable Sticky node class
    /// </summary>
    [Serializable]
    public class StickyNote
    {
        public string GUID;
        public Rect position;
        public string title = "Title";
        public string content = "Description";

        public StickyNote() { }

        public StickyNote(string title, Vector2 position)
        {
            GUID = Guid.NewGuid().ToString();
            this.title = title;
            this.position = new Rect(position.x, position.y, 200, 300);
        }

        /// <summary>
        /// Called when the StickyNote is created
        /// </summary>
        public virtual void OnCreated() => GUID = Guid.NewGuid().ToString();
    }
}