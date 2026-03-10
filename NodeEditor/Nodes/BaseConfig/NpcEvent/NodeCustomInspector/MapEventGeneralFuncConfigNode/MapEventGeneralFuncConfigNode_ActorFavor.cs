using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    /// <summary>
    /// MEGFT_ActorFavor
    /// </summary>
    public class MapEventGeneralFuncConfigNode_ActorFavor : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_ActorFavor(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 改变对象列表
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("改变对象列表")]
        [OnValueChanged("OnActorFavorTargetsChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnActorFavorTargetsAdd")]
        public List<MapEventTarget> ActorFavorTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnActorFavorTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_AllCostar);
        }

        private void OnActorFavorTargetsChanged()
        {
            baseNode.SaveConfigTarget1(ActorFavorTargets);

            CheckError();
        }
        #endregion

        #region Target2 相对对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("相对对象")]
        [OnValueChanged("OnActorFavorRelativeTargetChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnActorFavorRelativeTargetAdd")]
        public List<MapEventTarget> ActorFavorRelativeTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnActorFavorRelativeTargetAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Player);
        }

        private void OnActorFavorRelativeTargetChanged()
        {
            baseNode.SaveConfigTarget2(ActorFavorRelativeTargets);

            CheckError();
        }
        #endregion

        #region 好感度设置
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("好感度")]
        [OnValueChanged("OnChangeActorFavor", true), DelayedProperty]
        public ChangeFavorData ActorFavorData { get; private set; }

        private void OnChangeActorFavor()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int>()
            {
                (int)ActorFavorData.ChangeType,
                ActorFavorData.ChangeValue,
            });

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorFavor(ActorFavorData);

            baseNode.AddInspectorErrorTargetOnlyOne(ActorFavorRelativeTargets);

            baseNode.AddInspectorErrorTargetSame(ActorFavorTargets, ActorFavorRelativeTargets);

            ActorFavorTargets?.ForEach(target =>
            {
                if(target.TargetType == MapEventTargetType.MapEventTargetType_Player)
                {
                    baseNode.InspectorError += "【改变对象列表 为 玩家】 \n";
                }
            });
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, ActorFavorTargets);

            //Target2
            baseNode.RestoreTargets(baseNode.Config?.Target2, ActorFavorRelativeTargets);

            //IntParams1
            if (baseNode.Config?.IntParams1?.Count == 2)
            {
                ActorFavorData = new ChangeFavorData((SymbolType)baseNode.Config.IntParams1[0], baseNode.Config.IntParams1[1]);
            }
        }

        public void SetDefault()
        {
            ActorFavorTargets.Clear();
            ActorFavorTargets.Add(OnActorFavorTargetsAdd());
            baseNode.SaveConfigTarget1(ActorFavorTargets);

            ActorFavorRelativeTargets.Clear();
            ActorFavorRelativeTargets.Add(OnActorFavorRelativeTargetAdd());
            baseNode.SaveConfigTarget2(ActorFavorRelativeTargets);

            ActorFavorData = new ChangeFavorData(SymbolType.Add, 10);
            OnChangeActorFavor();
        }
    }
}
