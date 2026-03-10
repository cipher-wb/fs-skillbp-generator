using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        private bool IsFormulaA => Config?.IsFormulaA() ?? false;
        private bool IsFormulaB => Config?.IsFormulaB() ?? false;
        private bool IsFormulaC => Config?.IsFormulaC() ?? false;
        private bool IsFormulaD => Config?.IsFormulaD() ?? false;
        private bool IsFormulaE => Config?.IsFormulaE() ?? false;

        private GameEntityType gameEntityType;

        private int subGameEntityType;

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][公式][{Utils.GetEnumDescription(conditionType)}]");
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            conditionType = Config?.ConditionType ?? MapEventConditionType.MECT_NULL;

            if(edges.Count == 0) { return; }

            var conditionNode = GetPreviousNode<MapEventConditionConfigNode>();
            if (conditionNode != null)
            {
                gameEntityType = conditionNode.Config?.GameEntityType ?? GameEntityType.ET_Null;
                subGameEntityType = (conditionNode.Config?.SubGameEntityType ?? 0);
            }

            if(gameEntityType == GameEntityType.ET_Null) { return; }

            //逻辑符号
            logicOp = Config?.LogicOp ?? TLogicOp.TLogicOp_And;

            //公式A
            if (IsFormulaA)
            {
                RestoreFormulaA();
            }
            //公式B
            else if (IsFormulaB)
            {
                //枚举
                RestoreFormulaB_Enum();

                //表格
                RestoreFormulaB_RefTable();
            }
            //公式C
            else if (IsFormulaC)
            {
                RestoreFormulaC();
            }
            //公式D
            else if (IsFormulaD)
            {
                RestoreFormulaD();
            }
            //公式E
            else if (IsFormulaE)
            {
                RestoreFormulaE();
            }
        }

        #region 条件
        [Sirenix.OdinInspector.ShowInInspector, LabelText("条件"), ValueDropdown("GetConditionType")]
        [BoxGroup("条件", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnConditionType", true)]
        private MapEventConditionType conditionType = MapEventConditionType.MECT_NULL;

        private void OnConditionType()
        {
            SetConfigValue(nameof(Config.ConditionType), conditionType);
            SetConfigValue(nameof(Config.FormulaA), null);
            SetConfigValue(nameof(Config.FormulaB), null);
            SetConfigValue(nameof(Config.FormulaC), null);
            SetConfigValue(nameof(Config.FormulaD), null);
            SetConfigValue(nameof(Config.FormulaE), null);
            SetConfigValue(nameof(Config.LogicOp), TLogicOp.TLogicOp_And);

            SetCustomName($"[公式][{Utils.GetEnumDescription(conditionType)}]");

            //表格引用类型，自动选择表格
            //if (IsRefType)
            //{
            //    AddDefaultTable();
            //}
        }
        
        /// <summary>
        /// 获取条件列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> GetConditionType()
        {
            List<MapEventConditionType> conditionTypeList = gameEntityType switch
            {
                GameEntityType.ET_StoryPlayer => VD_PlayerType,
                GameEntityType.TET_MRT_MONSTER => VD_MonsterType,
                GameEntityType.TET_MRT_PLANT => VD_PlantType,
                GameEntityType.TET_MRT_METAL => VD_MetalType,
                GameEntityType.TET_MRT_FISH => VD_FishType,
                GameEntityType.TET_MRT_BOX => VD_BoxType,
                GameEntityType.TET_MRT_LINGQITUAN => VD_LingQiType,
                GameEntityType.ET_Npc => VD_NpcType,
                _ => default,
            };

            if(conditionTypeList == default) { yield break; }

            foreach(var conditionType in conditionTypeList)
            {
                yield return new ValueDropdownItem($"{conditionType.GetDescription(false)}", conditionType);
            }
        }
        #endregion

        #region 逻辑符号
        [Sirenix.OdinInspector.ShowInInspector, LabelText("逻辑符号")]
        [BoxGroup("条件", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnChangedLogicOp", true)]
        private TLogicOp logicOp = TLogicOp.TLogicOp_And;

        private void OnChangedLogicOp()
        {
            SetConfigValue(nameof(Config.LogicOp), logicOp);

            SetCustomName($"[公式][{Utils.GetEnumDescription(conditionType)}]");
        }
        #endregion
    }
}
