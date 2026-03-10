using CSGameShare.CSEffectAttribute;
using GraphProcessor;
using HotFix.Game.Quest;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;
using static NodeEditor.MapEventFormulaConfigNode;

namespace NodeEditor
{
    /// <summary>
    /// 种族类
    /// </summary>
    public class DynamicRace
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("种族")]
        public TRaceType RaceType { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类"), ShowIf("@RaceType == TRaceType.TRT_MONSTER")]
        public TRaceSubType RaceSubType { get; private set; }

        public DynamicRace(GFDynamic gfDynamic)
        {
            RaceType = (TRaceType)gfDynamic.DynmaicInt1;
            RaceSubType = (TRaceSubType)gfDynamic.DynmaicInt2;
        }

        public DynamicRace(TRaceType raceType, TRaceSubType raceSubType)
        {
            RaceType = raceType;
            RaceSubType = raceSubType;
        }

        public GFDynamic ToGFDynamic()
        {
            return new GFDynamic((int)RaceType, (int)RaceSubType);
        }
    }

    /// <summary>
    /// 变量设置
    /// </summary>
    public class DynamicVariable
    {
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("变量ID")]
        public TableSelectData VariableData { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("操作符")]
        public TNpcMatchRuleType SymbolType { get; private set; }

        public bool IsSpace => SymbolType == TNpcMatchRuleType.TNMRT_SPACE;

        [Sirenix.OdinInspector.ShowInInspector, LabelText("变量值1")]
        public int VariableValue1 { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("变量值2"), ShowIf("@IsSpace == true")]
        public int VariableValue2 { get; private set; }

        public DynamicVariable(GFDynamic gfDynamic)
        {
            VariableData = new TableSelectData(typeof(TagConfig).FullName, gfDynamic.DynmaicInt1);
            VariableData.OnSelectedID();

            SymbolType = (TNpcMatchRuleType)gfDynamic.DynmaicInt2;
            VariableValue1 = gfDynamic.DynmaicInt3;
            VariableValue2 = gfDynamic.DynmaicInt4;
        }

        public DynamicVariable(int variableID, TNpcMatchRuleType symbolType, int value1, int value2)
        {
            VariableData = new TableSelectData(typeof(TagConfig).FullName, 0);
            SymbolType = symbolType;
            VariableValue1 = value1;
            VariableValue2 = value2;
        }

        public GFDynamic ToGFDynamic()
        {
            return new GFDynamic((int)VariableData.ID, (int)SymbolType, VariableValue1, VariableValue2);
        }
    }

    /// <summary>
    /// 行省设置
    /// </summary>
    public class DynamicExploration
    {
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("变量ID")]
        public TableSelectData VariableData { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("操作符")]
        public TNpcMatchRuleType SymbolType { get; private set; }

        public bool IsSpace => SymbolType == TNpcMatchRuleType.TNMRT_SPACE;

        [Sirenix.OdinInspector.ShowInInspector, LabelText("变量值1")]
        public int VariableValue1 { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("变量值2"), ShowIf("@IsSpace == true")]
        public int VariableValue2 { get; private set; }

        public DynamicExploration(GFDynamic gfDynamic)
        {
            VariableData = new TableSelectData(typeof(ProvinceConfig).FullName, gfDynamic.DynmaicInt1);
            VariableData.OnSelectedID();

            SymbolType = (TNpcMatchRuleType)gfDynamic.DynmaicInt2;
            VariableValue1 = gfDynamic.DynmaicInt3;
            VariableValue2 = gfDynamic.DynmaicInt4;
        }

        public DynamicExploration(int variableID, TNpcMatchRuleType symbolType, int value1, int value2)
        {
            VariableData = new TableSelectData(typeof(ProvinceConfig).FullName, 0);
            SymbolType = symbolType;
            VariableValue1 = value1;
            VariableValue2 = value2;
        }

        public GFDynamic ToGFDynamic()
        {
            return new GFDynamic((int)VariableData.ID, (int)SymbolType, VariableValue1, VariableValue2);
        }
    }

    /// <summary>
    /// 玩家功能是否解锁
    /// </summary>
    public class DynamicPlayerFuncUnlock
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("玩家功能")]
        public TFuncUnlockType UnlockType { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("是否解锁")]
        public bool IsUnlock { get; private set; }

        public DynamicPlayerFuncUnlock(GFDynamic gfDynamic)
        {
            UnlockType = (TFuncUnlockType)gfDynamic.DynmaicInt1;
            IsUnlock = gfDynamic.DynmaicInt2 == 0 ? false : true ;
        }

        public DynamicPlayerFuncUnlock(TFuncUnlockType unlockType, bool isUnlock)
        {
            UnlockType = unlockType;
            IsUnlock = isUnlock;
        }

        public GFDynamic ToGFDynamic()
        {
            return new GFDynamic((int)UnlockType, IsUnlock ? 1 : 0);
        }
    }

    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        /// <summary>
        /// 转换列表
        /// </summary>
        private List<GFDynamic> converDynamicIntList = new List<GFDynamic>();

        private void OnDynamicIntChanged()
        {
            converDynamicIntList.Clear();

            if (IsShowRace)
            {
                dynamicRaceList?.ForEach(gfDynamic => {
                    converDynamicIntList.Add(gfDynamic.ToGFDynamic());
                });
            }
            else if (IsShowVariable)
            {
                variableList?.ForEach(gfDynamic => {
                    converDynamicIntList.Add(gfDynamic.ToGFDynamic());
                });
            }
            else if (IsShowExploration)
            {
                explorationList?.ForEach(gfDynamic => {
                    converDynamicIntList.Add(gfDynamic.ToGFDynamic());
                });
            }
            else if (IsShowFuncUnlock)
            {
                playerFuncUnlockList?.ForEach(gfDynamic => {
                    converDynamicIntList.Add(gfDynamic.ToGFDynamic());
                });
            }

            SetConfigValue(nameof(Config.FormulaE), converDynamicIntList);
        }

        /// <summary>
        /// 恢复数据
        /// </summary>
        private void RestoreFormulaE()
        {
            if (IsShowRace)
            {
                dynamicRaceList.Clear();

                Config?.FormulaE?.ForEach(gfDynamic =>
                {
                    dynamicRaceList.Add(new DynamicRace(gfDynamic));
                });
            }
            if (IsShowVariable)
            {
                variableList.Clear();

                Config?.FormulaE?.ForEach(gfDynamic =>
                {
                    variableList.Add(new DynamicVariable(gfDynamic));
                });
            }
            if (IsShowExploration)
            {
                explorationList.Clear();

                Config?.FormulaE?.ForEach(gfDynamic =>
                {
                    explorationList.Add(new DynamicExploration(gfDynamic));
                });
            }
            if (IsShowFuncUnlock)
            {
                playerFuncUnlockList.Clear();

                Config?.FormulaE?.ForEach(gfDynamic =>
                {
                    playerFuncUnlockList.Add(new DynamicPlayerFuncUnlock(gfDynamic));
                });
            }
        }

        #region 种族
        /// <summary>
        /// 种族 人妖魔 妖族可选子类型
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("种族")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnDynamicIntChanged", true), ShowIf("@IsShowRace")]
        [ListDrawerSettings(CustomAddFunction = "OnAddDynamicRace")]
        private List<DynamicRace> dynamicRaceList = new List<DynamicRace>();

        private bool IsShowRace => Config?.ConditionType == MapEventConditionType.MECT_RACE;

        private DynamicRace OnAddDynamicRace()
        {
            return new DynamicRace(TRaceType.TRT_NONE, TRaceSubType.TRST_NONE);
        }
        #endregion

        #region 任务变量
        [Sirenix.OdinInspector.ShowInInspector, LabelText("任务变量")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnDynamicIntChanged", true), ShowIf("@IsShowVariable")]
        [ListDrawerSettings(CustomAddFunction = "OnAddDynamicVariable")]
        private List<DynamicVariable> variableList = new List<DynamicVariable>();

        private bool IsShowVariable => Config?.ConditionType == MapEventConditionType.MECT_VARIABLE;

        private DynamicVariable OnAddDynamicVariable()
        {
            return new DynamicVariable(0, TNpcMatchRuleType.TNMRT_EQUAL, 0, 0);
        }
        #endregion

        #region 探索度
        [Sirenix.OdinInspector.ShowInInspector, LabelText("探索度")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnDynamicIntChanged", true), ShowIf("@IsShowExploration")]
        [ListDrawerSettings(CustomAddFunction = "OnAddDynamicExploration")]
        private List<DynamicExploration> explorationList = new List<DynamicExploration>();

        private bool IsShowExploration => Config?.ConditionType == MapEventConditionType.MECT_MAP_EXPLORATION;

        private DynamicExploration OnAddDynamicExploration()
        {
            return new DynamicExploration(0, TNpcMatchRuleType.TNMRT_LESS_EQUAL, 0, 0);
        }
        #endregion

        #region 玩家功能解锁
        [Sirenix.OdinInspector.ShowInInspector, LabelText("玩家功能解锁")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnDynamicIntChanged", true), ShowIf("@IsShowFuncUnlock")]
        [ListDrawerSettings(CustomAddFunction = "OnAddDynamicPlayerFuncUnlock")]
        private List<DynamicPlayerFuncUnlock> playerFuncUnlockList = new List<DynamicPlayerFuncUnlock>();

        private bool IsShowFuncUnlock => Config?.ConditionType == MapEventConditionType.MECT_PLAYER_FUNC_UNLOCK;

        private DynamicPlayerFuncUnlock OnAddDynamicPlayerFuncUnlock()
        {
            return new DynamicPlayerFuncUnlock(TFuncUnlockType.TFUT_UNDEFINE, false);
        }
        #endregion
    }
}
