using GraphProcessor;
using NodeEditor.PortType;
using NodeGraphProcessor.Examples;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TableDR;
using UnityEngine;
using Funny.Base.Utils;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class NpcEventLinkConfigNode
    {
        #region 模板列表

        /// <summary>
        /// 模板列表
        /// </summary>
        [HideIf("@true")]
        public List<int> NpcTemplateIDS = new List<int>();

        /// <summary>
        /// 指定的模板NPC
        /// </summary>
        [FoldoutGroup("模板NPC配置", true)]
        [LabelText("指定的模板NPC"), ShowInInspectorAttribute, HideReferenceObjectPicker]
        [OnValueChanged("OnTableSelectDataChanged", true)]
        [ListDrawerSettings(CustomAddFunction = "OnTableSelectDataAdd")]
        private List<TableSelectData> tableSelectDataList = new List<TableSelectData>();

        public void OnTableSelectDataChanged()
        {
            NpcTemplateIDS.Clear();
            foreach (var tableData in tableSelectDataList)
            {
                if (tableData.ID != 0)
                {
                    NpcTemplateIDS.Add(tableData.ID);
                }
            }
        }

        public TableSelectData OnTableSelectDataAdd()
        {       
            return new TableSelectData(typeof(NpcTemplateRuleConfig).FullName, 0);
        }

        #endregion

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][关联配角]");
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            //自动包含死亡
            if (Config.AutoFinishTypes == default)
            {
                SetConfigValue(nameof(Config.AutoFinishTypes), new List<TActionAutoFinish>()
                {
                    TActionAutoFinish.TActionAutoFinish_Dead
                });
            }
            else if (!Config.AutoFinishTypes.Contains(TActionAutoFinish.TActionAutoFinish_Dead))
            {
                Config.AutoFinishTypes.GetListRef().Add(TActionAutoFinish.TActionAutoFinish_Dead);
            }

            //恢复tableSelectDataList
            tableSelectDataList?.Clear();
            NpcTemplateIDS?.ForEach(npcTemplateId =>
            {
                var tableData = new TableSelectData(typeof(NpcTemplateRuleConfig).FullName, npcTemplateId);
                tableData.OnSelectedID();
                tableSelectDataList.Add(tableData);
            });

            int npcTemplateID = 0;
            //默认获取手配的模板
            if (NpcTemplateIDS?.Count > 0)
            {
                npcTemplateID = NpcTemplateIDS[0];
            }
            //自动获取动态模板
            else
            {
                List<BaseNode> childNodes = new List<BaseNode>();
                GetChildNodesRecursive<ConfigPortType_MapEventConditionGroupConfig, NpcTemplateRuleConfigNode>(childNodes);
                if (childNodes?.Count > 0)
                {
                    var npcTemplateNode = childNodes.FirstOrDefault() as NpcTemplateRuleConfigNode;
                    if (npcTemplateNode != default)
                    {
                        npcTemplateID = npcTemplateNode.ID;
                    }
                }
            }

            SetConfigValue(nameof(Config.NpcTemplateID), npcTemplateID);
        }

        /// <summary>
        /// 过滤指定节点
        /// </summary>
        /// <returns></returns>
        public override bool SpecificNodeFiltering(PortView outputPort, string portTitle, Type portType)
        {
            if (portType == typeof(RefConfigBaseNode))
            {
                return true;
            }
            //全局行为
            if (outputPort.portData.identifier == "GlobalEventAction")
            {
                if (!portTitle.Contains("全局行为/"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
