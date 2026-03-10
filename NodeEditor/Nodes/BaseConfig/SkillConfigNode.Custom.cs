using CSGameShare.Hotfix.CSBattleShare;
using GameApp;
using GameApp.Editor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEditor;
using Funny.Base.Utils;

namespace NodeEditor
{
    public partial class SkillConfigNode
    {
        // 初始技能参数数据表格
        private bool isCacheSkillValueConfig = false;
        private SkillValueConfig skillValueConfig = null;
        protected override void OnPreset()
        {
            base.OnPreset();

            // 给默认值
            Config.ExSetValue(nameof(Config.SmartCastTargetBasePriority), TCastTargetBasePriority.TCTBC_DISTANCE_NEAR);
            Config.ExSetValue(nameof(Config.SmartCastTargetCondTemplate), 1);
            Config.ExSetValue(nameof(Config.CastTargetCondTemplate), 1);
            Config.ExSetValue(nameof(Config.DamageType), TSkillDamageType.TSDT_ZHI_JIE);
            Config.ExSetValue(nameof(Config.BDLabels), new List<int>());

            Config.OnValueChange_SmartCastTargetCondTemplate();
            Config.OnValueChange_CastTargetCondTemplate();
        }

        protected override void Disable()
        {
            // Unity诡异bug，可能存在Recompile Assembly后，缓存值错乱
            isCacheSkillValueConfig = false;
            skillValueConfig = null;
            base.Disable();
        }

        protected override void OnConfigChanged()
        {
            base.OnConfigChanged();
        }

