using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_Dead : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        private bool IsEventEndGroup
        {
            get
            {
                var rootNode = baseNode?.GetPreviousNode<MapEventGeneralFuncGroupConfigNode>()?.GetPreviousNode<NpcEventConfigNode>();
                return rootNode != default;
            }
        }

        public MapEventGeneralFuncConfigNode_Dead(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 奖励对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("对象")]
        [OnValueChanged("OnDeadTargetsChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnDeadTargetsAdd")]
        public List<MapEventTarget> DeadTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnDeadTargetsAdd()
        {
            if (IsEventEndGroup)
            {
                return new MapEventTarget(MapEventTargetType.MapEventTargetType_SpecificActor);
            }
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnDeadTargetsChanged()
        {
            baseNode.SaveConfigTarget1(DeadTargets);

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetIsEmpty(DeadTargets);

            //结束通用功能只能使用指定演员类型
            if (IsEventEndGroup)
            {
                var isNotSpecificActor = DeadTargets?.Exists(target =>
                {
                    if(target.TargetType != MapEventTargetType.MapEventTargetType_SpecificActor)
                    {
                        return true;
                    }
                    return false;
                }) ?? false;

                if (isNotSpecificActor)
                {
                    baseNode.InspectorError += "【通用死亡】只能使用【指定演员类型】 \n";
                }
            }
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, DeadTargets);
        }

        public void SetDefault()
        {
            //Target1
            DeadTargets.Clear();
            DeadTargets.Add(OnDeadTargetsAdd());
            baseNode.SaveConfigTarget1(DeadTargets);
        }
    }
}
