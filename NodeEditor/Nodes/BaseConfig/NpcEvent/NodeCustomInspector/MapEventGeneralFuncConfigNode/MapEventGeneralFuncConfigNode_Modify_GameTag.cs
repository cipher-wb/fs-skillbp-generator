using CSGameShare.CSEffectAttribute;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_Modify_GameTag : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_Modify_GameTag(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 修改角色对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("修改对象")]
        [OnValueChanged("OnChangedModifyTargets", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddModifyTargets")]
        public List<MapEventTarget> ModifyTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAddModifyTargets()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnChangedModifyTargets()
        {
            baseNode.SaveConfigTarget1(ModifyTargets);

            CheckError();
        }
        #endregion

        #region 添加的游戏标签
        /// <summary>
        /// 添加的游戏标签
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker]
        [LabelText("添加游戏标签"), ValueDropdown("GetGameTagItem", DoubleClickToConfirm = true)]
        [OnValueChanged("OnChangedAddGameTagList", true)]
        public List<long> AddGameTagList { get; private set; } = new List<long>();

        private IEnumerable<ValueDropdownItem> GetGameTagItem()
        {
            foreach (var item in GameTagConfigManager.Instance.ItemArray.Items)
            {
                yield return new ValueDropdownItem($"{item.TagId}_{item.TagLevel}_{item.TagName}_{item.TagType.GetDescription(false)}", GameTagData.CombTagData(item.TagId, item.TagLevel));
            }
        }

        private void OnChangedAddGameTagList()
        {
            var specificItemTableDatas = new List<long>();
            AddGameTagList?.ForEach(gameTagID =>
            {
                specificItemTableDatas.Add(gameTagID);
            });

            baseNode.Config?.ExSetValue("LongParams1", specificItemTableDatas);

            CheckError();
        }
        #endregion

        #region 移除的游戏标签
        /// <summary>
        /// 添加的游戏标签
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker]
        [LabelText("移除游戏标签"), ValueDropdown("GetGameTagItem", DoubleClickToConfirm = true)]
        [OnValueChanged("OnChangedRemoveGameTagList", true)]
        public List<long> RemoveGameTagList { get; private set; } = new List<long>();

        private void OnChangedRemoveGameTagList()
        {
            var longParams2 = new List<long>();
            RemoveGameTagList?.ForEach(gameTagID =>
            {
                longParams2.Add(gameTagID);
            });

            baseNode.Config?.ExSetValue("LongParams2", longParams2);

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetOnlyOne(ModifyTargets);

            if(AddGameTagList.Count == 0 && RemoveGameTagList.Count == 0)
            {
                baseNode.InspectorError += "【缺少添加、移除的标签】";
            }
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, ModifyTargets);

            //LongParams1
            AddGameTagList.Clear();
            baseNode.Config.LongParams1?.ForEach(gameTagID =>
            {
                AddGameTagList.Add(gameTagID);
            });

            //LongParams2
            RemoveGameTagList.Clear();
            baseNode.Config.LongParams2?.ForEach(gameTagID =>
            {
                RemoveGameTagList.Add(gameTagID);
            });
        }

        public void SetDefault()
        {
            ModifyTargets.Clear();
            ModifyTargets.Add(OnAddModifyTargets());
            baseNode.SaveConfigTarget1(ModifyTargets);
        }
    }
}
