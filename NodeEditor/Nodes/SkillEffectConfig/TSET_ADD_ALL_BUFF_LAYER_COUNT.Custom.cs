using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class TSET_ADD_ALL_BUFF_LAYER_COUNT
    {
        // 节点描述记录
        private ParamsAnnotation customAnno;
        // 刷新显示枚举值
        private int cacheBuffOptType = -1; // TSkillEffectBuffOverlyingOptType
        private int cacheBuffAddType = -1; // TSkillEffectBuffOverlyingAddType

        protected override void OnConfigChanged()
        {
            var curOptType = Config?.Params.ExGet(1)?.Value ?? cacheBuffOptType;
            var curAddType = Config?.Params.ExGet(3)?.Value ?? cacheBuffAddType;
            if (curOptType != cacheBuffOptType || curAddType != cacheBuffAddType)
            {
                cacheBuffOptType = curOptType;
                cacheBuffAddType = curAddType;
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

            baseAnno.paramsAnn[2].RefTypeName = "";
            baseAnno.paramsAnn[2].Name = "-";
            baseAnno.paramsAnn[4].Name = "-";

            TableDR.TSkillEffectBuffOverlyingOptType eOptType = (TableDR.TSkillEffectBuffOverlyingOptType)paramsList[1].Value;
            switch (eOptType)
            {
                case TSkillEffectBuffOverlyingOptType.TSEBOOT_BUFF_ID:
                    baseAnno.paramsAnn[2].Name = "指定BuffID";
                    break;
                case TSkillEffectBuffOverlyingOptType.TSEBOOT_BUFF_TYPE:
                    baseAnno.paramsAnn[2].Name = "指定Buff类型";
                    baseAnno.paramsAnn[2].RefTypeName = "TBuffType";
                    break;
                default:
                    break;
            }

            TableDR.TSkillEffectBuffOverlyingAddType eAddType = (TableDR.TSkillEffectBuffOverlyingAddType)paramsList[3].Value;
            switch (eAddType)
            {
                case TSkillEffectBuffOverlyingAddType.TSEBOAT_COUNT:
                    baseAnno.paramsAnn[4].Name = "增加层数数量";
                    break;
                case TSkillEffectBuffOverlyingAddType.TSEBOAT_PERCENT:
                    baseAnno.paramsAnn[4].Name = "增加万分比[数量向下取整]";
                    break;
                default:
                    break;
            }

            baseAnno.paramsAnn[2].ForceDoChange();
        }
    }
}
