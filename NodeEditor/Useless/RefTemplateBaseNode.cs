using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NodeEditor
{
    public abstract class RefTemplateBaseNode : BaseNode
    {
        public abstract BaseNode NodeRef { get; }
        public abstract string PathTemplate { get; }
    }
    [Serializable, Obsolete("废弃引用")]
    public abstract class RefTemplateBaseNode<T> : RefTemplateBaseNode, IConfigBaseNode where T : class
    {
        #region Define
        private static readonly string invalidMessage = "模板节点类型非" + typeof(T).Name;
        private bool Invalid { get { return GetConfigName() != ConfigName; } }
        private string invalidOpenMessage = string.Empty;
        private bool InvalidOpen { get { return invalidOpenMessage.Length != 0; } }
        private Type configType = typeof(T);
        #endregion

        #region Override
        public override bool needsInspector => true;
        public override string name => "模板引用";
        public override bool CanExport() => false;
        protected override bool CanResetPort(NodePort port) => (port.fieldName == nameof(ID)) ? false : base.CanResetPort(port);
        public override Color color
        {
            get
            {
                return TableAnnotation.Inst.GetNodeColor(configType, base.color);
            }
        }
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
        protected override void OnSave()
        {
            base.OnSave();
            SyncPortDatas();
            OnSaveCheck();
        }
        #endregion

        #region Interface
        public int GenerateID(bool reset) => 0;

        public object GetConfig()
        {
            return ConfigBaseNodeRef?.GetConfig() ?? null;
        }

        public int GetConfigID()
        {
            return ConfigBaseNodeRef?.GetConfigID() ?? 0;
        }

        public string GetConfigJson()
        {
            return ConfigBaseNodeRef?.GetConfigJson() ?? null;
        }

        public string GetConfigName()
        {
            return ConfigBaseNodeRef?.GetConfigName() ?? null;
        }

        public int GetID()
        {
            return ConfigBaseNodeRef?.GetID() ?? 0;
        }

        public override string GetNodeSearchName()
        {
            return ConfigBaseNodeRef?.GetNodeSearchName() ?? null;
        }

        public string GetTableTash()
        {
            return ConfigBaseNodeRef?.GetTableTash() ?? null;
        }

        public bool OnPostProcessing()
        {
            Refresh();
            return true;
        }
        // TODO
        public virtual bool OnSaveCheck()
        {
            if (Invalid)
            {
                graph?.AddGraphFatalInfo($"[ID:{ID}_{invalidMessage}]", this, true);
                return false;
            }
            return true;
        }

        #endregion

        #region InputPort
        [Input(allowMultiple = false), LabelText("模板-表格ID"), ReadOnly]
        public int ID;

        [LabelText("模板-表格名"), ReadOnly, GraphProcessor.ShowInInspector(false)]
        public string ConfigName = typeof(T).Name;

        [CustomPortInput(nameof(ID), typeof(int), true)]
        public void GetInputs(List<SerializableEdge> edges)
        {
            // 不限制ID，可能存在Json或者节点引用
            if (edges.Count == 1/* && Config != null*/)
            {
                edges.First().passThroughBuffer = ID;
            }
        }

        [CustomPortBehavior(nameof(ID))]
        public IEnumerable<PortData> InputPortBehavior_ID(List<SerializableEdge> edges)
        {
            var displayType = TablePortTypesHelper.GetTypeInput(configType);
            if (displayType == null) displayType = typeof(int);

            yield return new PortData
            {
                displayName = $"模板{nameof(ID)}:{ID}",
                displayType = displayType,
                identifier = "0",
                acceptMultipleEdges = false,
                portColor = TableAnnotation.Inst.GetNodeColor(configType),
            };
        }
        #endregion


        [ButtonGroup("Button", order: -2), Button("点击打开模板")]
        private void OpenGraphRef()
        {
            Refresh();
            var ret = GraphAssetCallbacks.InitializeGraph(graphRef, PathRef);
            if (!ret)
            {
                invalidOpenMessage = $"打开失败:{PathRef}";
            }
        }
        [ButtonGroup("Button", order: -2), Button("点击刷新模板")]
        private void RefreshGraphRef()
        {
            Refresh(true);
        }

        [LabelText("模板引用路径"), ValueDropdown("GetPathRef", DoubleClickToConfirm = true), OnValueChanged("OnValueChanged_PathRef"), GraphProcessor.ShowInInspector(false), PropertyOrder(-1)]
        [InfoBox("$invalidMessage", "Invalid", InfoMessageType = InfoMessageType.Error)]
        [InfoBox("$invalidOpenMessage", "InvalidOpen", InfoMessageType = InfoMessageType.Error)]
        public string PathRef;
        public override string PathTemplate => PathRef;

        [GraphProcessor.ShowInInspector(false), Sirenix.OdinInspector.ShowInInspector, ReadOnly]
        private ConfigBaseNode nodeRef;
        private string GetLabelText()
        {
            return $"模板信息：{GetCustomName()}";
        }
        public override BaseNode NodeRef
        {
            get
            {
                //if (nodeRef == null) Refresh();
                return nodeRef;
            }
        }
        public IConfigBaseNode ConfigBaseNodeRef
        {
            get
            {
                if (NodeRef is IConfigBaseNode configBaseNode)
                {
                    return configBaseNode;
                }
                return null;
            }
        }
        private BaseGraph graphRef;
        public BaseGraph GraphRef
        {
            get
            {
                //if (graphRef == null) Refresh();
                return graphRef;
            }
        }
        private void Refresh(bool forceRfresh = false)
        {
            if (!graph) return;
            Clear();
            var desc = TemplateProvider.Get(graph.GetType(), PathRef, forceRfresh);
            //唯一引用的地方，但是该节点已经废弃了
            //graphRef = desc.Graph;
            nodeRef = desc.Node as ConfigBaseNode;
            if (nodeRef is IConfigBaseNode configBaseNode)
            {
                ID = configBaseNode.GetID();
            }
            var customName = Invalid ? $"错误类型:{ID}" : Path.GetFileName(PathRef);
            SetCustomName($"{name} [{customName}]");
            SyncPortDatas();
        }
        private void Clear()
        {
            graphRef = null;
            nodeRef = null;
            ID = 0;
            invalidOpenMessage = string.Empty;
        }
        private IEnumerable<ValueDropdownItem> GetPathRef()
        {
            if (graph == null) yield break;
            var pathSaves = GraphHelper.GetPathSaves(graph.GetType());
            Utils.PathFormat(ref pathSaves);
            var jsonPaths = Directory.GetFiles(pathSaves, $"*.json", SearchOption.AllDirectories);
            for (int i = 0, length = jsonPaths.Length; i < length; i++)
            {
                var jsonPath = jsonPaths[i];
                Utils.PathFormat(ref jsonPath);
                if (jsonPath.Contains("模板"))
                {
                    var showPath = jsonPath.Substring(pathSaves.Length + 1);
                    yield return new ValueDropdownItem(showPath, jsonPath);
                }
            }
        }
        private void OnValueChanged_PathRef()
        {
            Refresh();
        }
        private void OnAfterEdgeConnected(SerializableEdge edge)
        {
            // 刷新映射数据
            edge.SyncDatas();
        }
        private void OnAfterEdgeDisconnected(SerializableEdge edge)
        {
            // 刷新映射数据
            edge.SyncDatas();
        }
    }
}
