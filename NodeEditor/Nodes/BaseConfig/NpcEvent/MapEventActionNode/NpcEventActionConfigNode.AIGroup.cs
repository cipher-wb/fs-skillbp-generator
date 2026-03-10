using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public partial class NpcEventActionConfigNode
    {
        /// <summary>
        /// 显示交互选项
        /// </summary>
        public bool IsShowInteractItem => IsSubActionNode && Config.ActionType == NpcEventActionConfig_TEventActionType.TEVAT_DIALOG;

        /// <summary>
        /// AIGroup是否有效
        /// </summary>
        public bool IsValidAIGroupID => enumAIGroupID == 0 || handAIGroupID == 0;

        /// <summary>
        /// 还原到自定义数据
        /// </summary>
        private void ConfigToAI()
        {
            if (Config?.AIParamsID?.Count > 0)
            {
                enumAIGroupID = Config.AIParamsID[0];
            }
            if (Config?.AIParamsID?.Count > 1)
            {
                handAIGroupID = Config.AIParamsID[1];
            }

            //主行为 如果没有配置AI，默认给个
            if(!IsValidAIGroupID && !IsSubActionNode)
            {
                enumAIGroupID = (int)ActionAIMode.InteractCD;
            }
            OnAIChanged();

            //子行为 默认添加交互标识
            if (IsSubActionNode)
            {
                activeInteract = Config?.ActiveInteract ?? true;
                triggerInteract = Config?.TriggerInteract ?? false;
            }
            OnInteractChanged();
        }

        #region AI组

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker]
        [LabelText("AI类型"), FoldoutGroup("自主行为", order: 0), ShowIf("@IsSubActionNode == false")]
        [OnValueChanged("OnAIChanged", true), ValueDropdown("@VD_AIMode")]
        private int enumAIGroupID = 0;

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker]
        [LabelText("AI类型-手填(优先级更高)"), FoldoutGroup("自主行为", order: 0), ShowIf("@IsSubActionNode == false")]
        [OnValueChanged("OnAIChanged", true)]
        private int handAIGroupID = 0; 

        public void OnAIChanged()
        {
            List<int> idList = new List<int>()
            {
                enumAIGroupID,
                handAIGroupID,
            };
            SetConfigValue(nameof(Config.AIParamsID), idList);
        }
        #endregion

        #region 交互类型
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, ShowIf("IsShowInteractItem")]
        [LabelText("玩家主动与演员触发"), FoldoutGroup("自主行为", order: 0)]
        [OnValueChanged("OnInteractChanged", true)]
        private bool activeInteract = true;

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, ShowIf("IsShowInteractItem")]
        [LabelText("演员主动与玩家触发"), FoldoutGroup("自主行为", order: 0)]
        [OnValueChanged("OnInteractChanged", true)]
        private bool triggerInteract = false;

        public void OnInteractChanged()
        {
            SetConfigValue(nameof(Config.ActiveInteract), activeInteract);
            SetConfigValue(nameof(Config.TriggerInteract), triggerInteract);
        }
        #endregion
    }
}
