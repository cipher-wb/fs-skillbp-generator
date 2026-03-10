using Newtonsoft.Json;
using NodeEditor.AIEditor;
using NodeEditor.GamePlayEditor;
using NodeEditor.SkillEditor;
using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TableDR;

namespace NodeEditor
{
    [Serializable, HideReferenceObjectPicker/*, ShowIf("@($property.ValueEntry.WeakSmartValue as NodeBaseAnnotation).Visible", true)*/]
    public class NodeBaseAnnotation
    {
        private bool Invalid => GraphDatas.Count == 0;
        private string message;

        [HideIf("@true")]
        /// <summary>
        /// 是否是派生表格节点
        /// </summary>
        public virtual bool IsConfigDerive() { throw new NotImplementedException(); }

        /// <summary>
        /// 枚举中的某一个
        /// </summary>
        [HideIf("@true")]
        public string NodeName;

        [HideIf("@true")]
        public string ConfigName;

        /// <summary>
        /// Assets/Thirds/NodeEditor/AutoGenerate/Nodes/
        /// </summary>
        [HideIf("@true"), JsonIgnore]
        public string PathDirNodeScript;

        [HideIf("@true"), JsonIgnore]
        public string PathNodeScript { get { return $"{PathDirNodeScript}/{NodeName}.cs"; } }

        [FoldoutGroup("$FoldoutGroupName"), LabelText("节点所属配置")]
        [InfoBox("请配置节点所属编辑器，否则节点不显示", InfoMessageType.Error, "Invalid"), HideReferenceObjectPicker]
        [ListDrawerSettings(ShowFoldout = true)]
        public List<GraphData> GraphDatas = new List<GraphData>();

        [FoldoutGroup("$FoldoutGroupName"), LabelText("节点分类")]
        public NodeTypeFlags NodeTypeFlags = NodeTypeFlags.None;

        private List<GraphData> graphDatasSaving;
        public List<GraphData> GetGraphDatasSaving()
        {
            if (graphDatasSaving != null)
            {
                return graphDatasSaving;
            }
            // 鉴于玩法编辑器节点包含技能编辑器节点，这里二次处理数据
            graphDatasSaving = new List<GraphData>();
            graphDatasSaving.AddRange(GraphDatas);
            foreach (var graphData in GraphDatas)
            {
                // 处理技能编辑器暴露节点，添加玩法编辑器/AI编辑器节点暴露
                var grapghType = graphData?.GetGraphType();
                if (grapghType != null && grapghType.Name == nameof(SkillGraph))
                {
                    var graphDataNew = new GraphData
                    {
                        CompatibleGraphName = nameof(GamePlayGraph),
                        ModuleName = graphData.GetModuleName(),
                        ModuleNamePerfix = SkillEditorManager.Inst.Name
                    };
                    graphDatasSaving.Add(graphDataNew);

                    graphDataNew = new GraphData
                    {
                        CompatibleGraphName = nameof(AIGraph),
                        ModuleName = graphData.GetModuleName(),
                        ModuleNamePerfix = SkillEditorManager.Inst.Name
                    };
                    graphDatasSaving.Add(graphDataNew);
                }
            }
            return graphDatasSaving;
        }

        public bool IsGraphNeededConfig(Type graphType)
        {
            foreach (var graphData in GetGraphDatasSaving())
            {
                if (graphData.CompatibleGraphName == graphType?.Name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsCompatibleWithGraph(Type graphType)
        {
            foreach (var graphData in GetGraphDatasSaving())
            {
                if (graphData.CompatibleGraphName == graphType?.Name && !IsIgnoreConfig())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsIgnoreConfig()
        {
            if (!IsConfigDerive() && (
                ConfigName == nameof(SkillEffectConfig) ||
                ConfigName == nameof(SkillSelectConfig) ||
                ConfigName == nameof(SkillConditionConfig) ||
                ConfigName == nameof(AITaskNodeConfig)))
            {
                // 基础类型不导出
                return true;
            }
            return false;
        }

        [JsonIgnore]
        public bool IsShowNode { get { return GraphDatas.Count > 0; } }
        [JsonIgnore]
        public virtual string FoldoutGroupName
        {
            get
            {
                var foldoutGroupName = string.Empty;
                foreach (var graphData in GraphDatas)
                {
                    foldoutGroupName += $"  [{graphData.GetEditorName()}]";
                }
                return foldoutGroupName;
            }
        }
        [JsonIgnore]
        public virtual string Name { get { throw new NotImplementedException(); } }
        public virtual void Load() { }
        public virtual void UnLoad() { }
        public virtual string Save(StringBuilder sbSaveInfo)
        {
            // 基础
            return string.Empty;
        }

        /// <summary>
        /// 创建虚拟节点代码
        /// </summary>
        /// <param name="sbSaveInfo"></param>
        protected virtual string CreateVirtualNodeCode(StringBuilder sbSaveInfo) { return string.Empty; }

        public static void WriteToFile(string path, string content, StringBuilder sbSaveInfo)
        {
            var fileName = Path.GetFileName(path);
            if (!File.Exists(path))
            {
                Utils.WriteAllText(path, content);
                sbSaveInfo?.AppendLine($"【新增】:{fileName}");
            }
            else
            {
                var oldContent = Utils.ReadAllText(path);
                if (oldContent != content)
                {
                    Utils.WriteAllText(path, content);
                    sbSaveInfo?.AppendLine($"【更新】:{fileName}");
                }
            }
        }
    }
}
