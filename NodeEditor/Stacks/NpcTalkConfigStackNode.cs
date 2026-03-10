using UnityEngine;
using System;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    public partial class NpcTalkConfigStackNode : ConfigStackNode<NpcTalkConfig>
    {
        public NpcTalkConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }
    }
}