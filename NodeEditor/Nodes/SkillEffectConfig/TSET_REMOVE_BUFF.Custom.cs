using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class TSET_REMOVE_BUFF
    {
        // 节点描述记录
        private ParamsAnnotation customAnno;
        // 刷新显示枚举值
        private int cacheBuffRemoveType = -1;

        protected override void OnConfigChanged()
        {
            var curType = Config?.Params.ExGet(1)?.Value ?? cacheBuffRemoveType;
            if (curType != cacheBuffRemoveType)
            {
                cacheBuffRemoveType = curType;
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
            baseAnno.paramsAnn[3].RefTypeName = "";
            baseAnno.paramsAnn[4].RefTypeName = "";
            baseAnno.paramsAnn[5].RefTypeName = "";
            baseAnno.paramsAnn[2].Name = "-";
            baseAnno.paramsAnn[3].Name = "-";
            baseAnno.paramsAnn[4].Name = "-";
            baseAnno.paramsAnn[5].Name = "-";

            TableDR.TSkillEffectBuffRemoveType eRemoveType = (TableDR.TSkillEffectBuffRemoveType)paramsList[1].Value;
            switch (eRemoveType)
            {
                case TSkillEffectBuffRemoveType.TSEBRT_BUFF_ID:
                    baseAnno.paramsAnn[2].Name = "BuffID";
                    baseAnno.paramsAnn[3].Name = "移除层数(填0=全部层)";
                    baseAnno.paramsAnn[4].Name = "Buff来源单位实例ID（0不区分）";
                    break;
                case TSkillEffectBuffRemoveType.TSEBRT_GROUP_ID:
                    baseAnno.paramsAnn[2].Name = "中断标记ID(SkillTagID)";
                    break;
                case TSkillEffectBuffRemoveType.TSEBRT_BUFF_TYPE:
                    baseAnno.paramsAnn[2].Name = "Buff类型";
                    baseAnno.paramsAnn[2].RefTypeName = "TBuffType";
                    baseAnno.paramsAnn[3].Name = "移除数量(随机移除,填0=全部)";
                    baseAnno.paramsAnn[4].Name = "移除Buff等阶(<=)";
                    baseAnno.paramsAnn[5].Name = "Buff来源单位实例ID（0不区分）";
                    break;
                case TSkillEffectBuffRemoveType.TSEBRT_BUFF_TIMER_TASK_INDEX:
                    baseAnno.paramsAnn[2].Name = "添加Buff返回的TaskIndex(仅移除1层)";
                    break;
                default:
                    baseAnno.paramsAnn[2].Name = "配置错误，请联系程序";
                    break;
            }

            baseAnno.paramsAnn[2].ForceDoChange();
        }
    }
}
