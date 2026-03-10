using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    public partial class ModelConfigNode : IParamsNode
    {
        public void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            //throw new NotImplementedException();
        }

        protected override void OnPreset()
        {
            base.OnPreset();
            Config.ExSetValue(nameof(Config.DisappearType), ModelConfig_TModelConfigDisappearType.TMCDT_ANIM);
            Config.ExSetValue(nameof(Config.BodyType), B2BodyType.B2BT_DYNAMIC);
        }
    }
}
