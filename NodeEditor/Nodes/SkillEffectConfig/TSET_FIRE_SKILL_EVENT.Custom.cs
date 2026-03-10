using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using Funny.Base.Utils;

namespace NodeEditor
{
    public partial class TSET_FIRE_SKILL_EVENT
    {
        // SkillEventConfig索引
        public const int ParamIndex = 1;

        // 节点描述记录
        private ParamsAnnotation customAnno;
        // 刷新显示枚举值
        private int cacheSkillEventConfigID = -1;

        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);

            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config.Params.IndexOf(param);
                    if (index == ParamIndex)
                    {
                        attributes.Add(TParam.VD_EventValue);
                    }
                    break;
            }
        }

        protected override void OnConfigChanged()
        {
            var curID = Config?.Params.ExGet(1)?.Value ?? cacheSkillEventConfigID;
            if (curID != cacheSkillEventConfigID)
            {
                cacheSkillEventConfigID = curID;
                RefreshNameAnnoName();
            }
            base.OnConfigChanged();
        }

        public override bool OnPostProcessing()
        {
            RefreshNameAnnoName();
            bool ret = base.OnPostProcessing();
            return ret;
        }

        public override ParamsAnnotation GetParamsAnnotation()
        {
            var baseAnno = base.GetParamsAnnotation();
            if (baseAnno != null && (customAnno == null || customAnno.paramsAnn == null || customAnno.paramsAnn.Count == 0))
            {
                customAnno = Utils.DeepCopyByBinary(baseAnno);
            }
            return customAnno;
        }
        private void RefreshNameAnnoName()
        {
            var paramsList = GetParamsList();
            var baseAnno = GetParamsAnnotation();
            if (paramsList == null || baseAnno == null)
            {
                return;
            }

            var eventConfigs = SkillEventConfigManager.Instance.GetItem(paramsList[1].Value);
            if (eventConfigs != null)
            {
                for (int i = 2, length = baseAnno.paramsAnn.Count; i < length; i++)
                {
                    var memberValue = eventConfigs.ExGetValue($"ParamName{(i-1)}");
                    if (memberValue is string paramName)
                    {
                        baseAnno.paramsAnn[i].Name = string.IsNullOrEmpty(paramName) ? "不填" : paramName;
                    }
                }
            }
            else
            {
                for (int i = 2, length = baseAnno.paramsAnn.Count; i < length; i++)
                {
                    baseAnno.paramsAnn[i].Name = $"参数{(i-1)}";
                }
            }
        }
    }
}
