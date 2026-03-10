using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{

    [Serializable]
    [NodeMenuItem("主行为/创建动态气氛组", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed class TEVAT_CREATE_DYNAMIC_AMBIENCE : NpcEventActionConfigNode
    {
        public TEVAT_CREATE_DYNAMIC_AMBIENCE() : base(NpcEventActionConfig_TEventActionType.TEVAT_CREATE_DYNAMIC_AMBIENCE)
        {
            ActionData = new ActionCreateDynamicAmbienceData(this);
        }
    }

    public class ActionCreateDynamicAmbienceData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("气氛组ID"), DelayedProperty]
        public TableSelectData AmbienceConfig = null;

        [HideReferenceObjectPicker, LabelText("区块所在随机点ID"), DelayedProperty]
        public TableSelectData RandomPointId = null;

        [HideReferenceObjectPicker, LabelText("持续CD类型"), DelayedProperty]
        public CommonNpcAmbienceConfig_TColdDownType ColdDownType = CommonNpcAmbienceConfig_TColdDownType.TCDT_NULL;

        [HideReferenceObjectPicker, LabelText("持续多久"), DelayedProperty]
        public int coldDownVal = 0;

        public ActionCreateDynamicAmbienceData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            if(AmbienceConfig == null || AmbienceConfig.TableConfig == null)
            {
                BaseNode.InspectorError += $"CommonNpcAmbienceConfig配置不存在 {AmbienceConfig?.ID} \n";
            }

            if (RandomPointId == null || RandomPointId.TableConfig == null)
            {
                BaseNode.InspectorError += $"区块配置不存在 {RandomPointId?.ID} \n";
            }

            if(ColdDownType == CommonNpcAmbienceConfig_TColdDownType.TCDT_NULL)
            {
                BaseNode.InspectorError += $"需要配置持续CD类型 \n";
            }

            if(coldDownVal == 0)
            {
                BaseNode.InspectorError += $"持续cd为0?? \n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param == null || param.Count == 0)
            {
                return;
            }
            AmbienceConfig = new TableSelectData(typeof(CommonNpcAmbienceConfig).FullName, param[0]);
            if (param.Count >= 2)
            {
                RandomPointId = new TableSelectData(typeof(MapRandomPoint).FullName, param[1]);
            }
            if (param.Count >= 3)
            {
                ColdDownType = (CommonNpcAmbienceConfig_TColdDownType)param[2];
            }
            if (param.Count >= 4)
            {
                coldDownVal = param[3];
            }
        }

        public override List<int> ToParam()
        {
            AmbienceConfig ??= new TableSelectData(typeof(CommonNpcAmbienceConfig).FullName, 0);
            RandomPointId ??= new TableSelectData(typeof(MapRandomPoint).FullName,0);

            return new List<int>() { AmbienceConfig.ID, RandomPointId.ID, (int)ColdDownType , coldDownVal };
        }
    }

}