        public override bool OnPostProcessing()
        {
            var baseResult = base.OnPostProcessing();
            try
            {
                // TODO 后续看下怎么避免对象新建Odin避免派生类型显示， 暂时处理列表为创建情况，避免List创建选择错误类型
                OnPresetPropertyList();
                // 查找相关技能数据表
                skillValueConfigs.Clear();
                if (SkillValueConfigManager.Instance.IsLoadedData)
                {
                    // 最大等级按照30查找
                    for (int i = 1; i <= 30; i++)
                    {
                        var skillValueConfig = SkillTableManager.GetSkillValueConfig(AppFacade.DesignTableManager, ID, i);
                        if (skillValueConfig != null)
                        {
                            skillValueConfigs.Add(skillValueConfig);
                        }
                        else
                        {
                            // 未找到下一连续等级数据，直接返回
                            break;
                        }
                    }
                }
                skillDescConfigs.Clear();
                if (SkillDescConfigManager.Instance.IsLoadedData)
                {
                    // 最大等级按照30查找
                    for (int i = 1; i <= 30; i++)
                    {
                        var skillDescConfig = SkillDescConfigManager.Instance.GetItem(ID, i);
                        if (skillDescConfig != null)
                        {
                            skillDescConfigs.Add(skillDescConfig);
                        }
                        else
                        {
                            // 未找到下一连续等级数据，直接返回
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"{GetLogPrefix()} 技能节点后处理异常!\n{ex}");
            }
            return baseResult;
        }

        public override bool OnSaveCheck()
        {
            var ret = base.OnSaveCheck();
            if (ret)
            {
                // 检查技能时长相关
                bool isNeedCheck = true;
                if (Config.SkillMainType == TBattleSkillMainType.TBSMT_NULL || Config.SkillSubType == TBattleSkillSubType.TBSST_NULL)
                {
                    ret &= false;
                    AppendSaveRet($"技能类型未配置");
                }
                switch (Config.SkillSubType)
                {
                    case TBattleSkillSubType.TBSST_GONG_JI_FA_BAO:
					case TBattleSkillSubType.TBSST_FANG_YU_FA_BAO:
					case TBattleSkillSubType.TBSST_CHUAN_CHENG_XIN_FA:
                    case TBattleSkillSubType.TBSST_PU_TONG_XIN_FA:
                    case TBattleSkillSubType.TBSST_NULL:
                        isNeedCheck = false;
                        break;
                }
                bool isNeedCheckCD = true;
                if (Config.IsContinueUseSkill() && Config.CdType == TSkillColdType.TSCDT_AFTER_SKILL_END)
                {
                    // 持续使用类型技能 并且 技能结束后CD，那么不检查CD
                    isNeedCheckCD = false;
                }
                if (isNeedCheck && !Config.IsPassiveSkill())
                {
                    // 前摇<=缓冲区<=基础时长<=CD
                    bool isValidTime = 
                        Config.SkillCastFrame <= Config.SkillBufferFrame && 
                        Config.SkillBufferFrame <= Config.SkillBaseDuration &&
                        (!isNeedCheckCD || Config.SkillBaseDuration <= Config.CdTime);
                    if (Config.SkillCastFrame > 0 && Config.SkillCastFrame == Config.SkillBaseDuration)
                    {
                        // 如果是将前摇当作持续施法时间来用，当前摇跟基础时长一致情况下，不报错
                        isValidTime = true;
                    }
                    if (isValidTime && (Config.CdType == TSkillColdType.TSCDT_COMBO|| Config.CdType == TSkillColdType.TSCDT_COMBO_SKILL_END))
                    {
                        // 连招检查
                        for (int i = 0; i < Config.ComboCdList.Count; ++i)
                        {
                            var comboCD = Config.ComboCdList[i];
                            bool isNeedCheckDuration = true;
                            // 如果是技能结束进CD，必须最后一段的时间为0，否则可能触发无限放技能
                            if (Config.CdType == TSkillColdType.TSCDT_COMBO_SKILL_END && Config.ComboCdList.Count - 1 == i)
                            {
                                isNeedCheckDuration = false;
                                if (comboCD.BaseDuration != 0)
                                {
                                    AppendSaveRet($"技能时间不符合规则_[连招且技能结束后CD-该模式下，最后一段基础时长必须为0，自动修改{comboCD.BaseDuration}->{0}]");
                                    comboCD.ExSetValue(nameof(comboCD.BaseDuration), 0);
                                }
                            }
                            if (!(comboCD.CastFrame <= comboCD.BufferFrame &&
                                (!isNeedCheckDuration || comboCD.BufferFrame <= comboCD.BaseDuration) && 
                                (!isNeedCheckCD || !isNeedCheckDuration || comboCD.BaseDuration <= comboCD.CDTime)))
                            {
                                isValidTime = false;
                                break;
                            }
                        }
                    }
                    if (!isValidTime)
                    {
                        ret &= false;
                        AppendSaveRet($"技能时间不符合规则_[前摇<=缓冲区<=基础时长<=CD]");
                    }
                }
                // 检查AI施法距离
                if (isNeedCheck && ((Config.SkillRange == default && Config.SkillRangeTagConfigId == default) || Config.AISkillRange == default))
                {
                    ret &= false;
                    AppendSaveRet($"技能配置缺失_（AI）施法范围");
                }
                // 检查解控配置是否正确配置标记
                if (Config.SkillAITags?.Count > 0)
                {
                    if (Config.SkillAITags.Contains(TableDR.TBattleSkillAITag.TBSAT_CONTROL_RELEASE) && !Config.SkillProperty.HasFlag(TableDR.TBattleSkillProperty.TBSP_IGNORE_SKILL_FORBIDDEN))
                    {
                        ret &= false;
                        AppendSaveRet($"技能配置错误_AI标签-解控_需配置技能特性-无视禁止施法");
                    }
                }
                // 如果存在数据配置
                if (!isCacheSkillValueConfig && SkillValueConfigManager.Instance.IsLoadedData && AppEditorFacade.DesignTable != null)
                {
                    isCacheSkillValueConfig = true;
                    skillValueConfig = SkillTableManager.GetSkillValueConfig(AppEditorFacade.DesignTable, ID, 1);
                }
                // 检查SkillValueConfig配置
                if (isCacheSkillValueConfig && skillValueConfig != null)
                {
                    // 存在诡异问题，可能存在缓存SkillValueConfig数据为空对象
                    if (skillValueConfig.SkillConfigID == ID && skillValueConfig.SkillLevel == 1)
                    {
                        // 检查技能参数配置与SkillValueConfig配置是否统一
                        if (skillValueConfig.SkillTagsList == null && Config.SkillTagsList == null)
                        {
                            ret &= true;
                        }
                        else if (skillValueConfig.SkillTagsList != null && Config.SkillTagsList != null)
                        {
                            ret &= IsValidTagList(Config.SkillDamageTagsList, skillValueConfig.SkillDamageTagsList);
                            ret &= IsValidTagList(Config.SkillTagsList, skillValueConfig.SkillTagsList);
                            ret &= IsValidTagList(Config.SkillTipsConditionSkillTagsList, skillValueConfig.SkillTipsConditionSkillTagsList);
                        }
                        else
                        {
                            ret &= false;
                        }
                    }
                    else
                    {
                        if (LocalSettings.IsProgramer())
                        {
                            Log.Error($"SkillValueConfig 未知原因出现数据清空情况, ID:{ID}");
                        }
                    }
                }
            }
            return ret;
        }

        protected override void OnSave()
        {
            // 检查描述是否句号结尾，自动添加句号
            if (!string.IsNullOrEmpty(Config?.SkillDescEditor) && !Config.SkillDescEditor.EndsWith("。", System.StringComparison.Ordinal))
            {
                this.SetConfigValue(nameof(Config.SkillDescEditor), Config.SkillDescEditor + "。");
            }
            Config.UpdateIndicatorTypeConfig();
            base.OnSave();
        }

        /// <summary>
        /// 检查技能参数列表配置是否统一，SkillConfig->SkillValueConfig
        /// </summary>
        private bool IsValidTagList(IReadOnlyList<SkillTagInfo> skillTagInfos,  IReadOnlyList<SkillTagInfo> skillValueTagInfos)
        {
            if (skillTagInfos == null && skillValueTagInfos == null)
            {
                return true;
            }
            else if (skillTagInfos != null && skillValueTagInfos != null)
            {
                var ret = true;
                foreach (var skillTagInfo in skillTagInfos)
                {
                    // 数值表存在配置
                    if (skillValueTagInfos.Find((t) => { return t.SkillTagConfigID == skillTagInfo.SkillTagConfigID; }) != null)
                    {
                        continue;
                    }
                    ret = false;
                    var skillTagConfig = DesignTable.GetTableCell<SkillTagsConfig>(skillTagInfo.SkillTagConfigID);
                    if (skillTagConfig != null)
                    {
                        AppendSaveRet($"SkillValueConfig缺少技能参数:{skillTagConfig.Desc}_{skillTagConfig.ID}");
                    }
                    else
                    {
                        AppendSaveRet($"SkillTagConfig未配置_ID:{skillTagInfo.SkillTagConfigID}");
                    }
                }
                if (skillTagInfos.Count != skillValueTagInfos.Count)
                {
                    AppendSaveRet($"技能参数与SkillValueConfig个数不一致:{skillTagInfos.Count}_{skillValueTagInfos.Count}");
                    ret = false;
                }
                return ret;
            }
            return false;
        }

        // 编辑器扩展
        [InfoBox("\n技能多等级数据预览\nSkillValueConfig,SkillDescConfig等", InfoMessageType.Warning)]
        [FoldoutGroup("技能数据", true, 1)]
        [Button("点击-导出数据到Excel"), FoldoutGroup("技能数据/SkillValueConfig", false, 1)]
        private void ExportSkillValueConfigToExcel()
        {
            var title = "导出-SkillValueConfig";
            foreach (var excelName in SkillValueConfigManager.Excels)
            {
                var excelPath = $"{Constants.ExcelPathPrefix}/{excelName}";
                while (Utils.IsFileOpened(excelPath))
                {
                    var displayDialogInfo = $"【{excelName}】表格处于被打开状态/只读模式，请先关闭！";
                    if (EditorUtility.DisplayDialog(title, displayDialogInfo, "已关闭，继续导出", "不导出了"))
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (!Utils.IsFileOpened(excelPath))
                {
                    Utils.DisplayProcess(title, (_) =>
                    {
                        ExcelManager.Inst.ModifyExcelCell(excelPath, skillValueConfigs.ToList<object>());
                    });
                }
            }
        }

        [LabelText("多等级列表-技能数据表-SkillValueConfig"), FoldoutGroup("技能数据/SkillValueConfig", false, 1), ShowInInspector, TableList(AlwaysExpanded = true, IsReadOnly = true, HideToolbar = true)/*, ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true, DraggableItems = false, ListElementLabelName = "SkillLevel")*/]
        private List<SkillValueConfig> skillValueConfigs = new List<SkillValueConfig>();

        [LabelText("多等级列表-技能描述表-SkillDescConfig"), FoldoutGroup("技能数据/SkillDescConfig", false, 1), ShowInInspector, TableList(AlwaysExpanded = true, IsReadOnly = true, HideToolbar = true)/*, ListDrawerSettings(Expanded = true, HideAddButton = true, HideRemoveButton = true, DraggableItems = false, ListElementLabelName = "SkillLevel")*/]
        private List<SkillDescConfig> skillDescConfigs = new List<SkillDescConfig>();
    }
}
