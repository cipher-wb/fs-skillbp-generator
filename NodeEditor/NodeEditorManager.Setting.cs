using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;

namespace NodeEditor
{
    /// <summary>
    /// 节点编辑器相关window类型
    /// </summary>
    public interface INodeEditorWindow { }

    /// <summary>
    /// 节点编辑器配置信息
    /// </summary>
    public partial class NodeEditorManager
    {
        public const string name = "【节点编辑器配置】";
        public const string PathPrefix = "Assets/Thirds/NodeEditor/";

        // TODO 处理节点编辑器设置相关

        #region 编辑器操作/查找模板引用
        [InfoBox("注意：" +
            "\n  1-以下操作针对所有编辑器" +
            "\n  2-其他", InfoMessageType = InfoMessageType.Warning)]
        [TitleGroup("编辑器操作", order: -2, indent: true), FoldoutGroup("编辑器操作/查找模板引用", expanded: true)]
        [LabelText("查找文件选择："), ShowInInspector, PropertyOrder(-1), ValueDropdown("GetSearchPathRefs", DoubleClickToConfirm = true, ExpandAllMenuItems = false)]
        [InlineButton("DoSearchPathRef", "点击查找")]
        [InlineButton("DoRefreshSearchPathRef", "点击刷新")]
        private string searchPathRef;

        [FoldoutGroup("编辑器操作/查找模板引用")]
        [HideLabel, PropertyOrder(-1)]
        public GraphDataSearchedList graphDataSearchList = new GraphDataSearchedList();

        private List<ValueDropdownItem> cacheDropdownItems = new List<ValueDropdownItem>();

        public string SearchPathRef
        {
            get => searchPathRef;
            set => searchPathRef = value;
        }


        public void DoRefreshSearchPathRef()
        {
            cacheDropdownItems.Clear();
            var graphFiles = GraphHelper.GetGraphFiles();
            foreach (var graphFile in graphFiles)
            {
                var path = Utils.PathFull2SeparationFolder(graphFile, PathPrefix);
                cacheDropdownItems.Add(new ValueDropdownItem(path, graphFile));
            }
        }
        private IEnumerable<ValueDropdownItem> GetSearchPathRefs()
        {
            if (cacheDropdownItems.Count == 0)
            {
                DoRefreshSearchPathRef();
            }
            foreach (var item in cacheDropdownItems)
            {
                yield return item;
            }
        }

        //private static readonly Regex templatePathRegex = new Regex("\"TemplatePath\": \"(\\S+)\"");
        public void DoSearchPathRef()
        {
            graphDataSearchList.Clear();
            if (!string.IsNullOrEmpty(searchPathRef) && cacheDropdownItems?.Count > 0)
            {
                foreach (var item in cacheDropdownItems)
                {
                    var path = item.Value as string;
                    var fileContent = File.ReadAllText(path);
                    if (fileContent.Contains($"\"TemplatePath\": \"{searchPathRef}\""))
                    {
                        graphDataSearchList.Add(path);
                    }
                }
            }

            graphDataSearchList.Path = searchPathRef;
        }
        #endregion

        #region 编辑器操作/表格操作
        [LabelText("指定表格（如：SkillConfig|BuffConfig）"), FoldoutGroup("编辑器操作/批处理文件操作", expanded: true)]
        public string ConfigNames = string.Empty;

        [LabelText("批处理选项"), FoldoutGroup("编辑器操作/批处理文件操作", expanded: true), HorizontalGroup("编辑器操作/批处理文件操作/1")]
        public BatchFlags GraphBatchFlags = BatchFlags.导出Excel;

        [Button("点击处理"), FoldoutGroup("编辑器操作/批处理文件操作"), HorizontalGroup("编辑器操作/批处理文件操作/1")]
        public void GraphBatchProcessing()
        {
            if (!EditorUtility.DisplayDialog("批处理数据", "是否执行？", "是√", "否×"))
            {
                return;
            }
            var configNames = string.IsNullOrEmpty(ConfigNames) ? null : ConfigNames.Split("|").ToList();
            Utils.DisplayProcess(name, (sbInfo) =>
            {
                GraphHelper.ProcessEditor((manager) =>
                {
                    if (manager is ConfigEditorManager editorManager)
                    {
                        editorManager.GraphBatchProcessing(GraphBatchFlags, EditorFlag.DisplayProcess, configNames);
                    }
                });
            });
        }
        #endregion
    }
}
