using GraphProcessor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using TableDR;

namespace NodeEditor
{
    public partial class TSKILLSELECT_ENTITY_GROUP
    {
        protected override void OnPreset()
        {
            base.OnPreset();

            // 单位组本身自定义加入
            // 默认不配置，全部通过
            Config.ExSetValue(nameof(Config.SpecialSkillSelectFlag), TableDR.TSpecialSkillSelectFlag.TSSSF_None);
            Config.ExSetValue(nameof(Config.EntityTypeFilters), null);
        }
    }
}
