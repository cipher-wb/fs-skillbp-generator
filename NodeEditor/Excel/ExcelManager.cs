#if !NodeExport
using UnityEditor;
using UnityEngine;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Sirenix.Utilities;
using System.Diagnostics;
using TableDR;

namespace NodeEditor
{
    public partial class ExcelManager : Singleton<ExcelManager>
    {
        [Flags]
        public enum ExcelFlags
        {
            None = 0,               // 空
            CheckMembers = 1 << 0,  // 检查成员
            FormatData = 1 << 1,    // 数据排序
            OnlyModify = 1 << 2,    // 仅修改-不新增
        }
        // TODO
        // 子结构分列，如：RoleCommonProperty.OutLookStyle，RoleCommonProperty.OutLookCode
        // 字段忽略导出，如：NpcConfig.OutLookData
        // 页签不同情况处理，添加导出sheetName区分：SkillConfig-Map，SkillConfig-NPC等

        #region 常量定义
        /// <summary>
        /// 表格开头两行
        /// </summary>
        public const int EXCEL_HEAD_ROWS = 2;

        #endregion

        /// <summary>
        /// excel的字段是否可以填充数据,普通字段默认是可以的
        /// <sheet,fun<字段名，该字段是否可填充数据>
        /// </summary>
        private Dictionary<string, Func<string, object, bool>> excelMemberRefillableAction = new Dictionary<string, Func<string, object, bool>>();

        public const string name = "Excel 工具";

        /// <summary>
        /// 缓存表格Excel相关数据<{excelPath}|{configName}, CacheExcelData>（注意：导出操作才会缓存数据，优化效率便于下次导出处理）
        /// </summary>
        private Dictionary<string, CacheExcelData> excelDataCaches = new Dictionary<string, CacheExcelData>();

        public Dictionary<string, CacheExcelData> ExcelDataCaches { get { return excelDataCaches; } }

        #region config.json相关
        public ProjectConfig ProjectConfig { get; private set; }
        public string PackegeName => ProjectConfig.PackegeName;

        public string ProjectConfigPath => Constants.ConfigJsonPath;

        public string ExcelPathPrefix => Constants.ExcelPathPrefix;

        public string TableSeaperator => ProjectConfig.TableSeaperator;
        public string LocalizeKey => ProjectConfig.LocalizeKey;
        /// <summary>
        /// 编辑器缓存变量后缀
        /// </summary>
        public static readonly string LocalizeEditor = "Editor";
        #endregion
        public ExcelManager()
        {
            Init();
        }

