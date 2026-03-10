using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    [NodeMenuItem("主行为/设置气氛组进入冷却", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed class TEVAT_SET_AMBIENCE_COLDDOWN : NpcEventActionConfigNode
    {
        public TEVAT_SET_AMBIENCE_COLDDOWN() : base(NpcEventActionConfig_TEventActionType.TEVAT_SET_AMBIENCE_COLDDOWN)
        {
            ActionData = new ActionSetAmbienceColddownData(this);
        }
    }

    public class ActionSetAmbienceColddownData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("气氛组ID"), DelayedProperty]
        public TableSelectData ColdDownData = null;

        public ActionSetAmbienceColddownData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            if (ColdDownData == null || ColdDownData.TableConfig == default)
            {
                BaseNode.InspectorError += $"配置表不存在 {ColdDownData?.ID} \n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            param?.ForEach(auctionID =>
            {
                ColdDownData ??= new TableSelectData(typeof(CommonNpcAmbienceConfig).FullName, param[0]);
                ColdDownData.OnSelectedID();
            });
        }

        public override List<int> ToParam()
        {
            ColdDownData ??= new TableSelectData(typeof(CommonNpcAmbienceConfig).FullName, 0);

            return new List<int>() { ColdDownData.ID };
        }
    }
}
