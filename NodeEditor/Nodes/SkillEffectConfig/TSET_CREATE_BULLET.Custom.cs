using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_CREATE_BULLET
    {
        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                var bulletID = Config?.Params.ExGet(0);
                if (bulletID.ParamType == TableDR.TParamType.TPT_NULL && bulletID.Value == 0)
                {
                    AppendSaveRet("子弹ID不允许为0");
                    ret = false;
                }
            }
            return ret;
        }
    }
}
