using UnityEngine;
using System;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    public partial class MapEventConditionConfigStackNode : ConfigStackNode<MapEventConditionConfig>
    {
        public MapEventConditionConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }
    }
}