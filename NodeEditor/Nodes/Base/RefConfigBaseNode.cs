using System;
using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using Sirenix.OdinInspector;
using TableDR;
using GameApp.Editor;
using UnityEngine;
using System.Reflection;
using NodeEditor.PortType;

namespace NodeEditor
{
    // TODO 扩展
    [Serializable, NodeMenuItem("表格引用节点")]
    public class RefConfigBaseNode : BaseNode, IConfigBaseNode, IRefConfigBaseNode, IParamsNode
    {
        private const string invalidMessage = "【注意】Excel未找到对应ID";

        #region Override
        public virtual int RefConfigID => GetConfigID();
        public virtual string RefConfigName => GetConfigName();
        public override bool needsInspector => true;
        public override string name => "表格引用";
        public override bool CanExport() => false;
        protected override bool CanResetPort(NodePort port) => (port.fieldName == nameof(ID)) ? false : base.CanResetPort(port);
        protected override void Enable()
        {
            onAfterEdgeConnected += OnAfterEdgeConnected;
            onAfterEdgeDisconnected += OnAfterEdgeDisconnected;
            base.Enable();
        }
        protected override void Disable()
        {
            base.Disable();

            onAfterEdgeConnected -= OnAfterEdgeConnected;
            onAfterEdgeDisconnected -= OnAfterEdgeDisconnected;
        }

        public override string GetNodeAnnotation()
        {
            string nodeAnno = null;
            try
            {
                nodeAnno = base.GetNodeAnnotation();
                if (GetConfig() == null)
                {
                    return nodeAnno;
                }
                else
                {
                    var name = ConfigIDManager.Inst.GetCreatorName(RefConfigName, RefConfigID);
                    return $"【 节点负责人: {name} 】\n\n{nodeAnno}";
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"GetNodeAnnotation error, {nodeAnno}, ex:{ex}");
                return nodeAnno;
            }
        }
        #endregion

        // TODO 优化记录，改为表格名，如：SkillConfig
        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, LabelText("引用表格类型"), ValueDropdown("GetTableManagerNames")]
        [OnValueChanged("OnDataChanged"), DelayedProperty]
        public string TableManagerName;

