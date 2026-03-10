using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.IO;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_RUN_SKILL_EFFECT_TEMPLATE : IParamsNode, ITemplateReceiverNode, IRefConfigBaseNode
    {
        public int RefConfigID => GetParamsList()?.ExGet(TemplatePortIndex, null)?.Value ?? 0;
        public string RefConfigName => GetConfigName();

        [FoldoutGroup("模板引用信息", true), HideLabel, HideReferenceObjectPicker, GraphProcessor.ShowInInspector(false), PropertyOrder(-1), PropertySpace(5, 15)]
        public TemplateNodeData<SkillEffectConfig> TemplateData = new TemplateNodeData<SkillEffectConfig>();
        public ParamsAnnotation TemplateParamsAnnotation
        {
            get
            {
                var baseAnno = base.GetParamsAnnotation();
                return TemplateData.GetTemplateParamsAnnotation(baseAnno);
            }
        }
        public int TemplateBeginIndex => TemplateData.TemplateBeginIndex;
        public int TemplatePortIndex => TemplateData.TemplatePortIndex;
        public ITemplateNodeData TemplateNodeData => TemplateData;
        protected override void Enable()
        {
            base.Enable();
            TemplateData.Init(2, 3, graph, OnRefreshTemplate);
            TemplateData.RefreshGraphRef();
        }
        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = base.GetParamsAnnotation();
            return TemplateData.GetTemplateParamsAnnotation(baseAnno);
        }
        protected override bool CustomParamsPostProcessing()
        {
            return this.CustomParamsPostProcessingTemplate();
        }
        private void OnRefreshTemplate()
        {
            Utils.SafeCall(()=>
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
            SetCustomName($"模板效果({simpleName}) [{name}:{ID}]");
        }
        public override bool OnPostProcessing()
        {
            if (!string.IsNullOrEmpty(TemplateData.TemplatePath))
            {
                // TODO 性能优化
                //TemplateData.RefreshGraphRef();
                TemplateData.Refresh(false);
                OnRefreshTemplateWithoutID();
            }
            // 修改老数据
            //GraphProcessor.BaseNode inputNode = null;
            //foreach (var outputEdge in GetOutputEdges())
            //{
            //    if (outputEdge.outputPortIdentifier == TemplatePortIndex.ToString() &&
            //        outputEdge.inputNode is RefTemplateBaseNode)
            //    {
            //        inputNode = outputEdge.inputNode;
            //        break;
            //    }
            //}
            //if (inputNode is RefTemplateBaseNode refTemplateBaseNode)
            //{
            //    TemplateData.TemplatePath = refTemplateBaseNode.PathTemplate;
            //    graph.RemoveNode(inputNode);
            //    TemplateData.RefreshGraphRef();
            //}
            return base.OnPostProcessing();
        }

        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                if (!IsValidTemplate(out int templateID))
                {
                    AppendSaveRet($"模板ID存在差异-{templateID}->{TemplateData?.TemplateNodeInfo?.ID}");
                    return false;
                }
            }
            return ret;
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
