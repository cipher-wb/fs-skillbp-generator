using System;
using System.Collections.Generic;
using System.Reflection;

namespace NodeEditor
{
    /// <summary>
    /// 表格相关数据汇总
    /// </summary>
    public class CacheExcelData
    {
        /// <summary>
        /// Excel成员变量名称解析缓存<sheet,变量[]>，如<SkillConfig, {技能名,}>
        /// </summary>
        public Dictionary<string, PropertyInfo[]> ExcelMembersProp;

        /// <summary>
        /// Excel成员变量解析缓存<sheet,变量[]>，如<SkillConfig, {SkillName,}>
        /// </summary>
        public Dictionary<string, string[]> ExcelMembersName;

        /// <summary>
        /// Excel成员变量缺失缓存<sheet,变量[]>，如<SkillConfig, {技能名XXX,}>，即<技能名XXX>列缺失
        /// </summary>
        public Dictionary<string, List<string>> ExcelMembersMissingName;

        ///// <summary>
        ///// Excel ID解析缓存<ID, Index>
        ///// </summary>
        //public Dictionary<string, int> ExcelIDs;

        /// <summary>
        /// Excel ID解析缓存<sheet,<ID, Index>>
        /// </summary>
        public Dictionary<string, Dictionary<string, int>> ExcelIDs;

        /// <summary>
        /// Excel数据缓存，key是excel的sheet,value是在该sheet下的数据
        /// </summary>
        private Dictionary<string, object[,]> excelData;

        /// <summary>
        /// Excel路径
        /// </summary>
        public string ExcelPath;

        public void SetExcelData(string sheet, object[,] data)
        {
            if (excelData == null)
            {
                excelData = new Dictionary<string, object[,]>();
            }
            excelData[sheet] = data;
        }

        public object[,] GetDataBySheet(string sheet)
        {
            if (excelData == null)
            {
                return null;
            }
            excelData.TryGetValue(sheet, out var data);
            return data;
        }

        /// <summary>
        /// 获取列号（从1开始）
        /// </summary>
        /// <param name="sheetName">sheet名，如：SkillConfig-Monster</param>
        /// <param name="memberName">变量名，如：ID</param>
        /// <returns></returns>
        public int GetColumn(string sheetName, string memberName)
        {
            if (ExcelMembersName.TryGetValue(sheetName, out var memberNameIndexs))
            {
                return Array.IndexOf(memberNameIndexs, memberName) + 1;
            }
            return 0;
        }

        // 获取行号
        public int GetRow(string sheetName, string id)
        {
            if (ExcelIDs.TryGetValue(sheetName, out var dic))
            {
                if (dic.TryGetValue(id, out var row))
                {
                    return row;
                }
            }
            return 0;
        }

        // 获取单元格数据
        public string GetCellData(string sheetName, string id, string memberName)
        {
            if (excelData.TryGetValue(sheetName, out var data))
            {
                int row = GetRow(sheetName, id);
                int col = GetColumn(sheetName, memberName);
                var cellData = data[row, col]?.ToString().Trim();
                return cellData;
            }
            return default;
        }

        public bool IsExists(string id, out (string, int) sheetName2Row)
        {
            sheetName2Row = default;
            if (ExcelIDs == null)
            {
                return false;
            }
            foreach (var kv in ExcelIDs)
            {
                if (kv.Value.TryGetValue(id, out var row))
                {
                    sheetName2Row = (kv.Key, row);
                    return true;
                }
            }
            return false;
        }

        public Dictionary<string, int> GetExcelIDs(string sheetName)
        {
            if (ExcelIDs == null)
            {
                return null;
            }
            if (ExcelIDs.TryGetValue(sheetName, out var ids))
            {
                return ids;
            }
            return null;
        }

        public object[,] GetDataBySheet(params string[] sheets)
        {
            if (excelData == null)
            {
                return null;
            }

            if (sheets == null || sheets.Length == 0)
            {
                return null;
            }
            int len0 = 0;
            int len1 = 0;
            for (int i = 0; i < sheets.Length; i++)
            {
                if (excelData.TryGetValue(sheets[i], out var data))
                {
                    if (data != null)
                    {
                        len1 += data.GetLength(0);
                        int len = data.GetLength(1);
                        if (len > len0)
                        {
                            len0 = len;
                        }
                    }
                }
            }
            object[,] tmp = new object[len0, len1];
            int index0 = 0;
            for (int i = 0; i < sheets.Length; i++)
            {
                if (excelData.TryGetValue(sheets[i], out var data))
                {
                    if (data != null)
                    {
                        len0 += data.GetLength(0);
                        len1 = data.GetLength(1);
                        for (int k = 0; k < len0; k++)
                        {
                            for (int j = 0; j < len1; j++)
                            {
                                tmp[index0, j] = data[k, j];
                            }
                            index0++;
                        }
                    }
                }
            }

            return tmp;
        }

        public object[,] GetAllData()
        {
            if (excelData == null)
            {
                return null;
            }
            int len0 = 0;
            int len1 = 0;
            foreach (var kvp in excelData)
            {
                if (kvp.Value == null)
                {
                    continue;
                }
                len0 += kvp.Value.GetLength(0);
                int len = kvp.Value.GetLength(1);
                if (len > len1)
                {
                    len1 = len;
                }
            }

            object[,] tmp = new object[len0, len1];
            int index0 = 0;
            foreach (var kvp in excelData)
            {
                object[,] datas = kvp.Value;
                if (datas == null)
                {
                    continue;
                }
                len0 = datas.GetLength(0);
                len1 = datas.GetLength(1);
                for (int i = 0; i < len0; i++)
                {
                    for (int j = 0; j < len1; j++)
                    {
                        tmp[index0, j] = datas[i, j];
                    }
                    index0++;
                }
            }

            return tmp;
        }
    }
}
