using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    [Serializable]
    [NodeMenuItem("主行为/删除动态气氛组", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed class TEVAT_DEL_DYNAMIC_AMBIENCE : NpcEventActionConfigNode
    {
        public TEVAT_DEL_DYNAMIC_AMBIENCE() : base(NpcEventActionConfig_TEventActionType.TEVAT_CREATE_DYNAMIC_AMBIENCE)
        {
            ActionData = new ActionDelDynamicAmbienceData(this);
        }
    }

    public class ActionDelDynamicAmbienceData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("气氛组ID"), DelayedProperty]
        public TableSelectData AmbienceConfig = null;

        [HideReferenceObjectPicker, LabelText("区块所在随机点ID"), DelayedProperty]
        public TableSelectData RandomPointConfigId = null;

        public ActionDelDynamicAmbienceData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            if (AmbienceConfig == null || AmbienceConfig.TableConfig == null)
            {
                BaseNode.InspectorError += $"CommonNpcAmbienceConfig配置不存在 {AmbienceConfig?.ID} \n";
            }

            if (RandomPointConfigId == null || RandomPointConfigId.TableConfig == null)
            {
                BaseNode.InspectorError += $"随机点配置不存在 {RandomPointConfigId?.ID} \n";
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
                RandomPointConfigId = new TableSelectData(typeof(MapRandomPoint).FullName, param[1]);
            }
        }

        public override List<int> ToParam()
        {
            AmbienceConfig ??= new TableSelectData(typeof(CommonNpcAmbienceConfig).FullName, 0);
            RandomPointConfigId ??= new TableSelectData(typeof(MapRandomPoint).FullName, 0);

            return new List<int>() { AmbienceConfig.ID, RandomPointConfigId.ID };
        }
    }

}
