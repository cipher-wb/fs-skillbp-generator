using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Reflection;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_UNREGISTER_SKILL_EVENT
    {// SkillEventConfig索引
        public const int ParamIndex = 0;
        public override void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessParamAttributes(param, member, attributes);

            switch (member.Name)
            {
                case nameof(param.Value):
                    var index = Config?.Params.GetListRef().IndexOf(param);
                    if (index == ParamIndex)
                    {
                        attributes.Add(TParam.VD_EventValue);
                    }
                    break;
            }
        }

        public override bool OnSaveCheck()
        {
            // 反注册节点检测参数是否正确
            var ret = base.OnSaveCheck();
            if (ret)
            {
                var paramsList = GetParamsList();
                if (paramsList != null && paramsList.Count >= 5 && graph is ConfigGraph configGraph)
                {
                    int effectId = paramsList[1].Value;
                    if (effectId != 0)
                    {
                        int eventId = paramsList[0].Value;
                        int eventSubType = paramsList[3].Value;
                        int eventSubTypeVal = paramsList[4].Value;
                        BaseNode targetNode = configGraph.GetNodeByConfigNameAndID(nameof(SkillEffectConfig), effectId);
                        if (targetNode is IParamsNode targetParamsNode && targetNode is IConfigBaseNode targetConfigNode)
                        {
                            IReadOnlyList<TParam> paramList1 = targetParamsNode.GetParamsList();
                            if (paramList1.Count >= 11)
                            {
                                int eventId1 = paramList1[0].Value;
                                int eventSubType1 = paramList1[9].Value;
                                int eventSubTypeVal1 = paramList1[10].Value;
                                if (eventId != eventId1 || eventSubType != eventSubType1 || eventSubTypeVal != eventSubTypeVal1)
                                {
                                    AppendSaveRet($"反注册技能消息与注册消息的节点:{targetConfigNode.GetID()}参数值不一致");
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
}
