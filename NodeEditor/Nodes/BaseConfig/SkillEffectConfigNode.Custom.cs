using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class SkillEffectConfigNode : IParamsNode
    {
        // 名字由类型自动处理，不可自定义名字
        public override bool isRenamable => false;

        [HideInInspector]
        public TSkillEffectType SkillEffectType;
        public SkillEffectConfigNode(TSkillEffectType type) : base()
        {
            SkillEffectType = type;
        }
        /// <summary>
        /// 成员参数属性自定义处理
        /// </summary>
        public virtual void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes) { }
        protected override void OnRefreshCustomName()
        {
            // 依据表格效果类型显示不同表现
            SetCustomName($"{SkillEffectType.GetDescription(false)} [{name}:{ID}]");
        }
        protected override void OnPreset()
        {
            base.OnPreset();
            Config.ExSetValue(nameof(Config.SkillEffectType), SkillEffectType);
        }
        public override ParamsAnnotation GetParamsAnnotation()
        {
            return TableAnnotation.Inst.GetParamsAnnotation(SkillEffectType);
        }
        public override string GetParamsName()
        {
            return nameof(Config.Params);
        }
        public override IReadOnlyList<TParam> GetParamsList()
        {
            return Config.Params;
        }

    }
}
