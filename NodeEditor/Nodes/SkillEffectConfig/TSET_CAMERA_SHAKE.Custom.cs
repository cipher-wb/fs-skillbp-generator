using Funny.Base.Utils;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;
using System;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_CAMERA_SHAKE
    {
        // BattleCameraShakeConfig索引
        public const int ParamIndex = 5;
        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);

            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config?.Params.GetListRef().IndexOf(param);
                    if (index == ParamIndex)
                    {
                        attributes.Add(TParam.VD_CameraShakeValue);
                    }
                    break;
            }
        }
        [Button("打开曲线编辑器", ButtonSizes.Medium)]
        private void OpenCurveEditor()
        {
            DreamlandCurveEditor.OpenWindow();
        }
    }
}