        /// <summary>
        /// 统一表格节点输出
        /// </summary>
        [Input(allowMultiple = false), LabelText("ID")]
        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, ValueDropdown("GetTableManagerIDS"), OnValueChanged("OnDataChanged"), DelayedProperty]
        //[InlineButton("OnDataChanged", "显示表格数据")]
        public int ID;

        [LabelText("手动输入ID")]
        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, OnValueChanged("OnDataChangedManualID"), DelayedProperty]
        public int ManualID;

        [LabelText("节点描述"), UnityEngine.Multiline(), EnableIf("@false"), NonSerialized, Sirenix.OdinInspector.ShowInInspector, VisibleIf("IsVisibleDesc", true), ShowNodeView]
        public string Desc;

        [LabelText("是否显示描述"), GraphProcessor.ShowInInspector(false), NonSerialized]
        public bool IsVisibleDesc = true;

        private bool Invalid { get { return Config == null; } }
        [Sirenix.OdinInspector.ShowInInspector, HideLabel, BoxGroup("表格数据"), HideReferenceObjectPicker, EnableIf("@false"), GraphProcessor.ShowInInspector(false)]
        [InfoBox(invalidMessage, InfoMessageType.Error, "Invalid")]
        public object Config;

        // 引用的节点
        private BaseNode refNode;

        private string TableFullName { get { return TableManagerName?.Replace(Constants.TableManagerSuffix, "") ?? string.Empty; } }
        private string TableName
        {
            get
            {
                var split = TableFullName.Split('.');
                return split.Length == 2 ? split[1] : string.Empty;
            }
        }

        public override void OnNodeCreated()
        {
            base.OnNodeCreated();

            OnDataChanged();
        }

        [CustomPortInput(nameof(ID), typeof(int), true)]
        public void GetInputs(List<SerializableEdge> edges)
        {
            // 不限制ID，可能存在Json或者节点引用
            if (edges.Count == 1/* && Config != null*/)
            {
                edges.First().passThroughBuffer = ID;
            }
        }

        protected PortData PortData_ID = new PortData();
        [CustomPortBehavior(nameof(ID))]
        public IEnumerable<PortData> InputPortBehavior_ID(List<SerializableEdge> edges)
        {
            var displayType = typeof(IPortType);
            var configType = displayType;
            if (!string.IsNullOrEmpty(TableManagerName))
            {
                configType = TableHelper.GetTableType(TableFullName);
                displayType = TablePortTypesHelper.GetTypeInput(configType);
            }
            PortData_ID.displayName = $"ID:{ID}";
            PortData_ID.displayType = displayType;
            PortData_ID.identifier = "0";
            PortData_ID.acceptMultipleEdges = false;
            PortData_ID.portColor = TableAnnotation.Inst.GetNodeColor(configType);
            yield return PortData_ID;
        }

        private IEnumerable<ValueDropdownItem> GetTableManagerNames()
        {
            // TODO 改成记录ConfigName
            return AppEditorFacade.DesignTable?.TableName2Manager.Select(kv => new ValueDropdownItem(kv.Key, kv.Value.GetType().FullName)) ?? null;
        }

        /// <summary>
        /// 获取ConfigID
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> GetTableManagerIDS()
        {
            var tableManager = DesignTable.GetTableManager(TableName);

            if (tableManager is SkillEffectConfigManager)
            {
                yield return new ValueDropdownItem("表格数据太多，不支持下拉", ManualID);
                yield break;
            }
            else
            {
                yield return new ValueDropdownItem("0", 0);
            }

            if (tableManager != null)
            {
                foreach (var item in tableManager.GetItems())
                {
                    yield return GetDropdownItemID(tableManager, item);
                }
            }
            else
            {
                Log.Warning($"GetTableManagerIDS error, can not find TableManager: {TableName}");
            }
        }

        /// <summary>
        /// 创建节点 传入表格名)
        /// </summary>
        /// <param name="portTypeName">如：ConfigPortType_AITaskNodeConfig或者AITaskNodeConfig</param>
        public void CreateNodeByTableName(string portTypeName, int id = 0)
        {
            var splitName = portTypeName.Split('_');
            var configName = splitName.Length > 1 ? splitName[1] : portTypeName;
            TableManagerName = TableHelper.ToTableManager(configName);
            ID = id;
            ManualID = id;
            OnDataChanged();
        }

        /// <summary>
        /// ID变化后，直接显示对应表格数据
        /// </summary>
        private void OnDataChanged()
        {
            Desc = string.Empty;
            if (!string.IsNullOrEmpty(TableManagerName))
            {
                // 从所有graph内找
                //Config = ConfigDataUtils.GetSingleEditorConfigNodeData(TableName, ID);

                refNode = null;
                Config = null;
                // 从所属graph里找是否存在
                if (graph is ConfigGraph configGraph)
                {
                    var node = configGraph.GetNodeByConfigNameAndID(TableName, ID);
                    if (node is IConfigBaseNode configNode)
                    {
                        var tmpConfig = configNode.GetConfig();
                        if (tmpConfig != null)
                        {
                            Config = tmpConfig;
                            refNode = node;
                        }
                        if (node is ConfigBaseNode configBaseNode)
                        {
                            Desc = configBaseNode.NodeViewDesc;
                        }
                    }
                }

                // 从打开的窗口里找
                if (Config == null)
                {
                    var wins = ConfigGraphWindow.CacheOpenedWindows;
                    foreach (var win in wins)
                    {
                        if (win.configGraph != null)
                        {
                            var node = win.configGraph.GetNodeByConfigNameAndID(TableName, ID);
                            if (node is IConfigBaseNode configNode)
                            {
                                var tmpConfig = configNode.GetConfig();
                                if (tmpConfig != null)
                                {
                                    Config = tmpConfig;
                                    refNode = node;
                                }
                                if (node is ConfigBaseNode configBaseNode)
                                {
                                    Desc = configBaseNode.NodeViewDesc;
                                }
                                break;
                            }
                        }
                    }
                }

                // 从记录的文件信息里找
                if (Config == null)
                {
                    if (JsonGraphManager.Inst.TryGetNodeInfo(TableName, ID, out var nodeInfo))
                    {
                        Config = nodeInfo.Config;
                        Desc = ConfigBaseNode.GetNodeViewDesc(Config, nodeInfo.Desc);
                    }
                }

                // 从表格数据找
                if (Config == null)
                {
                    Config = DesignTable.GetTableCell(TableName, ID);
                }
            }
            Desc ??= string.Empty;
            IsVisibleDesc = !string.IsNullOrEmpty(Desc);

            UpdateCustomName();
            SyncPortDatas();
        }

        private void UpdateCustomName()
        {
            var typeName = string.Empty;
            switch (Config)
            {
                case SkillEffectConfig skillEffectConfig:
                    typeName = skillEffectConfig.SkillEffectType.GetDescription(false);
                    break;
                case SkillConditionConfig skillConditionConfig:
                    typeName = skillConditionConfig.SkillConditionType.GetDescription(false);
                    break;
                case SkillSelectConfig skillSelectConfig:
                    typeName = skillSelectConfig.SkillSelectType.GetDescription(false);
                    break;
            }
            Desc?.Insert(0, $"{typeName}\n");
            var customName = $"{name}-{typeName} [{TableName ?? string.Empty}:{ID}]";
            SetCustomName(customName);
        }

        /// <summary>
        /// 手动输入ID变化
        /// </summary>
        private void OnDataChangedManualID()
        {
            ID = ManualID;

            OnDataChanged();
        }

        private void OnAfterEdgeConnected(SerializableEdge edge)
        {
            // 更新引用节点
            // TODO

            // 刷新映射数据
            edge.SyncDatas();
        }
        private void OnAfterEdgeDisconnected(SerializableEdge edge)
        {
            // 刷新映射数据
            edge.SyncDatas();
        }

        #region Interface
        public bool OnPostProcessing()
        {
            OnDataChanged();
            return true;
        }
        public string GetConfigName() => TableName;
        public int GetConfigID() => ID;
        public int GetID() => ID;
        public object GetConfig() => Config;
        public string GetTableTash() { return string.Empty; }
        public string GetConfigJson() { return string.Empty; }
        public int GenerateID(bool reset) { return 0; }
        public string GetNodeSearcheName() => $"{name}:{TableName}:{ID}";
        public virtual bool OnSaveCheck() => true;

        #endregion


        /// <summary>
        /// 获取引用节点，如果有的话
        /// </summary>
        public override BaseNode GetReferenceNode()
        {
            if(graph == null || graph.nodes?.Count <= 0) { return default; }

            foreach(var node in graph.nodes)
            {
                if (!(node is RefConfigBaseNode) && node is IConfigBaseNode baseNode)
                {
                    if (this.GetConfigName() == baseNode.GetConfigName() && this.GetID() == baseNode.GetID())
                    {
                        return node;
                    }
                }
            }

            return default;
        }

        /// <summary>
        /// 根据不同的表格类型，设置下拉列表
        /// </summary>
        /// <param name="tableManager"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private ValueDropdownItem GetDropdownItemID(ITableManager tableManager, dynamic config)
        {
            //可定制
            return new ValueDropdownItem(config.ID.ToString(), config.ID);
        }

        #region Interface IParamsNode
        public void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            // do nothing
        }

        public ParamsAnnotation GetParamsAnnotation()
        {
            if (refNode is IParamsNode refParamsNode)
            {
                return refParamsNode.GetParamsAnnotation();
            }
            return null;
        }

        public string GetParamsName()
        {
            return string.Empty;
        }

        public IReadOnlyList<TParam> GetParamsList()
        {
            return null;
        }

        public void RefreshParamsDisplayName()
        {
            // do nothing
        }
        #endregion
    }
}
