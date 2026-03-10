using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class CommonClickable : Clickable
    {
        public CommonClickable(Action<EventType, Vector2, VisualElement> clickEvent, Action handler = null) : base(handler)
        {
            this.clickEvent = clickEvent;
        }

        private Action<EventType, Vector2, VisualElement> clickEvent;

        protected override void ProcessDownEvent(EventBase evt, Vector2 localPosition, int pointerId)
        {
            base.ProcessDownEvent(evt, localPosition, pointerId);
            clickEvent?.Invoke(EventType.MouseDown, localPosition, target);
        }

        protected override void ProcessMoveEvent(EventBase evt, Vector2 localPosition)
        {
            base.ProcessMoveEvent(evt, localPosition);
            clickEvent?.Invoke(EventType.MouseMove, localPosition, target);
        }

        protected override void ProcessUpEvent(EventBase evt, Vector2 localPosition, int pointerId)
        {
            base.ProcessUpEvent(evt, localPosition, pointerId);
            clickEvent?.Invoke(EventType.MouseUp, localPosition, target);
        }
    }
}
