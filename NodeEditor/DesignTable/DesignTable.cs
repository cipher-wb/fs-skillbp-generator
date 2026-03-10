#if !NodeExport
using CSGameShare.Hotfix.CSBattleShare;
#endif
using GameApp;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 编辑器表格数据管理
    /// </summary>
    public static partial class DesignTable
    {
        #region 节点编辑器相关表格数据，TODO 拆分NPC跟战斗
        // 避免重复添加表格，改成HashSet
        public static readonly HashSet<Type> EditorConfigManagerTypes = new HashSet<Type>
        {
            typeof(ConstValueConfigManager),
            typeof(BattleAIConfigManager),
            typeof(BulletConfigManager),
            typeof(BuffConfigManager),
            typeof(BuffDescConfigManager),
            typeof(SkillConfigManager),
            typeof(SkillValueConfigManager),
            typeof(SkillDescConfigManager),
            typeof(SkillEffectConfigManager),
            typeof(SkillSelectConfigManager),
            typeof(SkillConditionConfigManager),
            typeof(SkillEventConfigManager),
            typeof(SkillTagsConfigManager),
            typeof(SkillBDLabelConfigManager),
            typeof(SkillSchoolLabelConfigManager),
            typeof(SkillTargetCondTemplateConfigManager),
            typeof(ModelConfigManager),
            typeof(BattleUnitConfigManager),
            typeof(BattleFollowConfigManager),
            typeof(TextConfigManager),
            typeof(SkillInterruptConfigManager),
            typeof(FixtureConfigManager),
            typeof(BattleCustomParamConfigManager),
            typeof(BattleHitFlyForceConfigManager),
            typeof(BattleMissionGoalConfigManager),
            typeof(BattleMissionGroupConfigManager),
            typeof(BattleMissionOneConfigManager),
            typeof(VoiceConfigManager),
            typeof(TimelineConfigManager),
            typeof(BehaviorConfigManager),
            typeof(LocationConfigManager),
            typeof(RoleAttrCustomConfigManager),
            typeof(BattleMarkerConfigManager),
            typeof(BattleSceneObjectConfigManager),

            typeof(MapEventConditionConfigManager),
            typeof(MapEventConditionGroupConfigManager),
            typeof(MapEventFormulaConfigManager),
            typeof(NpcEventConfigManager),
            typeof(MapEventStoryConfigManager),
            typeof(NpcEventLinkConfigManager),
            typeof(NpcEventActionGroupIndexConfigManager),
            typeof(NpcEventActionGroupConfigManager),
            typeof(NpcEventActionConfigManager),
            typeof(NpcTalkGroupConfigManager),
            typeof(NpcTalkConfigManager),
            typeof(NpcTalkOptionConfigManager),
            typeof(NpcEventModelConfigManager),
            typeof(MapEventGeneralFuncConfigManager),
            typeof(MapEventGeneralFuncGroupConfigManager),
            typeof(AuctionConfigManager),
            typeof(CommonNpcAmbienceConfigManager),

            typeof(ItemConfigManager),
            typeof(QuestConfigManager),
            typeof(QuestEdgeConfigManager),
            typeof(WeatherConfigManager),
            typeof(RoleExpManager),
            typeof(MapRandomRegionManager),
            typeof(ConditionConfigManager),
            typeof(NpcConfigManager),
            typeof(BattleCameraShakeConfigManager),
            typeof(DestinyConfigManager),
            typeof(LifePathTextConfigManager),
            typeof(MapAIConfigManager),
            typeof(MapRandomPointManager),
            typeof(GameTagConfigManager),
            typeof(MapAICheckerConfigManager),
            typeof(MapMonsterConfigManager),

            typeof(GuideConfigManager),
            typeof(GuideGroupConfigManager),
            
            typeof(AITaskNodeConfigManager),
            typeof(MapAnimStateConfigManager),

            typeof(MapFishConfigManager),
            typeof(MapLargeFishConfigManager),
            typeof(MapPlantConfigManager),
            typeof(MapLargePlantConfigManager),
            typeof(MapMetalConfigManager),
            typeof(MapLargeMetalConfigManager),
            typeof(MapBoxConfigManager),
            typeof(MapLandMonsterConfigManager),
            typeof(MapWaterMonsterConfigManager),
            typeof(MapBossConfigManager),
            typeof(MapLingQiTuanConfigManager),
            typeof(DropConfigManager),
            typeof(BattleConfigManager),
            typeof(CommonSubmitItemConfigManager),
            typeof(MapActionConfigManager),
            typeof(TowerHighDropConfigManager),
            typeof(TowerLowDropConfigManager),
            typeof(BattleSettlementCondConfigManager),
            typeof(BattleSettlementRulesConfigManager),
            typeof(LocalizeTableManager),
            typeof(MapUnitBuffConfigManager),
            typeof(SubmitItemGroupConfigManager),
            typeof(TreasureBoxConfigManager),
            typeof(EvolutionHeadlineConfigManager),
            typeof(EvolutionEntryConfigManager),
            typeof(FairyHouseConfigManager),        
            typeof(StoryArenaConfigManager),
            typeof(ArenaMatchConfigManager),
            typeof(TagConfigManager),
            typeof(NpcTemplateRuleConfigManager),
            typeof(MapEventPerformanceGroupConfigManager),
            typeof(MapEventPerformanceConfigManager),
            typeof(LifePathTextConfigManager),
            typeof(MapEventActorFormationConfigManager),
            typeof(ItemMarqueeConfigManager),
            typeof(ProvinceConfigManager),
            typeof(MapPosConfigManager),
            typeof(MetaBattleUnitStateConfigManager),
        };

        private static List<ITableManager> editorConfigManagers;
        public static List<ITableManager> EditorConfigManagers
        {
            get
            {
                if (editorConfigManagers == null)
                {
                    editorConfigManagers = new List<ITableManager>();
                    foreach (var managerType in EditorConfigManagerTypes)
                    {
                        var manager = managerType.GetProperty(nameof(SkillConfigManager.Instance)).GetValue(null) as ITableManager;
                        if (manager != null)
                        {
                            editorConfigManagers.Add(manager);
                        }
                        else
                        {
                            Log.Error($"DesignTable Invalid TableManager:{managerType.Name}");
                        }
                    }
                }
                return editorConfigManagers;
            }
        }
        private static List<string> editorConfigNames;
        public static List<string> EditorConfigNames
        {
            get
            {
                if (editorConfigNames == null)
                {
                    editorConfigNames = new List<string>();
                    foreach (var managerType in EditorConfigManagerTypes)
                    {
                        if (managerType.Name.Contains("Manager"))
                        {
                            editorConfigNames.Add(managerType.Name.Replace("Manager", ""));
                        }
                        else
                        {
                            Log.Error($"DesignTable Invalid TableManager3:{managerType.Name}");
                        }
                    }
                }
                return editorConfigNames;
            }
        }
        private static SortedDictionary<string, string> editorConfigManagerName2TableTash;
        public static SortedDictionary<string, string> EditorConfigManagerName2TableTash
        {
            get
            {
                if (editorConfigManagerName2TableTash == null)
                {
                    editorConfigManagerName2TableTash = new SortedDictionary<string, string>();
                    foreach (var managerType in EditorConfigManagerTypes)
                    {
                        var tableTash = managerType.GetField(nameof(SkillConfigManager.TableTash)).GetValue(null) as string;
                        if (tableTash != null)
                        {
                            editorConfigManagerName2TableTash.Add(managerType.Name, tableTash);
                        }
                        else
                        {
                            Log.Error($"DesignTable Invalid TableManager2:{managerType.Name}");
                        }
                    }
                }
                return editorConfigManagerName2TableTash;
            }
        }

        #endregion
        // 是否注册编辑器相关表格
        private static bool isRegisterDesignTable = false;

        static DesignTable()
        {
            Load();
        }
        public static void Reload()
        {
            isRegisterDesignTable = false;
            Load();
        }
        // 加载编辑器表格数据
        public static void Load()
        {
            if (!isRegisterDesignTable)
            {
                isRegisterDesignTable = true;
                Utils.WatchTime("DesignTable.Load", () =>
                {
#if !NodeExport
                    GameApp.Editor.AppEditorFacade.RegisterDesignTableForEditor(EditorConfigManagers.ToArray(), true, (name, i, length) =>
                    {
                        //Log.Debug($"DesignTable.Load {name}, {i}/{length}");
                    });
#else

                    GameApp.Editor.DesignTableManager.Instance.RegistTableEditor((name, i, length) => { }, true, EditorConfigManagers.ToArray());
#endif
                });
            }
        }
        // 获取表格Manager，如SkillConfigManager
        public static ITableManager GetTableManager(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return null;
            }
#if !NodeExport
            return GameApp.Editor.AppEditorFacade.DesignTable?.GetTableManagerByName(tableName);
#else
            return GameApp.Editor.DesignTableManager.Instance.GetTableManagerByName(tableName);
#endif
        }

        // 获取表格Manager，如SkillConfigManager
        public static ITableManager GetTableManager<T>() where T : ITableManager
        {
            return GetTableManager(typeof(T).Name);
        }

        // 获取表格数据
        public static object GetTableCell(string tableName, int id)
        {
            var tableManager = GetTableManager(tableName);
            return tableManager?.GetTableCell(id);
        }

        // 获取表格数据
        public static T GetTableCell<T>(int id) where T : class
        {
            var tableName = typeof(T).Name;
            var tableManager = GetTableManager(tableName);
            return tableManager != null ? tableManager.GetTableCell(id) as T : null;
        }

