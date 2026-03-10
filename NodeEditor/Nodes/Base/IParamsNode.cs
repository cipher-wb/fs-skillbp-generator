using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    // TODO 抽出ConfigBaseNode关于Params的接口
    public interface IParamsNode
    {
        public void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes);
        public ParamsAnnotation GetParamsAnnotation();
        public string GetParamsName();
        public IReadOnlyList<TParam> GetParamsList();
        public void RefreshParamsDisplayName();
    }
}
