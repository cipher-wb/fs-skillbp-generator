using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace NodeEditor
{
    public interface ITemplateNodeData
    {
        void RefreshGraphRef(string path);

        string GetTemplatePath();

        JsonGraphInfo.NodeInfo GetTemplateGraphInfo();

        void SetTemplateGraphInfo(JsonGraphInfo.NodeInfo templateGraphInfo);
    }
    [Serializable]
    public sealed class TemplateNodeData<TConfig> : ITemplateNodeData where TConfig : class
    {
        public event Action OnRefresh;

        /// <summary>
        /// 模板参数描述文件缓存
        /// </summary>
        private ParamsAnnotation templateParamsAnnotation;

        private bool isRefreshParamsAnnotation = false;

        // TODO 保存模板时候需要刷新下，TParamAnnotation.ForceDoChange()
        [HideIf("@true")]
        public List<TParamAnnotation> TemplateParams = new List<TParamAnnotation>();

        /// <summary>
        /// 模板端口索引
        /// </summary>
        [LabelText("模板端口索引"), ReadOnly]
        public int TemplatePortIndex { get; private set; }

        /// <summary>
        /// 模板开始参数索引
        /// </summary>
        [LabelText("模板开始参数索引"), ReadOnly]
        public int TemplateBeginIndex { get; private set; }

        /// <summary>
        /// 模板引用路径
        /// </summary>
        [LabelText("模板引用路径"), ValueDropdown("GetPathRef", DoubleClickToConfirm = true), OnValueChanged("OnValueChanged_PathRef"), GraphProcessor.ShowInInspector(false), PropertyOrder(-1)]
        [InfoBox("$invalidMessage", "Invalid", InfoMessageType = InfoMessageType.Error)]
        [InfoBox("$invalidOpenMessage", "InvalidOpen", InfoMessageType = InfoMessageType.Error)]
        public string TemplatePath;

        [NonSerialized]
        public JsonGraphInfo.NodeInfo TemplateNodeInfo;
        public void Init(int templatePortIndex, int templateBeginIndex, BaseGraph owner, Action onRfresh)
        {
            TemplatePortIndex = templatePortIndex;
            TemplateBeginIndex = templateBeginIndex;
            ownerGraph = owner;
            OnRefresh = onRfresh;
        }
        public ParamsAnnotation GetTemplateParamsAnnotation(ParamsAnnotation baseAnno)
        {
            if (baseAnno != null && (templateParamsAnnotation == null || templateParamsAnnotation.paramsAnn.Count == 0))
            {
                templateParamsAnnotation = Utils.DeepCopyByBinary(baseAnno);
                isRefreshParamsAnnotation = true;
            }
            if (isRefreshParamsAnnotation)
            {
                if (templateParamsAnnotation != null)
                {
                    var baseCount = baseAnno.paramsAnn.Count;
                    var curCount = templateParamsAnnotation.paramsAnn.Count;
                    if (curCount > baseCount)
                    {
                        templateParamsAnnotation.paramsAnn.RemoveRange(baseCount, curCount - baseCount);
                    }

                    templateParamsAnnotation.paramsAnn.AddRange(TemplateParams);
                }
                isRefreshParamsAnnotation = false;
            }
            return templateParamsAnnotation;
        }

        #region 编辑器
        private BaseGraph ownerGraph;
        private string invalidOpenMessage = string.Empty;
        private static readonly string invalidMessage = "模板节点类型必须为：" + typeof(TConfig).Name;
        public bool Invalid
        {
            get
            {
                if (TemplateNodeInfo != null &&
                    TemplateNodeInfo.IsValid && 
                    TemplateNodeInfo.ConfigName == typeof(TConfig).Name)
                {
                    return false;
                }
                return true;
            }
        }
        private bool InvalidOpen { get { return invalidOpenMessage.Length != 0; } }
        [ButtonGroup("Button", order: -2), Button("打开模板")]
        private void OpenGraphRef()
        {
            Refresh();
            // 通过窗口打开的graph会销毁，这里重新load
            var win = GraphAssetCallbacks.OpenGraphWindow(TemplatePath);
            if (win is ConfigGraphWindow configWin)
            {
                if (TemplateNodeInfo != null && TemplateNodeInfo.IsValid && TemplateNodeInfo.IsTemplate)
                {
                    // 尝试定位节点
                    configWin.FrameNode(TemplateNodeInfo.ConfigName, TemplateNodeInfo.ID);
                }
            }
            else
            {
                invalidOpenMessage = $"打开失败:{TemplatePath}";
            }
        }
        [ButtonGroup("Button", order: -2), Button("刷新模板")]
        public void RefreshGraphRef()
        {

            Refresh(true);
            // 刷新节点的模板参数列表
            isRefreshParamsAnnotation = true;
            if (TemplateNodeInfo != null && TemplateNodeInfo.TemplateParams != null)
            {
                TemplateParams = Utils.DeepCopyByBinary(TemplateNodeInfo.TemplateParams);
            }
            else
            {
                TemplateParams.Clear();
            }

            OnRefresh?.Invoke();
        }

        public string GetTemplatePath() => TemplatePath;
        public void Refresh(bool forceRfresh = false)
        {
            invalidOpenMessage = string.Empty;
            TemplateNodeInfo = default;
            if (ownerGraph)
            {
                JsonGraphManager.Inst.TryGetTemplateNodeInfo(TemplatePath, out TemplateNodeInfo);

                //TemplateGraphDescription desc;
                //if (ownerGraph.path == TemplatePath)
                //{
                //    desc = TemplateGraphDescription.Create(ownerGraph);
                //}
                //else
                //{
                //    desc = TemplateProvider.Get(ownerGraph.GetType(), TemplatePath, forceRfresh);
                //}
                //if (desc.Node is IConfigBaseNode configBaseNode && 
                //    configBaseNode.GetConfigName() == typeof(TConfig).Name)
                //{
                //    TemplateNodeInfo = desc;
                //}
            }
        }
        private IEnumerable<ValueDropdownItem> GetPathRef()
        {
            if (ownerGraph == null) yield break;
            yield return new ValueDropdownItem("空模板", "");
            var pathTemplates = GraphHelper.GetTemplatePaths(ownerGraph.GetType());
            foreach (var kv in pathTemplates)
            {
                if (JsonGraphManager.Inst.TryGetTemplateNodeInfo(kv.Key, out var nodeInfo) && nodeInfo.ConfigName == typeof(TConfig).Name)
                {
                    yield return new ValueDropdownItem(kv.Value, kv.Key);
                }
            }
        }
        private void OnValueChanged_PathRef()
        {
            //Refresh();
            if (EditorUtility.DisplayDialog($"是否刷新模板", "模板路径变动，是否刷新", "是", "否"))
            {
                RefreshGraphRef();
            }
        }
        public void RefreshGraphRef(string path)
        {
            TemplatePath = path;
            RefreshGraphRef();
        }

        public JsonGraphInfo.NodeInfo GetTemplateGraphInfo() => TemplateNodeInfo;

        public void SetTemplateGraphInfo(JsonGraphInfo.NodeInfo templateGraphInfo)
        {
            this.TemplateNodeInfo = templateGraphInfo;
        }
        #endregion
    }
}
