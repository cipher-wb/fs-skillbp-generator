using UnityEngine;
using System;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    public partial class NpcEventActionConfigStackNode : ConfigStackNode<NpcEventActionConfig>
    {
        public NpcEventActionConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }
    }
}