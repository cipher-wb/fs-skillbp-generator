namespace NodeEditor
{
    public partial class BuffConfigNode
    {
        protected override void OnPreset()
        {
            base.OnPreset();

            Config.ExSetValue(nameof(Config.IsTempAttrs), true);
            Config.ExSetValue(nameof(Config.IsTempSkillTags), true);
        }
        protected override void OnSave()
        {
            // 检查描述是否句号结尾，自动添加句号
            if (!string.IsNullOrEmpty(Config?.BuffDescEditor) && !Config.BuffDescEditor.EndsWith("。", System.StringComparison.Ordinal))
            {
                this.SetConfigValue(nameof(Config.BuffDescEditor), Config.BuffDescEditor + "。");
            }
            base.OnSave();
        }
        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                // buff名，buff描述信息务必需要配置，便于定位问题及理解
                if (string.IsNullOrEmpty(Config.BuffNameEditor))
                {
                    ret = false;
                    AppendSaveRet($"Buff配置缺失_buff名");
                }
                if (Config.IsShowIcon)
                {
                    if (string.IsNullOrEmpty(Config.BuffDescEditor))
                    {
                        ret = false;
                        AppendSaveRet($"Buff配置缺失_buff描述");
                    }
                    // 配置显示图标，但是图标未配置情况
                    if (string.IsNullOrEmpty(Config.Icon))
                    {
                        ret = false;
                        AppendSaveRet($"Buff配置缺失_图标");
                    }
                    if (Config.BuffTypeFlags == TableDR.TBuffType.TBUFFT_NULL)
                    {
                        ret = false;
                        AppendSaveRet($"Buff配置缺失_buff类型");
                    }
                }
                // buff关系配置错误
                switch (Config.DiffCoexistType)
                {
                    case TableDR.TBuffCoexistType.TBUFFCOEXISTT_COEXIST:
                        break;
                    case TableDR.TBuffCoexistType.TBUFFCOEXISTT_OVERLAY:
                        {
                            if (Config.SameCoexistType == TableDR.TBuffCoexistType.TBUFFCOEXISTT_COEXIST
                                || Config.SameCoexistType == TableDR.TBuffCoexistType.TBUFFCOEXISTT_RESERVE
                                || Config.SameCoexistType == TableDR.TBuffCoexistType.TBUFFCOEXISTT_REPLACE)
                            {
                                AppendSaveRet($"Buff配置错误_不同施法者buff关系为[叠加]时，同施法者buff关系不允许为[共存/保留/顶替]");
                            }
                        }
                        break;
                    case TableDR.TBuffCoexistType.TBUFFCOEXISTT_RESERVE:
                        {
                            if (Config.SameCoexistType == TableDR.TBuffCoexistType.TBUFFCOEXISTT_COEXIST)
                            {
                                AppendSaveRet($"Buff配置错误_不同施法者buff关系为[保留]时，同施法者buff关系不允许为[共存]");
                            }
                        }
                        break;
                    case TableDR.TBuffCoexistType.TBUFFCOEXISTT_REPLACE:
                        {
                            if (Config.SameCoexistType == TableDR.TBuffCoexistType.TBUFFCOEXISTT_COEXIST)
                            {
                                AppendSaveRet($"Buff配置错误_不同施法者buff关系为[顶替]时，同施法者buff关系不允许为[共存]");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return ret;
        }
    }
}
