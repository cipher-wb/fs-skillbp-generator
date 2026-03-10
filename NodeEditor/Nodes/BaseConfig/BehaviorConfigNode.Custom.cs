using TableDR;

namespace NodeEditor
{
    public partial class BehaviorConfigNode
    {
        public static BehaviorConfig EmptyConfig = new BehaviorConfig();
        //public override bool ReadOnly => true;
        private string cacheType;
        protected override void Enable()
        {
            base.Enable();
            //BaseTransData ??= new BehaviorConfigTransData(this);
        }
        protected override void OnConfigChanged()
        {
            base.OnConfigChanged();

            if (cacheType != Config.Type)
            {
                cacheType = Config.Type;
                // 切换类型清空其他数据
                var props = Config.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (prop.SetMethod == null)
                    {
                        continue;
                    }
                    // 部分字段保留
                    if (prop.Name == nameof(Config.BaseID) ||
                        prop.Name == nameof(Config.Type))
                    {
                        continue;
                    }
                    var propValue = prop.GetValue(EmptyConfig);
                    prop.SetValue(Config, propValue);
                    this.OnPresetPropertyList();
                }
            }
            // 编辑器多语言读取，用编辑器数据直接覆写
            Config.ExSetValue(nameof(Config.LocStr1), Config.Loc1Editor);
            Config.ExSetValue(nameof(Config.LocStr2), Config.Loc2Editor);
            Config.ExSetValue(nameof(Config.LocStr3), Config.Loc3Editor);
        }
    }
}