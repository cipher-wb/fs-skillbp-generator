/////////////////////////////////////
// 注意！！此代码文件由工具自动生成！！ 
// 扩展方法请新建文件扩展partial类实现 
// 如:MapEventConditionConfig.Custom.cs
/////////////////////////////////////

using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// npc模板节点
    /// </summary>
    public partial class MapEventConditionConfigNode
    {
        [Output, HideIf("@true")]
        public int NpcTemplateID;

        [CustomPortBehavior(nameof(NpcTemplateID))]
        public IEnumerable<PortData> NpcTemplateID_Behavior(List<SerializableEdge> edges)
        {
            yield return new PortData
            {
                displayName = $"动态模板",
                displayType = typeof(INpcTemplateRuleConfig),
                identifier = nameof(NpcTemplateID),
                acceptMultipleEdges = false,
            };
        }

        public List<MapEventFormulaConfigNode> GetFormulaList()
        {
            var nodes = GetChildNodes<INpcTemplateRuleConfig, MapEventFormulaConfigNode>();
            return nodes;
        }
    }
}
