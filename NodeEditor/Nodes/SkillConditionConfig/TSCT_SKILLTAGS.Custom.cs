using System;
using System.Collections.Generic;
using System.Reflection;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSCT_SKILLTAGS
    {
        // SkillTagsConfig索引
        public const int TagValueIndex = 2;
        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);

            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config.Params.IndexOf(param);
                    if (index == TagValueIndex)
                    {
                        attributes.Add(TParam.VD_TagsValue);
                    }
                    break;
            }
        }
    }
}
