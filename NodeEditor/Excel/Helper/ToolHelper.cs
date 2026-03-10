using System.Text.RegularExpressions;

namespace NodeEditor
{
    public static class ToolHelper
    {
        /// <summary>
        /// 从sheet名称获取表格名称，主要去掉-
        /// </summary>
        public static bool TryGetTableName(string sheetName, string tableSeaperator, out string tableName)
        {
            tableName = sheetName;
            // 如果是默认的名字，那么跳过，需要修改改为有意义的名字
            if (sheetName.StartsWith("Sheet"))
                return false;

            // 过滤掉子表然后提取主表名称，如：[Item-Level] => [Item]
            var i = sheetName.IndexOf(tableSeaperator);
            if (i > 0) tableName = sheetName.Substring(0, i);
            if (tableName != null && Regex.IsMatch(tableName, @"^[a-zA-Z]+[a-zA-Z0-9_]*$"))
            {
                return true;
            }
            tableName = null;
            return false;
        }
    }
}