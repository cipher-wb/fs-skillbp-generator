#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace TableDR
{
    public partial class SkillTagInfo
    {
        private IEnumerable<ValueDropdownItem> ValueDropdown_TagsValue => SkillTagsConfig.ValueDropdown_TagsValue();
        public static string GetTagDesc(IReadOnlyList<SkillTagInfo> tagsList, int index)
        {
            if (tagsList == null)
            {
                return string.Empty;
            }
            return GetTagDesc(tagsList.Select(skillTagInfo => skillTagInfo.SkillTagConfigID).ToList(), index);
        }

        public static string GetTagDesc(IReadOnlyList<int> tagConfigIdList, int index)
        {
            if (tagConfigIdList == null)
            {
                return string.Empty;
            }
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

        public static string GetTagDesc(int tagConfigId)
        {
            try
            {
                var tagConfig = SkillTagsConfigManager.Instance.GetItem(tagConfigId);
                return $"{tagConfig?.Desc ?? "错误：找不到对应SkillTagConfig"}";
            }
            catch
            {
                return $"错误：获取Tag描述异常";
            }
        }
        public string GetNodeViewDesc()
        {
            return $"[{GetTagDesc(SkillTagConfigID)}_{SkillTagConfigID}_{Value}]";
        }
    }
}
#endif