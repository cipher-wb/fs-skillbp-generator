using System;
using System.Collections.Generic;
using System.Reflection;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_MODIFY_SKILL_TAG_VALUE
    {
        // SkillTagsConfig索引
        public const int ParamIndex = 2;
        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);

            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config?.Params.GetListRef().IndexOf(param);
                    if (index == ParamIndex)
                    {
                        attributes.Add(TParam.VD_TagsValue);
                    }
                    break;
            }
        }
    }
}
