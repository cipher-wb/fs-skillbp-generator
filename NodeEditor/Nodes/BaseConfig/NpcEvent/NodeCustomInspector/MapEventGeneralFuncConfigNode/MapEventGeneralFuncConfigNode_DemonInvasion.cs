using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_DemonInvasion : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_DemonInvasion(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if (!baseNode.IsLastNode())
            {
                baseNode.InspectorError += $"【必须是列表的最后一个】\n";
            }
        }

        public void ConfigToData()
        {

        }

        public void SetDefault()
        {

        }
    }
}
