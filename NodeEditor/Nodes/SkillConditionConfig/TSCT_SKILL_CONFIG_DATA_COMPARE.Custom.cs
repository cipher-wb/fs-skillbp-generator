using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSCT_SKILL_CONFIG_DATA_COMPARE
    {
        // TODO 改为TableDR消息处理
        // TODO 刷新显示枚举值
        private TSkillConfigDataFieldType cacheType;
        protected override void OnConfigChanged()
        {
            var curType = Config?.Params.ExGet(1)?.Value ?? (int)cacheType;
            if (curType != (int)cacheType)
            {
                cacheType = (TSkillConfigDataFieldType)curType;
                TryRepaint();
            }
            base.OnConfigChanged();
        }
        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);

            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config.Params.IndexOf(param);
                    // 依据第二个参数的类型，第三个参数先不同类型的数据
                    if (index == 2)
                    {
                        var param2 = Config?.Params.ExGet(1);
                        var param2Type = (TSkillConfigDataFieldType)param2.Value;
                        switch (param2Type)
                        {
                            case TSkillConfigDataFieldType.TSCDFT_SKILL_TYPE:
                                {
                                    attributes.Add(new ValueDropdownAttribute($"@TableDR.EnumUtility.VD_TBattleSkillSubType") { DropdownTitle = $"选择技能子类型...", });
                                    break;
                                }
                            case TSkillConfigDataFieldType.TSCDFT_ELEMENT_TYPE:
                                {
                                    attributes.Add(new ValueDropdownAttribute($"@TableDR.EnumUtility.VD_TElementsType") { DropdownTitle = $"选择五行类型...", });
                                    break;
                                }
                            case TSkillConfigDataFieldType.TSCDFT_DAMAGE_TYPE:
                                {
                                    attributes.Add(new ValueDropdownAttribute($"@TableDR.EnumUtility.VD_TSkillDamageType") { DropdownTitle = $"选择伤害类型...", });
                                    break;
                                }
                        }
                    }
                    break;
            }
        }
    }
}
