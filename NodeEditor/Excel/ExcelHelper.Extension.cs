using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NodeEditor
{
    public partial class ExcelHelper
    {
        static object[,] ReadExcelXml(string fileName, Func<string, bool> sheetValidCondition, int maxRow)
        {
            using (var package = new ExcelPackage(new FileInfo(fileName)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                    throw new Exception("None data readed.");

                var workSheet = package.Workbook.Worksheets.FirstOrDefault(sheet => sheetValidCondition(sheet.Name));
                if (workSheet == null)
                    throw new Exception("Sheet not found in " + fileName);

                return ReadSheetData(workSheet, maxRow);
            }
        }

        /// <summary>
        /// 修改Excel数据
        /// </summary>
        public static void ModifiyExcel(string fileName, Func<string, bool> sheetValidCondition, Func<ExcelRangeBase, bool> cellUpdate)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(fileName)))
                {
                    var dataWrited = false;
                    var formulaRanges = new Dictionary<string, string>();
                    for (var i = 0; i < package.Workbook.Worksheets.Count; i++)
                    {
                        var workSheet = package.Workbook.Worksheets[i];
                        if (!sheetValidCondition(workSheet.Name)) continue;

                        foreach (var cell in workSheet.Cells)
                        {
                            var dirty = cellUpdate(cell);
                            dataWrited |= dirty;
                        }
                    }
                    if (dataWrited) package.Save();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ModifiyExcel failed, excelPath:{fileName}\n{ex}");
            }
        }

        /// <summary>
        /// 处理ExcelSheet
        /// </summary>
        public static void ProcessExcelSheets(string fileName, Action<ExcelWorksheet> workSheetAction, bool saveFile = true)
        {
            try
            {
                if (workSheetAction == null)
                {
                    return;
                }
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);

                if (File.Exists(fileName))
                    File.Copy(fileName, tempFile);

                using (var package = new ExcelPackage(new FileInfo(fileName)))
                {
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        workSheetAction?.Invoke(sheet);
                    }
                    if (saveFile) package.Save();
                }
            }
            catch (IOException)
            {
                // 这里主要是excel文件打开的情况下，有读取失败的问题。
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);
                File.Copy(fileName, tempFile);
                // 拷贝到一个临时文件，重新尝试读取。
                ProcessExcelSheets(tempFile, workSheetAction);
            }
            catch (Exception ex)
            {
                throw new Exception($"排序错误：{fileName}\n 异常：{ex}");
            }
        }

        /// <summary>
        /// 排序+清理
        /// </summary>
        public static void FormatExcelSheet(ExcelWorksheet workSheet, int maxRow = 0)
        {
            try
            {
                var cells = workSheet.Cells;
                if (maxRow <= 0)
                {
                    maxRow = workSheet.Dimension.Rows;
                }
                else
                {
                    // 清理超出行
                    for (int i = workSheet.Dimension.Rows; i > maxRow; --i)
                    {
                        workSheet.DeleteRow(i);
                    }
                }
                var rows = maxRow;
                var colums = workSheet.Dimension.Columns;
                if (colums >= MaxColumneNum)
                    throw new Exception($"表格列数，超过最大数量允许数量：{workSheet.Dimension.Columns}>{MaxColumneNum}");

                var headRow = ExcelManager.EXCEL_HEAD_ROWS;
                // 默认前两行为名称+变量名，及第一列为ID列
                // 排序，升序
                if (rows <= headRow) return;
                var letter = GetColumeLetter(colums);
                var address = $"A{headRow + 1}:{letter}{rows}";
                // https://github.com/EPPlusSoftware/EPPlus/wiki/Sorting-ranges
                // TODO cell数据写入为字符串，待处理（string->int）
                for (int i = headRow + 1; i <= rows; i++)
                {
                    if (cells[i, 1].Value is string strValue && int.TryParse(strValue, out var intValue))
                    {
                        cells[i, 1].Value = intValue;
                    }
                }
                // 清空sort，避免异常：Too many sort conditions added, max number of conditions is 64
                //SortState 可能为空
                workSheet.SortState?.Clear();
                // 排序
                cells[address].Sort(0);
                // 清理空行
                for (int i = rows; i > headRow; --i)
                {
                    if (cells[i, 1].Value == null)
                    {
                        workSheet.DeleteRow(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"FormatExcelSheet failed, {workSheet.Name}\n{ex}");
            }
        }
    }
}