#if !NodeExport
        // TODO 后续按照配置加载表格
        public static void RegisterDesignTable(IConfigEditorManager manager)
        {
            var ns = Constants.TableNameSpace;
            var configManagers = new List<ITableManager>();

            var type = manager.GraphType;
            foreach (var configAnno in TableAnnotation.Inst.BaseConfigAnnos)
            {
                if (configAnno.IsGraphNeededConfig(type))
                {
                    var configManagerFulName = $"{ns}.{configAnno.ConfigName}Manager";
                    var configManagerType = TableHelper.GetTableType(configManagerFulName);
                    var configManager = configManagerType.GetProperty(nameof(SkillConfigManager.Instance)).GetValue(null) as ITableManager;
                    configManagers.Add(configManager);
                }
            }
            GameApp.Editor.AppEditorFacade.RegisterDesignTableForEditor(configManagers.ToArray());
        }
#endif

        #region 运行时-同步表格数据
        private static Dictionary<string, MethodInfo> cacheMethods_SerializeToBytes = new Dictionary<string, MethodInfo>();
        public static bool UpdateConfigData(object config, bool OnlyCS = false)
        {
            try
            {
                if (config == null)
                {
                    return false;
                }
#if !NodeExport
                // 特殊处理-技能数据刷新
                if (config is SkillConfig skillConfig)
                {
                    var skillValueConfig = SkillTableManager.GetSkillValueConfig(AppFacade.DesignTableManager, skillConfig.ID, 1);
                    if (skillValueConfig != null)
                    {
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SkillSchoolResType), skillConfig.SkillSchoolResType);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.FeatureLabel), skillConfig.FeatureLabel);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.BDLabels), skillConfig.BDLabels);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.MPCost), skillConfig.MPCost);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.HunLiValue), skillConfig.HunLiValue);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SectXinfaEnergyValue), skillConfig.SectXinfaEnergyValue);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.CdTime), skillConfig.CdTime);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SkillRange), skillConfig.SkillRange);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SkillMinRange), skillConfig.SkillMinRange);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.AISkillRange), skillConfig.AISkillRange);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.ChantCounterValuesList), skillConfig.ChantCounterValuesList);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.LGDamageValuesList), skillConfig.LGDamageValuesList);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SkillDamageTagsList), skillConfig.SkillDamageTagsList);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SkillTagsList), skillConfig.SkillTagsList);
                        skillValueConfig.ExSetValue(nameof(skillValueConfig.SkillTipsConditionSkillTagsList), skillConfig.SkillTipsConditionSkillTagsList);
                        DesignTable.UpdateConfigData(skillValueConfig);
                    }
                }
