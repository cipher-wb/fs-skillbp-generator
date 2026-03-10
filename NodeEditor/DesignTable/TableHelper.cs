using System;
using System.Collections.Generic;

namespace NodeEditor
{
    /// <summary>
    /// 表格-辅助函数
    /// </summary>
    public sealed class TableHelper
    {
        // 缓存表格名-类型字典
        private static Dictionary<string, Type> tableFullName2TypeCache = new Dictionary<string, Type>();

        /// <summary>
        /// 获取表格版本
        /// </summary>
        /// <param name="tableName">表格名:SkillConfig</param>
        /// <returns></returns>
        public static string GetTableTash(string tableName)
        {
            //return Utils.GetTableType(ToTableManager(tableName)).GetField(nameof(SkillConfigManager.TableTash)).GetValue(null) as string;
            return TableDR.TableHashes.Get(tableName);
        }

        /// <summary>
        /// 表格名转Manager
        /// </summary>
        /// <param name="tableName">表格名:SkillConfig</param>
        /// <returns>TableDR.SkillConfigManager</returns>
        public static string ToTableManager(string tableName)
        {
            return $"{Constants.TableNameSpace}.{tableName}Manager";
        }

        /// <summary>
        /// 表格全名
        /// </summary>
        /// <param name="tableName">表格名:SkillConfig</param>
        /// <returns>TableDR.SkillConfig</returns>
        public static string ToTableFullName(string tableName)
        {
            return $"{Constants.TableNameSpace}.{tableName}";
        }

        /// <summary>
        /// 获取表格类型
        /// </summary>
        /// <param name="typeFullName">类型全名</param>
        /// <returns>表格类型</returns>
        public static Type GetTableType(string typeFullName)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                return null;
            }
            if (!tableFullName2TypeCache.TryGetValue(typeFullName, out var type))
            {
                // 先找Hotfix
                type = typeof(TableDR.EnumUtility).Assembly.GetType(typeFullName);
                if (type == null)
                {
                    // 再找NotHotfix
                    type = typeof(TableDR.EnumUtility_NotHotfix).Assembly.GetType(typeFullName);
                }
                tableFullName2TypeCache[typeFullName] = type;
            }
            return type;
        }

        public static Type GetTableTypeByName(string name)
        {
            var fullName = ToTableFullName(name);
            return GetTableType(fullName);
        }
    }
}