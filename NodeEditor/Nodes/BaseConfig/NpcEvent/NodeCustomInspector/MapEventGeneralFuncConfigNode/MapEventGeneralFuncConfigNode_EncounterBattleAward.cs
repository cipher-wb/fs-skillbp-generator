using GraphProcessor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_EncounterBattleAward : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_EncounterBattleAward(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 奖励对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("奖励对象")]
        [OnValueChanged("OnAwardDropTargetsChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAwardDropTargetsAdd")]
        public List<MapEventTarget> AwardDropTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAwardDropTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Player);
        }

        private void OnAwardDropTargetsChanged()
        {
            baseNode.SaveConfigTarget1(AwardDropTargets);

            CheckError();
        }
        #endregion

        #region 奖励掉落
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("奖励-天")]
        [OnValueChanged("OnChangedEncounterBattleAwardData", true)]
        public EncounterBattleAwardData GradeTianData = new EncounterBattleAwardData(TBattleMissionGradeType.TBMGT_TIAN, 0, 0);

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("奖励-地")]
        [OnValueChanged("OnChangedEncounterBattleAwardData", true)]
        public EncounterBattleAwardData GradeDiData = new EncounterBattleAwardData(TBattleMissionGradeType.TBMGT_DI, 0, 0);

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("奖励-凡")]
        [OnValueChanged("OnChangedEncounterBattleAwardData", true)]
        public EncounterBattleAwardData GradeFanData = new EncounterBattleAwardData(TBattleMissionGradeType.TBMGT_FAN, 0, 0);
        private void OnChangedEncounterBattleAwardData()
        {
            var dyList = new List<GFDynamic>();
            dyList.Add(GradeTianData.ToGFDynamic());
            dyList.Add(GradeDiData.ToGFDynamic());
            dyList.Add(GradeFanData.ToGFDynamic());
            baseNode.Config?.ExSetValue("DynamicClass1", dyList);

            CheckError();
        }
        #endregion

        #region 掉落类型
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落类型")]
        [OnValueChanged("OnDropTypeChanged", true), DelayedProperty]
        public TDropInfoPushType DropType { get; private set; } = TDropInfoPushType.TD_NoTips;

        private void OnDropTypeChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { (int)DropType, (int)AwardCountType });

            CheckError();
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落类型")]
        [OnValueChanged("OnDropTypeChanged", true), DelayedProperty]
        public AwardCountType AwardCountType { get; private set; } = AwardCountType.One;

        private void OnAwardTypeChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { (int)DropType, (int)AwardCountType });

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            //检测对象
            baseNode.AddInspectorErrorTargetOnlyOne(AwardDropTargets);

            //检测奖励
            baseNode.AddInspectorErrorEncounterBattleAwardData(GradeTianData, "奖励-天");
            baseNode.AddInspectorErrorEncounterBattleAwardData(GradeDiData, "奖励-地");
            baseNode.AddInspectorErrorEncounterBattleAwardData(GradeFanData, "奖励-凡");

            //检测掉落
            if (baseNode.Config.IntParams1.Count != 2)
            {
                baseNode.InspectorError += $"掉落参数错误 \n";
            }

            var dropType = baseNode.Config.IntParams1.Count > 0 ? (TDropInfoPushType)baseNode.Config.IntParams1[0] : TDropInfoPushType.TD_NoTips;
            baseNode.AddInspectorErrorDropType(dropType);
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, AwardDropTargets);

            //IntParams1
            if (baseNode?.Config?.IntParams1?.Count > 0)
            {
                DropType = (TDropInfoPushType)baseNode.Config.IntParams1[0];
            }

            //IntParams1
            if (baseNode?.Config?.IntParams1?.Count > 1)
            {
                AwardCountType = (AwardCountType)baseNode.Config.IntParams1[1];
            }

            //DynamicClass1
            for (int i = 0; i < baseNode.Config?.DynamicClass1?.Count; i++)
            {
                var gfDynamic = baseNode.Config.DynamicClass1[i];
                if (i == 0)
                {
                    GradeTianData = new EncounterBattleAwardData((TBattleMissionGradeType)gfDynamic.DynmaicInt1, gfDynamic.DynmaicInt2, gfDynamic.DynmaicInt3);
                }
                else if(i == 1)
                {
                    GradeDiData = new EncounterBattleAwardData((TBattleMissionGradeType)gfDynamic.DynmaicInt1, gfDynamic.DynmaicInt2, gfDynamic.DynmaicInt3);
                }
                else if(i == 2)
                {
                    GradeFanData = new EncounterBattleAwardData((TBattleMissionGradeType)gfDynamic.DynmaicInt1, gfDynamic.DynmaicInt2, gfDynamic.DynmaicInt3);
                }
            }
        }

        public void SetDefault()
        {
            AwardDropTargets.Clear();
            AwardDropTargets.Add(OnAwardDropTargetsAdd());
            baseNode.SaveConfigTarget1(AwardDropTargets);
        }
    }
}
