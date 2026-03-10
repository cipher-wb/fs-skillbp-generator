#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;

namespace TableDR
{
    public partial class NpcMatchData
    {
        public NpcMatchData() { }

        public NpcMatchData(TNpcMatchRuleType rule, int value, int valueEnd)
        {
            Rule = rule;
            Value = value;
            ValueEnd = valueEnd;
        }
        
        public NpcMatchData Clone()
        {
            return new NpcMatchData(Rule, Value, ValueEnd);
        }

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<int> VDL_State = new ValueDropdownList<int>
        {
            {"空", 0},{"凡人境",1},{"炼气期",2},
            {"筑基前期",3},{"筑基中期",4},{"筑基后期",5},
            {"淬灵前期",6},{"淬灵中期",7},{"淬灵后期",8},
            {"金丹前期",9},{"金丹中期",10},{"金丹后期",11},
            {"元婴前期",12},{"元婴中期",13},{"元婴后期",14},
            {"化神前期",15},{"化神中期",16},{"化神后期",17},
        };

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<int> VDL_ItemRank = new ValueDropdownList<int>
        {
            {"白",1},
            {"蓝",2},
            {"紫",3},
            {"橙",4},
            {"金",5},
        };

        [NonSerialized, JsonIgnore]
        public ValueDropdownList<int> VDL_RankLevel = new ValueDropdownList<int>
        {
            {TRankLevelType.TRLT_None.GetDescription(false), (int)TRankLevelType.TRLT_None},
            {TRankLevelType.TRLT_One.GetDescription(false), (int)TRankLevelType.TRLT_One},
            {TRankLevelType.TRLT_Two.GetDescription(false), (int)TRankLevelType.TRLT_Two},
            {TRankLevelType.TRLT_Three.GetDescription(false), (int)TRankLevelType.TRLT_Three},
            {TRankLevelType.TRLT_Four.GetDescription(false), (int)TRankLevelType.TRLT_Four},
            {TRankLevelType.TRLT_Five.GetDescription(false), (int)TRankLevelType.TRLT_Five},
        };
    }
}
#endif
