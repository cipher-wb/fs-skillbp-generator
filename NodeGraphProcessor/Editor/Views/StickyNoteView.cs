#if UNITY_2020_1_OR_NEWER
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

namespace GraphProcessor
{
    public class StickyNoteView : UnityEditor.Experimental.GraphView.StickyNote
	{
		public BaseGraphView	owner;
		public StickyNote		note;

        Label                   titleLabel;
        ColorField              colorField;

        [HideInInspector]
        public bool initializing = false; //Used for applying SetPosition on locked node at init.

        public StickyNoteView()
        {
            fontSize = StickyNoteFontSize.Medium;
            theme = StickyNoteTheme.Classic;
		}

		public void Initialize(BaseGraphView graphView, StickyNote note)
		{
			this.note = note;
			owner = graphView;

            this.Q<TextField>("title-field").RegisterCallback<ChangeEvent<string>>(e => {
                note.title = e.newValue;
            });
            this.Q<TextField>("contents-field").RegisterCallback<ChangeEvent<string>>(e => {
                note.content = e.newValue;
            });
        
            title = note.title;
            contents = note.content;
            initializing = true;

            SetPosition(note.position);
		}

		public override void SetPosition(Rect newPos)
        {
            if (!initializing && owner != null && !owner.initializing && !owner.IsElementMovingByGroup(note.GUID))
            {
                owner.RegisterCompleteObjectUndo("Moved graph note" + newPos.ToString());
                var oPos = $"{note.GUID}:{GetPosition().position}";
                var nPos = $"{note.GUID}:{newPos.position}";
                owner.AddUnReDoInfo(DateTime.Now.ToString(), BaseGraphView.URDControlType.URDCT_StickyNote_Move, oPos, nPos);
            }
            base.SetPosition(newPos);

            if (note != null)
                note.position = newPos;
        }

        public override void OnResized()
        {
            note.position = layout;
        }
        public override bool IsAscendable() => true;

        public override bool IsCopiable() => true;

        public override bool IsDroppable() => true;

        public override bool IsGroupable() => true;
    }
}
#endif