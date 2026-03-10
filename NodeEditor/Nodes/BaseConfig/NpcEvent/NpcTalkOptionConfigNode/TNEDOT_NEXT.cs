using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_NEXT : TalkOptionData
    {
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker]
        [LabelText("跳转对话组")]
        [ValueDropdown("GetNextTalkGroupID")]
        public int NextTalkGroupID = 0;

        public TNEDOT_NEXT(NpcTalkOptionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            BaseNode.InspectorError = string.Empty;

            if (NextTalkGroupID <= 0)
            {
                BaseNode.InspectorError += "跳转对话组不存在\n";
            }
        }

        public override void OnValueChanged()
        {
            BaseNode.SetConfigValue(nameof(BaseNode.Config.IntArg1), NextTalkGroupID);
        }

        public override void OnRefreshCustomName()
        {
            var title = $"[{BaseNode.Config.ID}][选项][{EnumUtility.GetDescription(BaseNode.Config.NpcEventDialogOptionType, false)}]";
            //描述
            if (!string.IsNullOrEmpty(BaseNode.Config.OptionDescEditor))
            {
                title += $"[{BaseNode.Config.OptionDescEditor}]";
            }
            if (NextTalkGroupID != 0)
            {
                title += $"<color=#FF0000>[跳转-{NextTalkGroupID}]</color>";
            }
            BaseNode.SetCustomName(title);
        }

        public override void OnConfigToData()
        {
            NextTalkGroupID = BaseNode.Config.IntArg1;
        }

        public override void SetDefault()
        {
            NextTalkGroupID = 0;
        }

        private IEnumerable<ValueDropdownItem> GetNextTalkGroupID()
        {
            if (BaseNode.inputPorts == default || BaseNode.inputPorts.Count == 0) { yield return default; }

            yield return new ValueDropdownItem($"空", 0);

            var nodeList = BaseNode.GetNpcTalkGroupConfigNodes();
            if (nodeList != default)
            {
                foreach (var node in nodeList)
                {
                    yield return new ValueDropdownItem($"{node.ID}-{node.Config.DescEditor}", node.ID);
                }
            }
        }
    }
}