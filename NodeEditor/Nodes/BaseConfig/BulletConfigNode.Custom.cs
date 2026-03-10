using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class BulletConfigNode
    {
        protected override void OnPreset()
        {
            base.OnPreset();
            // 默认给予时间上限限制
            Config.ExSetValue(nameof(Config.LifeFlag), BulletConfig_TBulletLifeFlag.TBLT_TIME);
            Config.ExSetValue(nameof(Config.PhysicalReflectCount), 1);
            Config.ExSetValue(nameof(Config.Model), 4);
        }
    }
}
