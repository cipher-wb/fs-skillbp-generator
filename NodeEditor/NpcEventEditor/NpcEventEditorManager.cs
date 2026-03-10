using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TableDR;
using UnityEditor;

namespace NodeEditor.NpcEventEditor
{
    public partial class NpcEventEditorManager : ConfigEditorManager<NpcEventEditorManager, NpcEventEditorSetting, NpcEventGraph, NpcEventGraphWindow>
    {
        private string firstNodeName = "NpcEventConfigNode";

        private HashSet<string> firstNodeConfigName = new HashSet<string>()
        {
            "NpcEventConfig",
            "NpcEventActionConfig",
            "MapEventConditionGroupConfig",
            "MapEventPerformanceGroupConfig",
        };

        public override string Version => Constants.NpcEventEditor.Version;

        public override string Name => "Npc事件编辑器";

        public override string Path => "Assets/Thirds/NodeEditor/NpcEventEditor";

        /// <summary>
        /// 是否显示首个节点ID
        /// </summary>
        protected override bool IsShowFirstNodeID => false;

        /// <summary>
        /// 是否显示拷贝列表
        /// </summary>
        protected override bool IsShowGraphCopy => true;

        /// <summary>
        /// 创建节点信息
        /// </summary>
        protected override string GraphCreateInfo
        {
            get
            {
                var info = string.IsNullOrEmpty(copyGraphName) ? "当前操作【新建】" : $"当前操作【拷贝】【{copyGraphName}】";
                if (string.IsNullOrEmpty(graphCreatedDesc))
                {
                    info += "\n请配置Graph描述！！";
                }

                return info;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            SetDefaultFirstNode();
        }

        /// <summary>
        /// 设置默认的首个节点
        /// </summary>
        protected override void SetDefaultFirstNode()
        {
            graphCreatedFirstNodeName = firstNodeName;
        }

        /// <summary>
        /// 获取下拉节点的名字
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ValueDropdownItem> GetNodeValidNames()
        {
            foreach (var anno in TableAnnotation.Inst.GetNodeBaseAnnotations())
            {
                if (firstNodeConfigName.Contains(anno.ConfigName))
                {
                    yield return new ValueDropdownItem($"{anno.ConfigName}", anno.NodeName);
                }
            }
        }

        [TitleGroup("检查操作", indent: true)]
        [TitleGroup("检查操作/事件ID与文件名不符", Order = 1)]
        [Button("开始检查")]
        private void CheckNameID()
        {
            SearchedList.Clear();

            var graghFiles = Directory.GetFiles(PathSavesJsons, "*.json", SearchOption.AllDirectories);
            for (int i = 0; i < graghFiles.Length; i++)
            {
                var graghFile = graghFiles[i];
                Utils.PathFormat(ref graghFile);

                GraphHelper.ProcessGraph(graghFile, (graph) =>
                {
                    foreach (var node in graph.nodes)
                    {
                        if (node is NpcEventConfigNode eventConfigNode)
                        {
                            var eventID = eventConfigNode.ID.ToString();
                            if (!graghFile.Contains(eventID))
                            {
                                SearchedList.Add(graghFile, "", 0);
                                break;
                            }
                        }
                    }
                });
            }
        }

        [ShowInInspector, HideLabel, HideReferenceObjectPicker]
        [TitleGroup("检查操作/事件ID与文件名不符", Order = 2)]
        public GraphDataSearchedList SearchedList = new GraphDataSearchedList();

        [TitleGroup("检查操作", indent: true)]
        [TitleGroup("检查操作/检查所有节点配置问题", Order = 1)]
        [Button("开始检查")]
        private void CheckError()
        {
            var graghFiles = Directory.GetFiles(PathSavesJsons, "*.json", SearchOption.AllDirectories);
            for (int i = 0; i < graghFiles.Length; i++)
            {
                var graghFile = graghFiles[i];
                Utils.PathFormat(ref graghFile);

                GraphHelper.ProcessGraph(graghFile, (graph) =>
                {
                    graph.SaveGraphToDisk();
                });
            }

            EditorUtility.DisplayDialog("提示", "配置错误项已导出至Console页签", "OK");
        }
    }
}
