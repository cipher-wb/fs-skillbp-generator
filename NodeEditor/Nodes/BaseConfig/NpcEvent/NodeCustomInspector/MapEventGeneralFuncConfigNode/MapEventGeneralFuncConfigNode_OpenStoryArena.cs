using Funny.Base.Utils;
using GraphProcessor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_OpenStoryArena : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_OpenStoryArena(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region 擂台中心NPC
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("擂台中心NPC")]
        [OnValueChanged("OnCenterNpcChanged", true), DelayedProperty]
        public MapEventTarget CenterNpc { get; private set; } = new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);

        private void OnCenterNpcChanged()
        {
            baseNode.SaveConfigTarget1(new List<MapEventTarget> { CenterNpc });

            CheckError();
        }
        #endregion

        #region 剧情擂台表ID
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("剧情擂台表ID")]
        [OnValueChanged("OnStoryArenaChanged", true), DelayedProperty]
        public TableSelectData StoryArenaTableData { get; private set; } = new TableSelectData(typeof(StoryArenaConfig).FullName, 0);

        private void OnStoryArenaChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { StoryArenaTableData.ID });

            CheckError();
        }
        #endregion

        public void ConfigToData()
        {
            //Target1
            List<MapEventTarget> targets = new List<MapEventTarget>();
            baseNode.RestoreTargets(baseNode.Config?.Target1, targets);
            CenterNpc = targets.NullOrEmpty() ? null : targets[0];

            //IntParams
            if (baseNode.Config?.IntParams1?.Count >= 1)
            {
                StoryArenaTableData = new TableSelectData(typeof(StoryArenaConfig).FullName, baseNode.Config?.IntParams1[0] ?? 0);
                StoryArenaTableData.OnSelectedID();
            }
        }

        public void SetDefault()
        {
            //Target1
            CenterNpc = new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
            baseNode.SaveConfigTarget1(new List<MapEventTarget> { CenterNpc });
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTableNotSelect(StoryArenaTableData);

            baseNode.AddInspectorErrorTargetOnlyOne(new List<MapEventTarget> { CenterNpc });

            if (!baseNode.IsLastNode())
            {
                baseNode.InspectorError += $"【必须是列表的最后一个】\n";
            }
        }
    }
}