        /// <summary>
        /// 表格数据更新，刷新缓存数据
        /// </summary>
        public void OnExcelChange()
        {
            Init();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="configFile"></param>
        public void LoadProjectConfig()
        {
            try
            {
                var configFile = Constants.ConfigJsonPath;
                if (!File.Exists(configFile))
                    throw new FileNotFoundException($"找不到文件: {configFile}");

                ProjectConfig = Utils.ReadFromJson<ProjectConfig>(configFile);
                if (ProjectConfig == null)
                    throw new NullReferenceException($"加载配置文件失败: {configFile}");

                // 如果是按照表格拆分保存的，那么需要重新去挨个加载一下。
                var splitConfig = ProjectConfig.SplitConfig;
                if (splitConfig)
                {
                    var configPath = Path.Combine(Path.GetDirectoryName(configFile), ProjectConfig.SplitConfigDir);
                    if (Directory.Exists(configPath))
                    {
                        ProjectConfig.Tables.Clear();

                        // 加载目录下所有的表格json文件。
                        Directory.GetFiles(configPath, "*.json").Foreach(tableConfigFile =>
                        {
                            var table = Utils.ReadFromJson<Table>(tableConfigFile);

                            // 跳过不是合法表格的配置文件。
                            if (table == null || string.IsNullOrEmpty(table.Name) || table.Members.Count == 0)
                                return;

                            ProjectConfig.Tables.Add(table.Name, table);
                        });
                    }
                }

                ProjectConfig.Init();
            }
            catch (Exception ex)
            {
                throw new Exception($"加载配置文件失败, 详情如下:\n{ex}");
            }
        }

        /// <summary>
        /// 解析config.json
        /// </summary>
        private void Init()
        {
            try
            {
                excelDataCaches.Clear();
                LoadProjectConfig();
#if UNITY_EDITOR
                TurnFileWatcher(true);
#endif
            }
            catch (Exception ex)
            {
                throw new Exception($"解析表格配置文件异常，请检查文件：{Path.GetFileName(Constants.ConfigJsonPath)}\n{ex}");
            }
        }

        #region CacheExcelData，缓存Excel数据，避免重复解析数据

        /// <summary>
        /// 获取表格相关所有信息，不为空
        /// </summary>
        public CacheExcelData GetCacheExcelData(string excelPath, string configName)
        {
            // TODO 记录时间戳，有新文件需要刷新
            var formatPath = Utils.PathFormat(Path.GetFullPath(excelPath));
            var keyValue = $"{formatPath}|{configName}";
            if (!excelDataCaches.TryGetValue(keyValue, out var configDatas))
            {
                configDatas = new CacheExcelData
                {
                    ExcelPath = excelPath,
                };
                excelDataCaches.Add(keyValue, configDatas);
                // 读表缓存数据
                DoCacheExcelData(excelPath, configName);
            }
            return configDatas;
        }

        /// <summary>
        /// 获取Excel缓存对应表格已配置ID
        /// </summary>
        private Dictionary<string, int> GetCacheExcelIDs(string excelPath, string sheet, string configName)
        {
            var dic = GetCacheExcelData(excelPath, configName).ExcelIDs;
            if (dic == null)
            {
                return null;
            }
            dic.TryGetValue(sheet, out var retDic);
            return retDic;
        }

        /// <summary>
        /// 缓存表格数据  TODO 一次性缓存所有
        /// </summary>
        private void DoCacheExcelData(string excelPath, string configName)
        {
            try
            {
                if (string.IsNullOrEmpty(excelPath) ||
                    string.IsNullOrEmpty(configName))
                {
                    Log.Error($"缓存表格数据失败，传入参数错误: {string.IsNullOrEmpty(excelPath)}, {string.IsNullOrEmpty(configName)}");
                    return;
                }
                // 注：Type.GetType("TableDR.SkillConfig, TableDR_CS");
                var configType = TableHelper.GetTableType($"{PackegeName}.{configName}");
                if (configType == null)
                {
                    Log.Error($"缓存表格数据失败, 找不到表格类型: {PackegeName}.{configName}");
                    return;
                }
                // 获取组合键对应成员信息
                var configTableKeys = ProjectConfig.GetTable(configName)?.GetKeys();
                if (configTableKeys == null)
                {
                    Log.Error($"缓存表格数据失败, 找不到表格key: {PackegeName}.{configName}");
                    return;
                }
                if (configTableKeys.Count > 1 && configTableKeys.Count((x) => x.Type == "String") != 0)
                {
                    Log.Error($"缓存表格数据异常, String组合键不支持: {configName}");
                    return;
                }
                var excelName = Path.GetFileName(excelPath);
                var configDatas = GetCacheExcelData(excelPath, configName);
                var sheetNames = new List<string>();
                ExcelHelper.ReadExcelSheets(excelPath, (sheetName) =>
                {
                    if (ToolHelper.TryGetTableName(sheetName, ProjectConfig.TableSeaperator, out var tableName) && configName == tableName)
                    {
                        if (!sheetNames.Contains(sheetName))
                        {
                            sheetNames.Add(sheetName);
                            return true;
                        }
                        else
                        {
                            Log.Error($"缓存表格数据异常, 表格存在相同命名的sheet: {excelName}, {sheetName}");
                        }
                    }
                    return false;
                }, (sheetName, data) =>
                {
                    configDatas.SetExcelData(sheetName, data);
                });
                if (sheetNames.Count == 0)
                {
                    // 无符合configName的sheet
                    return;
                }
                var propertyInfos = configType.GetProperties();
                // 记录已存在的表格ID
                var IDs = new Dictionary<string, Dictionary<string, int>>();
                // 记录成员变量描述名
                var membersPropDic = new Dictionary<string, PropertyInfo[]>();
                // 记录成员变量信息
                var membersNameDic = new Dictionary<string, string[]>();
                // 记录成员缺失变量信息
                var membersMissingNameDic = new Dictionary<string, List<string>>();

                foreach (var sheetName in sheetNames)
                {
                    var excelDataInSheet = configDatas.GetDataBySheet(sheetName);
                    // 可能存在sheetName找不到情况
                    if (excelDataInSheet == null)
                    {
                        continue;
                    }
                    int length0 = excelDataInSheet.GetLength(0);
                    int length1 = excelDataInSheet.GetLength(1);
                    var membersName = new string[length1];
                    var membersProp = new PropertyInfo[length1];

                    if (!membersMissingNameDic.TryGetValue(sheetName, out var missingList))
                    {
                        missingList = new List<string>();
                        membersMissingNameDic.Add(sheetName, missingList);
                    }

                    // 记录1，2行数据
                    for (int i = 1; i <= length1; i++)
                    {
                        // 第一行描述名 desc
                        var cell1 = excelDataInSheet[EXCEL_HEAD_ROWS - 1, i];
                        var columnDesc = cell1?.ToString().Trim() ?? null;
                        // 第二行变量名 name
                        var cell2 = excelDataInSheet[EXCEL_HEAD_ROWS, i];
                        var columnName = cell2?.ToString().Trim() ?? null;
                        membersName[i - 1] = columnName;
                        // 记录property
                        if (columnName != null)
                        {
                            var prop = configType.GetProperty(columnName);
                            membersProp[i - 1] = prop;
                            var desc = prop.GetAttribute<DescriptionAttribute>();
                            if (desc != null && desc.Description != columnDesc)
                            {
                                var missingInfo = $"【列描述不一致】{excelName}-{sheetName}:{columnDesc}-{desc.Description}";
                                if (!missingInfo.Contains(missingInfo))
                                {
                                    missingList.Add(missingInfo);
                                    Log.Error(missingInfo);
                                }
                            }
                        }
                        else if (columnDesc != null)
                        {
                            foreach (var prop in propertyInfos)
                            {
                                var desc = prop.GetAttribute<DescriptionAttribute>();
                                if (desc?.Description == columnDesc)
                                {
                                    membersProp[i - 1] = prop;
                                    break;
                                }
                            }
                        }
                    }
                    membersPropDic.Add(sheetName, membersProp);
                    membersNameDic.Add(sheetName, membersName);

                    // TODO 需要适配组合键
                    // 找到Table组合键（如，ID-SkillLevel）对应的列索引
                    var configTableKeysIndex = configTableKeys.Select(x => Array.IndexOf(membersName, x.Name)).ToList();

                    // 记录ID
                    var idsDic = new Dictionary<string, int>();
                    for (int i = EXCEL_HEAD_ROWS + 1; i <= length0; i++)
                    {
                        try
                        {
                            long id = 0;
                            string ID = string.Empty;
                            int keyBits = 0;
                            // 组合键id获取
                            for (int j = 0; j < configTableKeys.Count; j++)
                            {
                                var excelMember = configTableKeys[j];
                                var index = configTableKeysIndex[j] + 1;
                                var cellValue = excelDataInSheet[i, index]?.ToString().Trim();
                                keyBits += excelMember.KeyBits;
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    if(!int.TryParse(cellValue,out var cellValueInt))
                                    {
                                        var subClass = ProjectConfig.Classes.FirstOrDefault(x => x.Enum && x.Name == excelMember.Type);
                                        if(subClass == null)
                                        {
                                            subClass = ProjectConfig.GetTable(configName)?.Classes.FirstOrDefault(x => x.Enum && x.Name == excelMember.Type);
                                        }
                                        if(subClass == null)
                                        {
                                            throw new Exception($"缓存表格数据异常,key值获取不对,{excelName},{sheetName},{excelMember.Name}");
                                        }
                                        var enumInfo = subClass.EnumInfos.FirstOrDefault(x=>x.Name == cellValue || x.Text == cellValue);
                                        if (enumInfo == null)
                                        {
                                            // 有可能key是使用了，表达式转换，取不到值
                                            throw new Exception($"缓存表格数据异常,key值获取不对,{excelName},{sheetName},{excelMember.Name}");
                                        }
                                        cellValueInt = enumInfo.Value;
                                    }
                                    id |= (long)(cellValueInt) << keyBits;
                                }
                            }
                            if (id != 0)
                            {
                                // 组合键id缓存
                                idsDic[id.ToString()] = i;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error($"缓存表格数据异常, 缓存ID异常: {excelName}, {sheetName}\n{ex}");
                            idsDic.Clear();
                            break;
                        }
                    }
                    IDs.Add(sheetName, idsDic);

                    // 记录缺失变量/列，TableDR.SkillConfig
                    var membersList = membersName.ToList();
                    if ((propertyInfos.Length - 1) != membersList.Count)
                    {
                        foreach (var propertyInfo in propertyInfos)
                        {
                            var desc = propertyInfo.GetAttribute<DescriptionAttribute>();
                            if (desc != null)
                            {
                                var propName = propertyInfo.Name;
                                var index = membersList.IndexOf(propName);
                                var has = index != -1;
                                if (!has && propName.EndsWith(LocalizeKey))
                                {
                                    var nameWithoutKey = propName.Remove(propName.Length - LocalizeKey.Length, LocalizeKey.Length);
                                    index = membersList.IndexOf(nameWithoutKey);
                                    has = index != -1;
                                }
                                if (!has && propertyInfo.SetMethod != null && propertyInfo.GetCustomAttribute<ExcelIgnore>() == null)
                                {
                                    var missingInfo = $"【缺失字段列】{excelName}-{sheetName}:{propName}";
                                    if (!missingList.Contains(missingInfo))
                                    {
                                        missingList.Add(missingInfo);
                                        Log.Error(missingInfo);
                                    }
                                }
                            }
                        }
                    }
                }
                configDatas.ExcelMembersProp = membersPropDic;
                configDatas.ExcelMembersName = membersNameDic;
                configDatas.ExcelIDs = IDs;
                configDatas.ExcelMembersMissingName = membersMissingNameDic;
            }
            catch (Exception ex)
            {
                Log.Error($"缓存表格数据失败, configName: {configName}, {excelPath}\n{ex.ToString()}");
            }
        }

        /// <summary>
        /// 获取缓存表格数据，未缓存，则读表缓存
        /// </summary>
        private object[,] GetCacheSheetData(string excelPath, string configName, string sheetName)
        {
            var excelDataCache = GetCacheExcelData(excelPath, configName);
            object[,] excelDataArray = excelDataCache.GetDataBySheet(sheetName);
            if (excelDataArray == null)
            {
                // 未缓存，则读表缓存
                DoCacheExcelData(excelPath, configName);
                excelDataArray = excelDataCache.GetDataBySheet(sheetName);
            }
            return excelDataArray;
        }

        /// <summary>
        /// 获取Excel缓存对应成员名称
        /// </summary>
        private PropertyInfo[] GetExcelMembersProp(string excelPath, string sheet, string configName)
        {
            var configData = GetCacheExcelData(excelPath, configName);
            if (configData.ExcelMembersProp == null)
            {
                return null;
            }
            configData.ExcelMembersProp.TryGetValue(sheet, out var excelMembersDesc);
            return excelMembersDesc;
        }

        /// <summary>
        /// 获取Excel缓存对应成员信息
        /// </summary>
        private string[] GetExcelMembersName(string excelPath, string sheet, string configName)
        {
            var configData = GetCacheExcelData(excelPath, configName);
            if (configData.ExcelMembersName == null)
            {
                return null;
            }
            configData.ExcelMembersName.TryGetValue(sheet, out var excelMembersName);
            return excelMembersName;
        }

        /// <summary>
        /// 获取Excel缓存对应缺失成员信息
        /// </summary>
        private List<string> GetExcelMembersMissingName(string excelPath, string sheet, string configName)
        {
            var configData = GetCacheExcelData(excelPath, configName);
            if (configData.ExcelMembersMissingName == null)
            {
                return null;
            }
            configData.ExcelMembersMissingName.TryGetValue(sheet, out var excelMembersMisingName);
            return excelMembersMisingName;
        }

        #endregion CacheExcelData，缓存Excel数据，避免重复解析数据

        /// <summary>
        /// 转化对象成Excel存储的信息
        /// </summary>
        /// <param name="obj">需要转化对象</param>
        /// <param name="objParent">对象父节点，用于处理数组等</param>
        /// <returns>表格存储信息</returns>
        private string ConvertObject(object obj, object objParent, string seaperatorParent = "")
        {
            var propString = string.Empty;
            if (obj == null)
            {
                return propString;
            }
            var type = obj.GetType();
            var typeName = type.Name;
            var objName = obj.ToString();
            var configJson = ProjectConfig.GetConfigJson(typeName);
            if (configJson != null)
            {
                // 如果是子类，那么展开下
                if (configJson.IsSubClass)
                {
                    var seaperator = configJson.Annotaion.Seaperator.Trim();
                    // 如果是枚举，那么用注释替换下
                    if (configJson.Annotaion.Enum)
                    {
                        // 如果不是枚举数组
                        if (string.IsNullOrEmpty(seaperator))
                        {
                            var field = type.GetField(objName);
                            if (field != null)
                            {
                                var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
                                if (descAttr != null)
                                {
                                    propString = descAttr.Description;
                                }
                                else
                                {
                                    propString = objName;
                                }
                            }
                            else
                            {
                                // 严重错误，必须要抛异常，避免导出数据错误被忽略
                                throw new Exception($"未获取到类型:typeName:{typeName}, objName:{objName}");
                            }
                        }
                        else
                        {
                            if (configJson.Annotaion != null)
                            {
                                seaperatorParent = configJson.Annotaion.Seaperator.Trim();
                            }
                            // 枚举数组，注：枚举数组toString后以','分割
                            var enumNames = objName.Split(/*seaperatorParent.ToCharArray()*/',');
                            foreach (var enumName in enumNames)
                            {
                                var enumFiled = type.GetField(enumName.Trim());
                                if (enumFiled == null)
                                {
                                    continue;
                                }
                                var attrs = enumFiled.GetCustomAttributes(typeof(DescriptionAttribute), false);
                                foreach (var attr in attrs)
                                {
                                    if (attr is DescriptionAttribute descAttr)
                                    {
                                        propString += (string.IsNullOrEmpty(propString) ? "" : seaperatorParent) + descAttr.Description;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // 非枚举类型的数组
                        if (string.IsNullOrEmpty(seaperator))
                        {
                            // TODO
                            //propString =  ConvertProperty(null, null, objSelf);
                            Log.Error($"未实现类型:typeName:{typeName}, objName:{objName}");
                        }
                        else
                        {
                            propString = ConvertDesc(obj, type);
                        }
                    }
                }
            }
            else
            {
                // 如果是列表
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IReadOnlyList<>) || type.GetGenericTypeDefinition() == typeof(ReadOnlyCollection<>)))
                {
                    var itemType = type.GetGenericArguments()[0];
                    if (obj is IEnumerable collection)
                    {
                        var enu = collection.GetEnumerator();
                        while (enu.MoveNext())
                        {
                            propString = propString + (string.IsNullOrEmpty(propString) ? "" : seaperatorParent) + ConvertObject(enu.Current, null);
                        }
                        // TODO 优化，如果是TParam列表，那么去除末尾的0参数
                    }
                }
                else if (obj != null)
                {
                    propString = obj.ToString();
                }
            }

            return propString;
        }

        /// <summary>
        /// 转译约束等信息
        /// </summary>
        /// <param name="obj">对象值</param>
        /// <param name="type">对象类型</param>
        /// <param name="singleMemberName">单个对象转化，如配置则TRoleCommonProperty.FirstName（"测试", TRoleCommonProperty, "FirstName"）</param>
        /// <returns></returns>
        public string ConvertDesc(object obj, Type type, string singleMemberName = null)
        {
            if (type == null)
            {
                return string.Empty;
            }
            var propString = string.Empty;
            var typeName = type.Name;
            var configJson = ProjectConfig.GetConfigJson(typeName);
            bool isFirst = true;
            var subProps = type.GetProperties();
            var isSingleMember = !string.IsNullOrEmpty(singleMemberName);
            foreach (var subProp in subProps)
            {
                var ignore = subProp.GetAttribute<ExcelIgnore>();
                if(ignore != null)
                {
                    continue;
                }
                // 特殊指定的对象
                if (isSingleMember)
                {
                    if (subProp.Name != singleMemberName)
                    {
                        continue;
                    }
                }
                // 过滤下PID
                if (subProp.SetMethod != null)
                {
                    var jsonMembers = ProjectConfig.GetConfigJsonMembers(typeName);
                    var jsonMember = jsonMembers?.Find(m =>
                    {
                        return m.Name == subProp.Name;
                    });

                    var propValue = isSingleMember ? obj : subProp.GetValue(obj);
                    var parentValue = isSingleMember ? null : obj;
                    var subPropValue = ConvertObject(propValue, parentValue, jsonMember?.Seaperator);
                    // config记录成员
                    var membersConfig = configJson?.Members ?? null;
                    if (membersConfig != null)
                    {
                        var memberConfig = membersConfig.Find(m =>
                        {
                            return m.Name == subProp.Name;
                        });
                        if (memberConfig != null)
                        {
                            var RefDesc = memberConfig.FromFiled?.RefDesc;
                            if ((!string.IsNullOrEmpty(memberConfig.FieldExpr) || !string.IsNullOrEmpty(RefDesc))
                                && subPropValue == "0")
                            {
                                // 如果是约束表格ID，默认如果ID是0，那么不写入
                                subPropValue = string.Empty;
                            }
                            else if (!string.IsNullOrEmpty(RefDesc))
                            {
                                // 如果是约束描述，需要查找下对应的描述文本替换ID
                                var propValues = subPropValue.Split(jsonMember?.Seaperator);
                                subPropValue = string.Empty;
                                for (int i = 0; i < propValues.Length; i++)
                                {
                                    if (int.TryParse(propValues[i], out var refId))
                                    {
                                        var refTableType = memberConfig.FromFiled.RefTableType;
                                        var subConfigObj = DesignTable.GetTableCell(refTableType?.Name, refId);
                                        var desc = subConfigObj?.ExGetValue(RefDesc)?.ToString() ?? string.Empty;
                                        if (string.IsNullOrEmpty(desc))
                                        {
                                            Log.Error($"【{typeName}】导出【subProp.Name】数据有误，找不到ID={refId}");
                                            continue;
                                        }

                                        if (string.IsNullOrEmpty(subPropValue))
                                            subPropValue = desc;
                                        else
                                            subPropValue += jsonMember?.Seaperator + desc;
                                    }
                                }
                            }
                        }
                    }
                    if (isFirst)
                    {
                        propString += subPropValue;
                        isFirst = false;
                    }
                    else
                    {
                        propString += configJson.Annotaion.Seaperator + subPropValue;
                    }
                }
            }
            // 如果填写数值为空，那么过滤下
            if (propString.Length == 0 || (configJson.Annotaion != null && propString.Replace(configJson.Annotaion.Seaperator, "").Length == 0))
            {
                propString = string.Empty;
            }
            return propString;
        }

        /// <summary>
        /// 添加config写回到excel时，excel字段是否可以填充数据的回调
        /// </summary>
        /// <param name="configType">config的数据类型</param>
        /// <param name="action">回调函数</param>
        public void AddExcelMemberRefillableAction(string configType, Func<string, object, bool> action)
        {
            excelMemberRefillableAction[configType] = action;
        }

        public void RemoveExcelMemberRefillableAction(string sheet, Func<string, object, bool> action)
        {
            excelMemberRefillableAction.Remove(sheet);
        }

        /// <summary>
        /// 转化表数据为Excel数据映射
        /// </summary>
        /// <param name="configObj"></param>
        /// <returns></returns>
        public Dictionary<string, string> ConvertConfig2Excel(string excelPath, string sheet, object configObj)
        {
            try
            {
                if (configObj == null)
                {
                    return null;
                }
                var configType = configObj.GetType();
                var configName = configType.Name;
                // 依据对象成员信息，转化成表格存储信息
                var excelData = new Dictionary<string, string>();
                // 表格记录成员
                var membersProp = GetExcelMembersProp(excelPath, sheet, configName);
                var membersName = GetExcelMembersName(excelPath, sheet, configName);
                // config记录成员
                var jsonMembers = ProjectConfig.GetConfigJsonMembers(configName);
                if (membersProp != null)
                {
                    for (int i = 1, length = membersProp.Length; i <= length; i++)
                    {
                        var memberName = membersProp[i - 1]?.Name;
                        if (membersProp[i - 1] == null)
                        {
                            memberName = membersName[i - 1];
                        }
                        
                        string memberValue = null;
                        if (!string.IsNullOrEmpty(memberName))
                        {
                            bool refillable = true;
                            if (excelMemberRefillableAction.TryGetValue(configName, out var action))
                            {
                                refillable = action.Invoke(memberName, configObj);
                            }
                            if (!refillable)
                            {
                                continue;
                            }

                            var prop = configType.GetProperty(memberName);
                            if (prop != null)
                            {
                                var jsonMember = jsonMembers?.Find(m =>
                                {
                                    return m.Name == memberName;
                                });
                                // 如果存在成员变量是多语言配置，那么更换下获取值
                                if (jsonMember?.Localize == true)
                                {
                                    // 编辑器缓存变量后缀变更：LocalizeKey->LocalizeEditor
                                    var keyProp = configType.GetProperty(memberName + /*LocalizeKey*/LocalizeEditor);
                                    if (keyProp != null)
                                    {
                                        var keyValue = keyProp.GetValue(configObj);
                                        if (keyValue != null)
                                        {
                                            memberValue = ConvertObject(keyValue, configObj);
                                        }
                                        else
                                        {
                                            memberValue = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        memberValue = ConvertObject(prop.GetValue(configObj), configObj);
                                    }
                                }
                                else if (prop.SetMethod != null)
                                {
                                    var propValue = prop.GetValue(configObj);
                                    // 如果是约束表格ID，那么如果是默认值0，那么就不写回Excel
                                    var isFieldExpr = !string.IsNullOrEmpty(jsonMember?.FieldExpr);
                                    if (isFieldExpr && propValue?.ToString() == "0")
                                    {
                                        memberValue = string.Empty;
                                    }
                                    else
                                    {
                                        if (isFieldExpr && !string.IsNullOrEmpty(jsonMember.FromFiled?.RefDesc))
                                        {
                                            // 约束字段转化
                                            memberValue = ConvertDesc(propValue, configType, memberName);
                                        }
                                        else
                                        {
                                            memberValue = ConvertObject(propValue, configObj, jsonMember?.Seaperator ?? "");
                                        }
                                    }
                                }
                            }
                            else if (memberName.Contains(".") || memberName.Contains("["))
                            {
                                var keyValue = GetConfig2ExcelExpandedMemberValue(configObj, configType, memberName);
                                var obj = keyValue.Item1;
                                var type = keyValue.Item3;
                                var name = keyValue.Item4;
                                if (obj == null)
                                {
                                    memberValue = string.Empty;
                                }
                                else
                                {
                                    memberValue = ConvertDesc(obj, type, name);
                                }
                            }
                        }
                        if (memberName != null && memberValue != null)
                        {
                            excelData.Add(memberName, memberValue);
                        }
                    }
                }
                return excelData;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// config数据转成excel数据时,获取展开的字段数据只,类似A.B.C和A.B[1].C
        /// </summary>
        /// <param name="configType"></param>
        /// <param name="memberName"></param>
        private (object, string, Type, string) GetConfig2ExcelExpandedMemberValue(object configVal, Type configType, string memberName)
        {
            int pointIndx = memberName.IndexOf('.');
            int arrayFlagIndex = memberName.IndexOf('[');
            if (pointIndx < 0 && arrayFlagIndex < 0)
            {
                PropertyInfo propertyInfo = configVal.GetType().GetProperty(memberName);
                if (propertyInfo != null)
                {
                    var jsonMembers = ProjectConfig.GetConfigJsonMembers(configVal.GetType().Name);
                    var jsonMember = jsonMembers?.Find(m =>
                    {
                        return m.Name == memberName;
                    });
                    
                    return (propertyInfo.GetValue(configVal), jsonMember?.Seaperator, configVal.GetType(), memberName);
                }
                return (null, null, null, null);
            }

            if (pointIndx > 0 && (pointIndx < arrayFlagIndex || arrayFlagIndex < 0)) //取属性
            {
                string propName = memberName.Substring(0, pointIndx);
                object subVal = configVal.ExGetValue(propName);
                if (subVal == null)
                {
                    return (null, null, null, null);
                }
                return GetConfig2ExcelExpandedMemberValue(subVal, configType, memberName.Substring(pointIndx + 1));
            }
            else if (arrayFlagIndex > 0 && (arrayFlagIndex < pointIndx || pointIndx < 0)) // 取数组值
            {
                int arrayFlagIndex1 = memberName.IndexOf(']');
                string indexStr = memberName.Substring(arrayFlagIndex + 1, arrayFlagIndex1 - arrayFlagIndex);
                int index = int.Parse(indexStr);
                string propName = memberName.Substring(0, arrayFlagIndex);
                IList subList = (IList)configVal.ExGetValue(propName);
                if (subList == null)
                {
                    return (null, null, null, null);
                }
                object subVal = subList[index];
                if (pointIndx < 0)
                {
                    return (subVal, null, null, null);
                }
                return GetConfig2ExcelExpandedMemberValue(subVal, configType, memberName.Substring(pointIndx + 1));
            }
            return (null, null, null, null);
        }

        /// <summary>
        /// 修改表格数据，包括修改+新增数据
        /// </summary>
        private bool ModifyExcelData(string excelPath, string sheetName, string configName, List<object> configs, ref object[,] newExcelData, Action<bool, string, float> actionModify = null, ExcelFlags flags = ExcelFlags.None)
        {
            string exceptionGraph = string.Empty;
            try
            {
                if (configs == null || configs.Count == 0)
                {
                    return false;
                }
                var excelName = Path.GetFileName(excelPath);
                // 先读表，追加数据
                var excelData = GetCacheSheetData(excelPath, configName, sheetName);
                if (excelData == null)
                {
                    Log.Fatal($"【{excelName}】修改表数据失败，找不到sheet: {sheetName}");
                    return false;
                }
                var excelLength0 = excelData.GetLength(0);
                var excelLength1 = excelData.GetLength(1);
                // 依据对象成员信息，转化成表格存储信息
                var curExcelDataCache = GetCacheExcelData(excelPath, configName);
                var excelIDs = curExcelDataCache.GetExcelIDs(sheetName);

                var tempExcelData = excelData;
                // 允许新增行数据，那么重新分配数组
                var isAllowAddNewData = !flags.HasFlag(ExcelFlags.OnlyModify);
                if (isAllowAddNewData)
                {
                    // 分配表格数据
                    tempExcelData = Array.CreateInstance(typeof(object), new int[] { excelLength0 + configs.Count, excelLength1 }, new int[] { 1, 1 }) as object[,];
                    // 读表数据从1开始写入
                    for (var i = 1; i <= excelLength0; i++)
                    {
                        for (var j = 1; j <= excelLength1; j++)
                        {
                            tempExcelData[i, j] = excelData[i, j];
                        }
                    }
                }

                bool isDirtyAll = false;
                int lastDataIndex = excelLength0;
                for (int j = 0, configCount = configs.Count; j < configCount; j++)
                {
                    var config = configs[j];
                    // 报错文件不一定准，可能存在多个文件混合表格数据
                    if (string.IsNullOrEmpty(exceptionGraph) && config is EditorBaseConfig editorBaseConfig)
                    {
                        exceptionGraph = editorBaseConfig.EditorGraphName;
                    }
                    string configID = null;
                    if (config is ITable configTable)
                    {
                        configID = configTable.GetKey().ToString();
                    }
                    else
                    {
                        var configKey = ProjectConfig.GetTable(configName)?.GetKeySingle();
                        if (configKey != null)
                        {
                            configID = config.ExGetValue(configKey.Name)?.ToString() ?? null;
                        }
                    }
                    if (string.IsNullOrEmpty(configID))
                    {
                        continue;
                    }
                    if (configID == "0")
                    {
                        Log.Error($"ModifyExcelData error, configID is 0, configName : {configName}");
                        continue;
                    }
                    var oldRowIndex = -1;
                    if (excelIDs.TryGetValue(configID, out var rowIndx))
                    {
                        // 缓存老数据索引
                        oldRowIndex = rowIndx;
                        // TODO 清理数据可以此处标脏，统一清理
                    }
                    else if (!isAllowAddNewData)
                    {
                        // 如果不允许新增数据，那么跳过
                        continue;
                    }
                    bool isNewLine = oldRowIndex == -1;
                    bool isDirty = isNewLine;
                    int newRowIndex = oldRowIndex;
                    if (isNewLine)
                    {
                        lastDataIndex++;
                        newRowIndex = lastDataIndex;
                    }
                    // 转化对象为表数据内容
                    var configExcelMap = ConvertConfig2Excel(excelPath, sheetName, config);
                    // 表格记录列成员变量
                    var members = GetExcelMembersName(excelPath, sheetName, configName);
                    // 表格记录列描述
                    var membersProp = GetExcelMembersProp(excelPath, sheetName, configName);
                    if (members != null)
                    {
                        for (int i = 1, length = members.Length; i <= length; i++)
                        {
                            var memberName = members[i - 1];
                            var memberProp = membersProp[i - 1];
                            var memberValue = string.Empty;
                            // 无变量名情况
                            if (memberName == null)
                            {
                                // 纯Excel备注信息，如描述及所属编辑器文件名等
                                // 从描述中取列
                                memberName = memberProp?.Name;
                            }
                            if (memberName != null && configExcelMap.TryGetValue(memberName, out memberValue))
                            {
                                tempExcelData[newRowIndex, i] = memberValue;
                            }
                            tempExcelData[newRowIndex, i] = memberValue;
                            if (!isDirty && (newRowIndex > excelLength0 || !object.Equals(memberValue, excelData[newRowIndex, i])))
                            {
                                isDirty = true;
                            }
                        }
                    }
                    isDirtyAll |= isDirty;
                    if (isNewLine)
                    {
                        // 新ID，缓存增加
                        excelIDs.Add(configID, newRowIndex);
                    }
                    // 刷新缓存数据
                    curExcelDataCache.SetExcelData(sheetName, tempExcelData);
                    actionModify?.Invoke(isNewLine, $"{configName}:{j}/{configCount}", (float)j / configCount);
                    //Log.Info(string.Format("修改表格，{0} ,{1}第{2}行, ID:{3}", configName, isNewLine ? "新增" : "覆写", newDataIndex, configID));
                }
                // 注：末尾无效数据清理通过ExcelHelper.FormatExcelSheet()处理
                var newLength0 = lastDataIndex;
                var newLength1 = excelLength1;
                newExcelData = Array.CreateInstance(typeof(object), new int[] { newLength0, newLength1 }, new int[] { 1, 1 }) as object[,];
                // 读表数据从1开始写入
                for (int i = 1; i <= newLength0; i++)
                {
                    for (int j = 1; j <= newLength1; j++)
                    {
                        newExcelData[i, j] = tempExcelData[i, j];
                    }
                }
                //Log.Error($"tempExcelData:{tempExcelData.GetLength(0)},{tempExcelData.GetLength(1)}, newLength0:{newLength0},{newLength1}");
                return isDirtyAll;
            }
            catch (Exception ex)
            {
                Log.Exception(exceptionGraph,ex);
                return false;
            }
        }

        /// <summary>
        /// 将数据写回到excel，该函数会遍历所有configs的数据然后调用excel文件和sheet的回调接口
        /// 以确定config数据所在的excel文件和sheet
        /// </summary>
        /// <param name="excelPathAction">config数据所在excel文件的接口</param>
        /// <param name="excelSheetAction">config数据所在sheet的接口</param>
        /// <param name="configs">需要写回到excel的配置数据</param>
        /// <param name="fromJenkins"></param>
        public bool WriteExcel(Func<object, string> excelPathAction, Func<object, string> excelSheetAction, IList configs, bool showErrorDialog)
        {
            bool bWriteOk = true;

            Dictionary<string, Dictionary<string, List<object>>> tempExcelData = new Dictionary<string, Dictionary<string, List<object>>>();
            for (int i = 0, n = configs.Count; i < n; i++)
            {
                string excelFile = excelPathAction?.Invoke(configs[i]);
                string sheetName = excelSheetAction?.Invoke(configs[i]);
                if (string.IsNullOrEmpty(excelFile) || string.IsNullOrEmpty(sheetName))
                {
                    continue;
                }
                tempExcelData.TryGetValue(excelFile, out var fileInfo);
                if (fileInfo == null)
                {
                    fileInfo = new Dictionary<string, List<object>>();
                    tempExcelData.Add(excelFile, fileInfo);
                }
                fileInfo.TryGetValue(sheetName, out var sheetInfo);
                if (sheetInfo == null)
                {
                    sheetInfo = new List<object>();
                    fileInfo.Add(sheetName, sheetInfo);
                }
                sheetInfo.Add(configs[i]);
            }

            foreach (var kvp in tempExcelData)
            {
                string excelPath = kvp.Key;
                try
                {
                    var path = Path.GetFullPath(excelPath);
                    var fileName = Path.GetFileName(path);
                    if (Utils.IsFileOpened(path))
                    {
#if UNITY_EDITOR
                        if (showErrorDialog)
                        {
                            EditorUtility.DisplayDialog("导出失败", $"【{fileName}】表格处于被打开状态/只读模式，请先关闭！", "确定");
                        }
#endif
                        Log.Error($"【{fileName}】表格处于被打开状态/只读模式，请先关闭！");
                        bWriteOk = false;
                        continue;
                    }
                    WriteExcel(excelPath, kvp.Value);
                }
                catch (Exception ex)
                {
#if UNITY_EDITOR
                    if (showErrorDialog)
                    {
                        EditorUtility.DisplayDialog("导出失败", $"代码存在报错，请查看Concole日志信息", "确定");
                    }
#endif
                    Log.Exception(ex);
                    bWriteOk = false;
                }
            }
            return bWriteOk;
        }

        /// <summary>
        /// 导出数据到单个excel文件，sheetConfigDic -> <sheet,List<configobj>>
        /// </summary>
        /// <param name="excelPath">excel文件路径</param>
        /// <param name="sheetConfigDic">以sheet为key，config数据列表为value的数据集合</param>
        /// <returns></returns>
        public bool WriteExcel(string excelPath, Dictionary<string, List<object>> sheetConfigDic)
        {
            var path = Path.GetFullPath(excelPath);
            var fileName = Path.GetFileName(path);
            if (Utils.IsFileOpened(path))
            {
                Log.Error($"【{fileName}】表格处于被打开状态/只读模式，请先关闭！");
                return false;
            }
            Dictionary<string, object[,]> newExcelDataDic = new Dictionary<string, object[,]>();
            foreach (var sheetKvp in sheetConfigDic)
            {
                var configObjects = sheetKvp.Value;
                string excelSheet = sheetKvp.Key;
                if (configObjects.Count == 0)
                {
                    continue;
                }
                var type = configObjects[0].GetType();
                var listConfig = configObjects;
                var configName = type.Name;
                object[,] newExcelData = null;

                var isModified = ModifyExcelData(excelPath, excelSheet, configName, listConfig, ref newExcelData);
                if (isModified)
                {
                    newExcelDataDic.Add(excelSheet, newExcelData);
                }
            }

            if (newExcelDataDic.Count > 0)
            {
                ExcelHelper.WriteExcelSheet(path, newExcelDataDic, false);
                return true;
            }
            else
            {
                Log.Warning($"{fileName}】数据没有修改，不用写文件");
            }

            return false;
        }

        /// <summary>
        /// 导出数据到单张表，只能存储到sheet和配置表Config的名称相同的excel中
        /// </summary>
        /// <param name="excelPath">表格路径</param>
        /// <param name="configs">表格数据列表（支持同表不同sheet数据冗杂）</param>
        /// <returns></returns>
        public bool WriteExcel(string excelPath, List<object> configs, ExcelFlags flags = ExcelFlags.CheckMembers | ExcelFlags.FormatData)
        {
            try
            {
                var path = Path.GetFullPath(excelPath);
                var excelName = Path.GetFileName(path);
                if (Utils.IsFileOpened(path))
                {
                    Log.Error($"【{excelName}】表格处于被打开状态/只读模式，请先关闭！");
                    return false;
                }
                if (configs == null || configs.Count == 0)
                {
                    Log.Error($"【{excelName}】写入数据失败，无有效数据！");
                    return false;
                }
                // 解析数据成Excel格式<SkillConfig, List<object>>
                var type2List = DecomposeConfigs(configs);
                // 同步数据到内存中的表格数据，可能存在不同表之间约束引用，如。SkillConfig写入表格数据依赖SkillTagConfig的描述数据等
                foreach (var tmpConfigs in type2List.Values)
                {
                    foreach (var tmpConfig in tmpConfigs)
                    {
                        // 同步数据到缓存表格记录
                        DesignTable.UpdateConfigData(tmpConfig, true);
                    }
                }
                var sb = new StringBuilder();
                // 避免多次打开Excel耗时，优化处理只打开一次Excel
                ExcelHelper.ProcessExcelSheets(excelPath, (workSheet) =>
                {
                    var sheetName = workSheet.Name;
                    if (!ToolHelper.TryGetTableName(sheetName, ProjectConfig.TableSeaperator, out var configName))
                    {
                        return;
                    }
                    // 仅处理包含类型的sheet内容
                    if (type2List.TryGetValue(configName, out var listConfig))
                    {
                        object[,] newExcelData = null;
                        var isModified = ModifyExcelData(excelPath, sheetName, configName, listConfig, ref newExcelData, (isNewLine, info, process) =>
                        {
                            //Log.Info($"[{(int)(process * 100)}%] 写入表格数据:{info}");
#if UNITY_EDITOR
                            EditorUtility.DisplayProgressBar($"写入表格数据...[{excelName}]", info, process);
#endif
                        }, flags);
                        if (isModified)
                        {
                            if (flags.HasFlag(ExcelFlags.CheckMembers)) // 检测是否存在excel中没有字段，但是config中有字段的情况
                            {
                                var membersMissing = GetExcelMembersMissingName(excelPath, sheetName, configName);
                                if (membersMissing?.Count > 0)
                                {
                                    for (int i = 0; i < membersMissing.Count; i++)
                                    {
                                        var tip = membersMissing[i];
                                        sb.AppendLine(membersMissing[i]);
                                    }
                                }
                            }
                        }
                        var rows = newExcelData.GetLength(0);
                        var rStart = newExcelData.GetLowerBound(0);
                        var colums = newExcelData.GetLength(1);
                        var cStart = newExcelData.GetLowerBound(1);
                        var cells = workSheet.Cells;
                        for (var i = 1; i <= rows; i++)
                        {
                            for (var j = 1; j <= colums; j++)
                                cells[i, j].Value = newExcelData[i + rStart - 1, j + cStart - 1];
                        }
                        if (flags.HasFlag(ExcelFlags.FormatData))
                        {
                            ExcelHelper.FormatExcelSheet(workSheet, rows);
                        }
                    }
                });
#if UNITY_EDITOR
                if (sb.Length > 0 && LocalSettings.IsProgramer())
                {
                    // 仅做提示
                    EditorUtility.DisplayDialog("警告", sb.ToString(), "好的");
                }
#endif
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
            finally
            {
#if UNITY_EDITOR
                EditorUtility.ClearProgressBar();
#endif
            }
        }

        /// <summary>
        /// TODO-删除excel中的数据
        /// </summary>
        public void DelExcelData(string excelPath, string excelSheet, List<object> configs)
        {

        }

        /// <summary>
        /// 按config类型分类(例："SkillConfig", List<SkillConfig>)
        /// </summary>
        public Dictionary<string, List<object>> DecomposeConfigs(List<object> configs)
        {
            var typeName2List = new Dictionary<string, List<object>>();
            if (configs != null)
            {
                foreach (var config in configs)
                {
                    if (config == null)
                    {
                        continue;
                    }
                    var configType = config.GetType();
                    if (!typeName2List.TryGetValue(configType.Name, out var list))
                    {
                        list = new List<object>();
                        typeName2List.Add(configType.Name, list);
                    }
                    list.Add(config);
                }
            }
            return typeName2List;
        }

#if UNITY_EDITOR
        #region 检测文件变化，刷新数据

        // 表格文件变动监听
        private static FileSystemWatcher fileWatcherExcel;
        private static bool isDoFileChange = false;
        private static List<string> changeFilesPathCache = new List<string>();

        private static Action<List<string>> onExcelChanged;
        public static event Action<List<string>> OnExcelChanged
        {
            add
            {
                onExcelChanged -= value;
                onExcelChanged += value;
            }
            remove { onExcelChanged -= value; }
        }
        [Conditional("UNITY_EDITOR")]
        public static void TurnFileWatcher(bool on)
        {
            if (fileWatcherExcel == null)
            {
                fileWatcherExcel = new FileSystemWatcher(Constants.ExcelPathPrefix)
                {
                    IncludeSubdirectories = false,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Security,
                };
                fileWatcherExcel.Created += OnFileChangedExcel;
                fileWatcherExcel.Changed += OnFileChangedExcel;
                fileWatcherExcel.Deleted += OnFileChangedExcel;
                fileWatcherExcel.Renamed += OnFileChangedExcel;
            }
            fileWatcherExcel.EnableRaisingEvents = on;
        }
        private static readonly List<string> ignoreFiles = new List<string>
        {
            "~$", "LocalizeTable.xlsx"
        };
        private static void OnFileChangedExcel(object sender, FileSystemEventArgs e)
        {
            var path = e.FullPath;
            // 过滤下临时文件变动通知，如：~$BattleConfig.xlsx
            if (ignoreFiles.All(f => { return !path.Contains(f); }))
            {
                if ((path.EndsWith(".xlsx", StringComparison.Ordinal) || path.EndsWith(".json", StringComparison.Ordinal)))
                {
                    lock (changeFilesPathCache)
                    {
                        changeFilesPathCache.Add(path);
                    }
                    if (!isDoFileChange)
                    {
                        isDoFileChange = true;
                        EditorCoroutineRunner.StartEditorCoroutine(FileChangeDelayReInit(e));
                    }
                }
            }
        }
        /// <summary>
        /// 延迟5秒，重新初始化编辑器数据
        /// </summary>
        /// <returns></returns>
        private static IEnumerator FileChangeDelayReInit(FileSystemEventArgs e)
        {
            if (!isDoFileChange || changeFilesPathCache.Count == 0)
            {
                yield break;
            }

            yield return new WaitForSeconds(5);

            lock (changeFilesPathCache)
            {
                var pathCache = new List<string>(changeFilesPathCache);
                changeFilesPathCache.Clear();
                //延迟5秒后，集合可能出现数量减少
                if (pathCache.Count == 0)
                {
                    yield break;
                }
                var matchPath = false;
                try
                {
                    for (int i = pathCache.Count - 1; i >= 0; --i)
                    {
                        var path = pathCache.ExGet(i, null);
                        if (string.IsNullOrEmpty(path))
                        {
                            continue;
                        }
                        if (path.EndsWith(Constants.ConfigJsonName))
                        {
                            matchPath = true;
                            break;
                        }
                        foreach (var manager in GraphHelper.GetEditorManagers())
                        {
                            if (path.EndsWith(manager.Setting.ExcelName))
                            {
                                matchPath = true;
                                break;
                            }
                        }
                        if (matchPath) break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                    matchPath = true;
                }
                if (matchPath)
                {
                    // 只有在编辑器表格，以及json文件变动才出发更新
                    Inst.OnExcelChange();
                }
                onExcelChanged?.Invoke(pathCache);
                Log.Info("表格数据变化，重新初始化编辑器数据");
                isDoFileChange = false;
            }
        }
        #endregion
#endif

        #region 表格数据排序 && 清空空行数据
        /// <summary>
        /// 清理Sheet所有数据-保留头两行
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="filter">sheet过滤函数，为空则全部清空</param>
        public void CleanSheets(string excelPath, Func<string, bool> filter)
        {
            Utils.SafeCall(() =>
            {
                var path = Path.GetFullPath(excelPath);
                var fileName = Path.GetFileName(path);
                if (Utils.IsFileOpened(path))
                {
                    Log.Error($"【{fileName}】表格处于被打开状态/只读模式，请先关闭！");
                    return;
                }

                ExcelHelper.ProcessExcelSheets(path, (sheet) =>
                {
                    var sheetName = sheet.Name;
                    bool valid = false;
                    if (filter == null)
                    {
                        foreach (var configName in DesignTable.EditorConfigNames)
                        {
                            if (sheetName.Contains(configName))
                            {
                                valid = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        valid = filter.Invoke(sheetName);
                    }
                    if (valid)
                    {
                        Utils.WatchTime($"清理表格：{sheetName}", () =>
                        {
                            var rows = sheet.Dimension.Rows;
                            // 删除多余行
                            //for (int i = rows; i > EXCEL_HEAD_ROWS; --i)
                            //{
                            //    sheet.DeleteRow(i);
                            //}
                            sheet.DeleteRow(EXCEL_HEAD_ROWS + 1, rows);
                        });
                    }
                }, true);
            });
        }

        /// <summary>
        /// 仅修改表格数据，即存在数据更新，不存在不做写入
        /// </summary>
        /// <param name="excelPath">excel路径</param>
        /// <param name="configsAll">需要同步到excel的表格数据列表</param>
        public void ModifyExcelCell(string excelPath, List<object> configsAll)
        {
            WriteExcel(excelPath, configsAll, /*ExcelFlags.CheckMembers | */ExcelFlags.OnlyModify);
        }
        #endregion

        #region 遍历表格文件
        /// <summary>
        /// 遍历excel目录所有表格
        /// </summary>
        /// <param name="action">遍历action</param>
        /// <param name="condition">excel过滤条件</param>
        public void ProcessExcels(Action<string, int, int> action, Func<string, bool> condition = null)
        {
            if (action == null) return;
            var excelFiles = Directory.GetFiles(Constants.ExcelPathPrefix, "*.xlsx", SearchOption.TopDirectoryOnly);
            for (int i = 0, length = excelFiles.Length; i < length; i++)
            {
                var excelFile = excelFiles[i];
                var excelFileName = Path.GetFileName(excelFile);
                // 过滤临时文件
                if (excelFileName.StartsWith("~")) continue;
                if (condition != null && !condition.Invoke(excelFile)) continue;
                action.Invoke(excelFile, i, length);
            }
        }
        #endregion

        #region 表格配置相关数据
        private static Dictionary<string, string> tableName2SingleKeyName = new Dictionary<string, string>();
        public string GetTableKey(string tableName)
        {
            if (!tableName2SingleKeyName.TryGetValue(tableName, out var keyName))
            {
                keyName = ProjectConfig?.GetTable(tableName)?.GetKeySingle()?.Name ?? "ID";
                tableName2SingleKeyName.Add(tableName, keyName);
            }
            return keyName;
        }
        #endregion
    }
}
