#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.Linq;

namespace TableDR
{
    public partial class SkillTagsConfig
    {
        public static ValueDropdownAttribute VD_TagsValue = new ValueDropdownAttribute("ValueDropdown_TagsValue")
        {
            DropdownTitle = "选择技能参数：",
            ExpandAllMenuItems = false,
            AppendNextDrawer = true,
            DoubleClickToConfirm = true,
        };
        public static IEnumerable<ValueDropdownItem> ValueDropdown_TagsValue()
        {
            // TODO 考虑未导出表格数据
            var tags = SkillTagsConfigManager.Instance.ItemArray.Items;
            foreach (var tagConfig in tags)
            {
                yield return new ValueDropdownItem($"{tagConfig.TagType.GetDescription()}/{tagConfig.ID}-{tagConfig.Desc}", tagConfig.ID);
            }
        }
        public static string GetTagDesc(List<SkillTagInfo> tagsList, int index)
        {
            return GetTagDesc(tagsList.Select(skillTagInfo => skillTagInfo.SkillTagConfigID).ToList(), index);
        }

        public static string GetTagDesc(List<int> tagConfigIdList, int index)
        {
            return GetTagDesc(tagConfigIdList.GetAt(index), index);
        }

        public static string GetTagDesc(int tagConfigId, int index)
        {
            try
            {
                var tagConfig = SkillTagsConfigManager.Instance.GetItem(tagConfigId);
                return $"{index + 1}-{tagConfig?.Desc ?? "错误TagID"}";
            }
            catch
            {
                return $"{index + 1}-获取Tag值异常";
            }
        }
    }
}
#endif