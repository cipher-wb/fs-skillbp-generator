#if UNITY_EDITOR
using System.Collections.Generic;
using Funny.Base.Utils;
using Sirenix.Utilities.Editor;

namespace TableDR
{
    public partial class BuffConfig
    {
        /// <summary>
        /// 选择状态时，配置默认的buff类型
        /// </summary>
        private void OnValueChange_AddState()
        {
            if (AddState != TEntityState.TEST_NULL)
            {
                MetaBattleUnitStateConfig pMetaBattleUnitStateConfig = MetaBattleUnitStateConfigManager.Instance.GetItem(AddState);
                if (pMetaBattleUnitStateConfig != null)
                {
                    if (pMetaBattleUnitStateConfig.DefaultBuffTypeFlags != TBuffType.TBUFFT_NULL)
                        BuffTypeFlags = pMetaBattleUnitStateConfig.DefaultBuffTypeFlags;
                    if (pMetaBattleUnitStateConfig.DefaultBuffLevel > 0)
                        BuffLevel = pMetaBattleUnitStateConfig.DefaultBuffLevel;
                    if (!string.IsNullOrEmpty(pMetaBattleUnitStateConfig.DefaultBuffName))
                        BuffNameEditor = pMetaBattleUnitStateConfig.DefaultBuffName;
                    if (!string.IsNullOrEmpty(pMetaBattleUnitStateConfig.DefaultBuffDesc))
                        BuffDescEditor = pMetaBattleUnitStateConfig.DefaultBuffDesc;
                    if (!string.IsNullOrEmpty(pMetaBattleUnitStateConfig.DefaultBuffIcon))
                    {
                        Icon = pMetaBattleUnitStateConfig.DefaultBuffIcon;
                        IsShowIcon = true;
                    }
                }
            }
        }
    }
}
#endif