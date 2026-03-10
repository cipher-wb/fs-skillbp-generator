namespace NodeEditor
{
    public partial class SkillTagsConfigNode
    {
        protected override void Enable()
        {
            base.Enable();
            if (createdFromDuplication && !string.IsNullOrEmpty(Config?.Desc))
            {
                // 拷贝的节点，需要将描述做下差异，避免复制后命名一致，导致导表错误
                SetConfigValue(nameof(Config.Desc), $"{Config.Desc}_[{ID}]", true);
            }
        }
        protected override void OnConfigChanged()
        {
            base.OnConfigChanged();
            // 实时同步节点，每次修改都需要同步数据
            DesignTable.UpdateConfigData(Config, true);
        }

        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                if (!string.IsNullOrEmpty(Config.Desc) && Config.Desc.EndsWith($"_[{ID}]", System.StringComparison.Ordinal))
                {
                    AppendSaveRet("请重命名技能参数描述");
                    ret = false;
                }
            }
            return ret;
        }
    }
}
