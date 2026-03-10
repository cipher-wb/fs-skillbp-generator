using UnityEngine;
using System;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    public partial class MapEventFormulaConfigStackNode : ConfigStackNode<MapEventFormulaConfig>
    {
        public MapEventFormulaConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }
    }
}