#endif
                if (config is ITable iTable)
                {
                    // 必须通过GetKey获取，可能存在组合键情况
                    var key = iTable.GetKey();
                    var configType = config.GetType();
                    var configName = configType.Name;
                    if (!cacheMethods_SerializeToBytes.TryGetValue(configName, out var method))
                    {
                        method = configType.GetMethod("SerializeToBytes", BindingFlags.Static | BindingFlags.Public);
                        cacheMethods_SerializeToBytes.Add(configName, method);
                    }
                    // TODO 扩展如Manager的IAfterDeserialize，如，BattleCameraShakeConfig
                    var configData = (byte[])method.Invoke(null, new object[] { config });
                    // 同步C#
                    var manager = GetTableManager(configName);

                    // TODO 组合键数据同步
                    //if (manager != null)
                    //{
                    //    if (GetTableCell(configName, key))
                    //    {
                    //    }
                    //}
                    if (manager != null && manager.ParseFromBytes(key, configData, out var configObj))
                    {
#if !NodeExport
                        // 反序列化数据存在多语言数据未处理，进一步拷贝
                        configObj?.CopyLocal(iTable);
                        if (config is SkillConfig skillConfigOld)
                        {
                            var skillConfigSync = SkillConfigManager.Instance.GetItem(skillConfigOld.ID);
                            if (skillConfigSync != null)
                            {
                                // 同步数据，避免缓存错误，清理缓存
                                skillConfigSync.ClearCache();
                            }
                        }
#endif
                    }
#if !NodeExport
                    // 记录下修改的表格信息
                    GameApp.AppFacade.DesignTableManager?.CacheUpdateTables(configName, key);
                    // 同步C++
                    if (!OnlyCS && GameApp.AppFacade.BattleManager?.Battle != null)
                    {
                        GameApp.Native.Battle.BattleWrapper.BattleManager_UpdateTableData(configName, key, configData, configData.Length);
                    }
#endif
                    return true;
                }
                Log.Error($"UpdateConfigData failed, config is not ITable, type: {config?.GetType().Name ?? "空"}");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"UpdateConfigData failed, {ex}");
            }
            return false;
        }
#endregion

        // TODO 监听表数据变化，刷新编辑器表格加载

    }
}
