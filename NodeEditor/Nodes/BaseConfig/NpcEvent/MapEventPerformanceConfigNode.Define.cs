using GraphProcessor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 定义其他结构
    /// </summary>
    public partial class MapEventPerformanceConfigNode
    {
        #region ValueDropdown
        public readonly static ValueDropdownList<int> VDL_State = new ValueDropdownList<int>
        {
            {"凡人境",1},{"炼气期",2},
            {"筑基前期",3},{"筑基中期",4},{"筑基后期",5},
            {"淬灵前期",6},{"淬灵中期",7},{"淬灵后期",8},
            {"金丹前期",9},{"金丹中期",10},{"金丹后期",11},
            {"元婴前期",12},{"元婴中期",13},{"元婴后期",14},
            {"化神前期",15},{"化神中期",16},{"化神后期",17},
        };
        #endregion

        #region Enum
        /// <summary>
        /// 动作类型
        /// </summary>
        public enum PlayAnimType
        {
            [System.ComponentModel.Description("常规动作")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("常规动作")]
            Normal = 1,

            [System.ComponentModel.Description("Override动作")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("Override动作")]
            Override = 2,
        }

        /// <summary>
        /// 创建自定义对象类型
        /// </summary>
        public enum ModelPositionType
        {
            [System.ComponentModel.Description("固定点")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("固定点")]
            Position = 1,

            [System.ComponentModel.Description("跟随执行者")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("跟随执行者")]
            Follow = 2,
        }

        /// <summary>
        /// 淡入淡出类型
        /// </summary>
        public enum FadeType
        {
            [System.ComponentModel.Description("现身")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("现身")]
            FadeIn = 1,

            [System.ComponentModel.Description("隐身")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("隐身")]
            FadeOut = 2,
        }

        public enum TargetType
        {
            [System.ComponentModel.Description("自己")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("自己")]
            Self = 0,

            [System.ComponentModel.Description("其他演员或自定义对象")]
            [Sirenix.OdinInspector.ShowInInspector, LabelText("其他演员或自定义对象")]
            Other = 1,
        }
        #endregion
    }
}
