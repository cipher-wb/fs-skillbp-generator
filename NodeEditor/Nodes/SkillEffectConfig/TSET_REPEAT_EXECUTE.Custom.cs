using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_REPEAT_EXECUTE
    {
        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                var frameCount = Config?.Params.ExGet(0);
                var executeCount = Config?.Params.ExGet(1);
                if (frameCount.ParamType == TableDR.TParamType.TPT_NULL &&
                    executeCount.ParamType == TableDR.TParamType.TPT_NULL &&
                    frameCount.Value <= 0 && executeCount.Value > 100)
                {
                    AppendSaveRet("重复执行_间隔帧数_执行次数错误");
                    return false;
                }
            }
            return ret;
        }
    }
}
