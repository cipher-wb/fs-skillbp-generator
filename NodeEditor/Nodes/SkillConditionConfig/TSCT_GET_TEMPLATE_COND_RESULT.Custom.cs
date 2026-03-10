using GraphProcessor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using TableDR;

namespace NodeEditor
{
    public partial class TSCT_GET_TEMPLATE_COND_RESULT : IParamsNode, ITemplateReceiverNode, IRefConfigBaseNode
    {
        public int RefConfigID => GetParamsList()?.ExGet(TemplatePortIndex, null)?.Value ?? 0;
        public string RefConfigName => GetConfigName();

        [FoldoutGroup("模板引用信息", true), HideLabel, HideReferenceObjectPicker, GraphProcessor.ShowInInspector(false), PropertyOrder(-1), PropertySpace(5, 15)]
        public TemplateNodeData<SkillConditionConfig> TemplateData = new TemplateNodeData<SkillConditionConfig>();

        // TODO 改为读取默认
        private ParamsAnnotation templateParamsAnnotation;
        public ParamsAnnotation TemplateParamsAnnotation
        {
            get
            {
                if (templateParamsAnnotation == null || templateParamsAnnotation.paramsAnn.Count == 0)
                {
                    var baseAnno = base.GetParamsAnnotation();
                    templateParamsAnnotation = Utils.DeepCopyByBinary(baseAnno);
                }
                return templateParamsAnnotation;
            }
        }
        public int TemplateBeginIndex => 2;
        public int TemplatePortIndex => 1;
        public ITemplateNodeData TemplateNodeData => TemplateData;
        protected override void Enable()
        {
            base.Enable();
            TemplateData.Init(1, 2, graph, OnRefreshTemplate);
            TemplateData.RefreshGraphRef();
        }
        protected override void Disable()
        {
            base.Disable();
            templateParamsAnnotation = null;
        }
        protected override void OnAfterEdgeConnected(SerializableEdge edge)
        {
            base.OnAfterEdgeConnected(edge);
            CustomParamsPostProcessing();
        }
        protected override void OnAfterEdgeDisconnected(SerializableEdge edge)
        {
            base.OnAfterEdgeDisconnected(edge);
            CustomParamsPostProcessing();
        }
        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = base.GetParamsAnnotation();
            return TemplateData.GetTemplateParamsAnnotation(baseAnno);
        }
        protected override List<PortData> DoGetParamsAnnotationPortDatas(List<SerializableEdge> edges)
        {
            CustomParamsPostProcessing();
            return base.DoGetParamsAnnotationPortDatas(edges);
        }
        protected override bool CustomParamsPostProcessing()
        {
            return this.CustomParamsPostProcessingTemplate();
        }
        public override bool OnPostProcessing()
        {
            if (!string.IsNullOrEmpty(TemplateData.TemplatePath))
            {
                //TemplateData.RefreshGraphRef();
                TemplateData.Refresh(false);
                OnRefreshTemplateWithoutID();
            }
            return base.OnPostProcessing();
        }
        private void OnRefreshTemplate()
        {
            Utils.SafeCall(() =>
            {
                var paramList = GetParamsList();
                if (paramList != null)
                {
                    var tParam = paramList.ExGet(TemplateData.TemplatePortIndex);
                    tParam.ExSetValue(nameof(tParam.Value), TemplateData?.TemplateNodeInfo?.ID);
                }
                OnRefreshTemplateWithoutID();
            });
        }
        private void OnRefreshTemplateWithoutID()
        {
            CustomParamsPostProcessing();
            SyncPortDatas();
            Desc = "错误模板路径";
            var desc = TemplateData.GetTemplateGraphInfo();
            if (TemplateData.TemplatePath != null && File.Exists(TemplateData.TemplatePath) && desc != null)
            {
                Desc = Path.GetFileName(TemplateData.TemplatePath);

                var templateDesc = desc.Desc;

                if (!string.IsNullOrEmpty(templateDesc))
                {
                    Desc = $"{Desc}" +
                        $"\n-----------------------模板说明-----------------------\n" +
                        $"{templateDesc}";
                }
            }
            OnDescChanged();
        }
        protected override void OnRefreshCustomName()
        {
            var simpleName = GraphHelper.GetSimpleGraphName(TemplateData.TemplatePath);
            SetCustomName($"模板条件({simpleName}) [{name}:{ID}]");
        }

        public bool IsValidTemplate(out int templateID)
        {
            var paramList = GetParamsList();
            templateID = paramList.ExGet(TemplateData.TemplatePortIndex).Value;
            var compareID = TemplateData?.TemplateNodeInfo?.ID;
            return templateID == compareID;
        }
    }
}
