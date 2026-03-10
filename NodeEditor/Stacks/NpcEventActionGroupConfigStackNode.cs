using UnityEngine;
using System;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    public partial class NpcEventActionGroupConfigStackNode : ConfigStackNode<NpcEventActionGroupConfig>
    {
        public NpcEventActionGroupConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }
    }
}