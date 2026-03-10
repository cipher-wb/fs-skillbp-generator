using NodeEditor.PortType;
#if UNITY_EDITOR
using NodeEditor.SkillEditor;
#endif
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 编辑器相关配置信息处理
    /// </summary>
    public partial class ProjectConfig
    {
#if UNITY_EDITOR
        // 表格约束忽略不显示port
        static readonly List<(string, string)> ignoreRefPort = new List<(string, string)>
        {
            (nameof(ModelConfig), nameof(ModelConfigProcessor.TConfig.FixtureConfigId)),
            (nameof(ModelConfig), nameof(ModelConfigProcessor.TConfig.MapCollider)),
            (nameof(SkillConfig), nameof(SkillConfigProcessor.TConfig.BDLabels)),
            (nameof(SkillConfig), nameof(SkillConfigProcessor.TConfig.EnhanceSkillBuffConfigID)),
        };
#endif

        private Dictionary<string, ConfigJsonAnnotation> configAnnotations = new Dictionary<string, ConfigJsonAnnotation>();

        private void InitAnnotation()
        {
            // 清理老数据
            configAnnotations.Clear();

            //获取表结构记录
            var tables = Tables.Values;
            foreach (var table in tables)
            {
                var configName = table.Name;
                var configJson = GetConfigJson(configName, true);
                // 表结构
                foreach (var member in table.Members)
                {
                    Record(configName, table, member);
                }
                // Table子结构
                foreach (var subClass in table.Classes)
                {
                    var subClassName = subClass.GetClassName(configName);
                    var subConfigJson = GetConfigJson(subClassName, true);
                    subConfigJson.Annotaion = new ConfigMemberAnnotaion
                    {
                        Colume = subClass.Desc,
                        Type = null,
                        Name = subClass.Name,
                        Seaperator = subClass.Seaperator.ToString(),
                        Enum = subClass.Enum,
                        Localize = false,
                        ClassType = TableClassType.Local,
                    };
                    foreach (var member in subClass.Members)
                    {
                        Record(subClassName, null, member);
                    }
                }
            }
            // 全局子结构类型记录
            foreach (var subClass in Classes)
            {
                string configName = subClass.Name;
                var configJson = GetConfigJson(configName, true);
                configJson.Annotaion = new ConfigMemberAnnotaion
                {
                    Colume = subClass.Desc,
                    Type = null,
                    Name = subClass.Name,
                    Seaperator = subClass.Seaperator.ToString(),
                    Enum = subClass.Enum,
                    Localize = subClass.Localize,
                    ClassType = TableClassType.Global,
                };
                foreach (var member in subClass.Members)
                {
                    Record(configName, null, member);
                }
            }

#if UNITY_EDITOR
            // 特殊处理节点
            var effectExecuteInfo = GetConfigJson(nameof(SkillEffectExecuteInfo));
            if (effectExecuteInfo != null)
            {
                foreach (var kv in configAnnotations)
                {
                    var configJson = kv.Value;
                    foreach (var member in configJson.Members)
                    {
                        if (member.Type == nameof(SkillEffectExecuteInfo))
                        {
                            foreach (var exeMember in effectExecuteInfo.Members)
                            {
                                var configType = exeMember.FromFiled?.RefTableType;
                                var displayType = TablePortTypesHelper.GetType(configType) ?? TParamAnnotation.DefaultPortType;
                                var acceptMultipleEdges = !string.IsNullOrEmpty(exeMember.Seaperator.Trim());
                                configJson.RefPorts.Add(new RefPortAnnotation
                                {
                                    displayName = $"{member.Colume}.{exeMember.Colume}",
                                    displayType = displayType,
                                    acceptMultipleEdges = acceptMultipleEdges,
                                    identifier = $"{member.Name}.{exeMember.Name}",
                                    portColor = TableAnnotation.Inst.GetNodeColor(configType),
                                });
                            }
                        }
                    }
                }
            }
#endif
        }

        /// <summary>
        /// 获取Config.json信息
        /// </summary>
        public ConfigJsonAnnotation GetConfigJson(string configName, bool create = false)
        {
            if (!configAnnotations.TryGetValue(configName, out var configJson))
            {
                if (create)
                {
                    configJson = new ConfigJsonAnnotation();
                    configAnnotations.Add(configName, configJson);
                }
                else
                {
                    configJson = null;
                }
            }
            return configJson;
        }

        /// <summary>
        /// 获取Configjson记录表格成员信息
        /// </summary>
        public List<ConfigMemberAnnotaion> GetConfigJsonMembers(string configName)
        {
            var configJson = GetConfigJson(configName);
            return configJson?.Members ?? null;
        }

        /// <summary>
        /// 记录Config.json
        /// </summary>
        private void Record(string configName, object tableObject, object tableMember)
        {
            var configAnnotation = GetConfigJson(configName, true);
            var memberAnn = new ConfigMemberAnnotaion();
            string fieldExpr = null;
            switch (tableMember)
            {
                case Member member:
                    memberAnn.Colume = member.Desc;
                    memberAnn.Type = member.Type;
                    memberAnn.Name = member.Name ?? member.Desc;
                    memberAnn.Seaperator = member.Seaperator.ToString();
                    memberAnn.Enum = false;
                    memberAnn.Localize = member.Localize;
                    memberAnn.SkipEmpty = member.SkipEmpty;
                    fieldExpr = member.FromFieldExpr;
                    if (member is ExcelMember excelMember)
                    {
                        memberAnn.Colume = excelMember.Colume ?? excelMember.Desc;
                        memberAnn.FieldIndex = excelMember.FieldIndex;
                    }
                    break;
                case SubClass subClass:
                    memberAnn.Colume = subClass.Desc;
                    memberAnn.Type = null;
                    memberAnn.Name = subClass.Name ?? subClass.Desc;
                    memberAnn.Seaperator = subClass.Seaperator.ToString();
                    memberAnn.Enum = subClass.Enum;
                    memberAnn.Localize = subClass.Localize;
                    break;
            }
            configAnnotation.Members.Add(memberAnn);
            memberAnn.FieldExpr = fieldExpr;
             
            // 有表格ID关联 加入port列表
            if (fieldExpr != null)
            {
                // 可能存在多个约束，如模板约束等
                string[] fieldExprValues = fieldExpr.Split(';');
                foreach (string fieldExprValue in fieldExprValues)
                {
                    // 仅处理表格ID约束
                    string[] values = fieldExprValue.Split('|');
                    if (values.Length == 2) // SkillConfig.ID|ID
                    {
                        var value0 = values[0];
                        var value1 = values[1];
                        var value0s = value0.Split('.');
                        // 举例：SkillEffectConfig.ID
                        if (value0s.Length == 2)
                        {
                            // SkillEffectConfig
                            var value00 = value0s[0];
                            // ID
                            var value01 = value0s[1];
                            var refConfigName = value00;
                            // 约束ID： "SkillEffectConfig.ID|ID"
                            // 约束描述："SkillTagsConfig.Desc|ID"
                            // 仅处理表格约束
                            var keyMember = GetTable(refConfigName)?.GetKeySingle();
                            if (keyMember != null)
                            {
                                // 暂时仅支持表格单键值，组合键不支持
                                var isConfigId = value01 == keyMember.Name;
                                // 无需判定ID，可能是SkillTagConfig.Desc|ID形式
                                //if (isConfigId)
                                {
                                    memberAnn.FromFiled = new FromFiledAnnotation
                                    {
                                        Name = memberAnn.Colume,
                                        isConfigId = isConfigId,
                                        RefDesc = isConfigId ? null : value01,
                                        RefTableName = refConfigName,
                                        RefTableFullName = TableHelper.ToTableFullName(refConfigName),
                                        RefTableManagerFullName = TableHelper.ToTableManager(refConfigName),
                                    };
                                    #if UNITY_EDITOR
                                    // 忽略端口显示
                                    if (!ignoreRefPort.Contains((configName, memberAnn.Name)))
                                    {
                                        // 端口信息
                                        var configType = memberAnn.FromFiled?.RefTableType;
                                        var displayType = TablePortTypesHelper.GetType(configType) ?? TParamAnnotation.DefaultPortType;
                                        // 是否是列表 列表支持多条连线
                                        var acceptMultipleEdges = !string.IsNullOrEmpty(memberAnn.Seaperator.Trim());
                                        // 端口类型
                                        configAnnotation.RefPorts.Add(new RefPortAnnotation
                                        {
                                            displayName = memberAnn.Colume,
                                            displayType = displayType,
                                            acceptMultipleEdges = acceptMultipleEdges,
                                            identifier = memberAnn.Name,
                                            portColor = TableAnnotation.Inst.GetNodeColor(configType),
                                        });
                                    }
                                    #endif
                                }
                            }
                        }
                    }
                    else if (fieldExprValue.Trim().Length > 0 && values.Length != 3) // 三项为模板数值替换，不做报错提示，如：TemplateID|BattleUnitTemplateConfig.ID|UnitType
                    {
                        Log.Fatal($"表格ID关联必须是|分隔的两项，例如：SkillEffectConfig.ID|ID, fieldExpr: {fieldExpr}");
                    }
                }
            }
        }
    }
}