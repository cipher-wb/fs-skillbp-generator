using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using OfficeOpenXml;

namespace NodeEditor
{
    public static partial class ExcelHelper
    {
        public const int LetterCount = 26;
        public const int MaxListFormulaChar = 255;
        public const string FormulaSheeet = "SheetTableTool";

        /// <summary>
        /// 最大excel文件的列数，如果你用的插件需要超过此数值，可动态修改。
        /// </summary>
        public static int MaxColumneNum = 100;


        static ExcelHelper()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// 获取excel的列名称
        /// </summary>
        /// <param name="columnsCount"></param>
        /// <returns></returns>
        public static string GetColumeLetter(int columnsCount)
        {
            string ret = "";
            while (columnsCount > LetterCount)
            {
                var num = (columnsCount - 1) / LetterCount;

                ret += (char)('A' + num - 1);
                columnsCount -= num * LetterCount;
            }
            return ret + (char)('A' + columnsCount - 1);
        }

        /// <summary>
        /// 读取一个sheet的数据。
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="maxRow"></param>
        /// <returns></returns>
        static object[,] ReadSheetData(ExcelWorksheet workSheet, int maxRow)
        {
            var cells = workSheet.Cells;
            if (null == workSheet.Dimension)
                return new object[0, 0];

            if (maxRow <= 0) maxRow = workSheet.Dimension.Rows;
            if (workSheet.Dimension.Columns >= MaxColumneNum)
                throw new Exception($"表格列数，超过最大数量允许数量：{workSheet.Dimension.Columns}>{MaxColumneNum}");

            var ret = Array.CreateInstance(typeof(object), new int[] { maxRow, workSheet.Dimension.Columns }, new int[] { 1, 1 }) as object[,];
#if false
            for (var i = 1; i <= maxRow; i++)
            {
                for (var j = 1; j <= workSheet.Dimension.Columns; j++)
                    ret[i, j] = cells[i, j].Value;
            }
#else
            var rowOffset = workSheet.Dimension.Start.Row - 1;
            var columnOffset = workSheet.Dimension.Start.Column - 1;
            foreach (var cell in workSheet.Cells)
                ret[cell.Start.Row - rowOffset, cell.Start.Column - columnOffset] = cell.Value;
#endif
            return ret;
        }

        /// <summary>
        /// 读取xml里面excle的数据。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="maxRow"></param>
        /// <returns></returns>
        public static object[,] ReadExcelXml(string fileName, string sheetName, int maxRow)
        {
            using (var package = new ExcelPackage(new FileInfo(fileName)))
            {
                if (package.Workbook.Worksheets.Count == 0)
                    throw new Exception("None data readed.");

                var workSheet = package.Workbook.Worksheets.FirstOrDefault(sheet => string.IsNullOrEmpty(sheetName) || sheet.Name == sheetName);
                if (workSheet == null)
                    throw new Exception("Sheet not found in " + fileName + " / " + sheetName);

                return ReadSheetData(workSheet, maxRow);
            }
        }

        /// <summary>
        /// 使用OLEDB读取excel，在xml读取失败的情况下。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        static object[,] ReadExcelEx(string fileName, string sheetName)
        {
            var strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + ";" + "Extended Properties='Excel 8.0;HDR=YES; IMEX=1';";
            var conn = new OleDbConnection(strConn);
            conn.Open();

            if (string.IsNullOrEmpty(sheetName))
                sheetName = "Sheet1";

            var strExcel = "select * from [" + sheetName + "!]";
            var myCommand = new OleDbDataAdapter(strExcel, strConn);
            var dataTable = new DataTable();
            myCommand.Fill(dataTable);

            var ret = Array.CreateInstance(typeof(object), new int[] { dataTable.Rows.Count + 1, dataTable.Columns.Count }, new int[] { 1, 1 }) as object[,];
            for (var j = 0; j < dataTable.Columns.Count; j++)
            {
                var columnName = dataTable.Columns[j].ColumnName;
                if (Regex.IsMatch(columnName, "F\\d*")) // 去除莫名奇妙空格为：F5, F6....这样的东西。
                    continue;
                ret[1, j + 1] = columnName;
            }

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                var row = dataTable.Rows[i];
                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    var value = row[dataTable.Columns[j]];
                    ret[i + 2, j + 1] = value;
                }
            }

            return ret;
        }

        /// <summary>
        /// 读取excel文件。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static object[,] ReadExcel(string fileName, string sheetName = null, int maxRow = 0)
        {
            try
            {
                return ReadExcelXml(fileName, sheetName, maxRow);
            }
            catch (IOException ex1)
            {
                Log.Info(ex1.ToString());
                // 这里主要是excel文件打开的情况下，有读取失败的问题。
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);
                File.Copy(fileName, tempFile);
                // 拷贝到一个临时文件，重新尝试读取。
                return ReadExcel(tempFile, sheetName);
            }
            catch (Exception ex)
            {
                try
                {
                    return ReadExcelEx(fileName, sheetName);
                }
                catch (Exception)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 读取excel里面的的数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="filter"></param>
        /// <param name="dataAction"></param>
        /// <param name="maxRaw"></param>
        /// <returns></returns>
        public static int ReadExcelSheets(string fileName, Func<string, bool> filter, Action<string, object[,]> dataAction = null, int maxRaw = 0)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(fileName)))
                {
                    return package.Workbook.Worksheets
                        .Count(sheet =>
                        {
                            if (!filter(sheet.Name))
                                return false;

                            dataAction?.Invoke(sheet.Name, ReadSheetData(sheet, maxRaw));
                            return true;
                        });
                }
            }
            catch (IOException)
            {
                // 这里主要是excel文件打开的情况下，有读取失败的问题。
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);
                File.Copy(fileName, tempFile);
                // 拷贝到一个临时文件，重新尝试读取。
                return ReadExcelSheets(tempFile, filter, dataAction);
            }
            catch (Exception ex)
            {
                throw new Exception($"读取错误：{fileName}\n 异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 简单的写入excel函数。
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void WriteExcel(string fileName, Dictionary<string, object[,]> sheetDatas, Action<string> progress = null)
        {
            try
            {
                // 先弄一个临时文件过来
                var tmpFile = Path.GetTempFileName();
                File.Delete(tmpFile);

                // 创建excel写入进去
                using (var package = new ExcelPackage(new FileInfo(tmpFile)))
                {
                    // 写入所有的表格数据。
                    sheetDatas.Count(sheetData =>
                    {
                        progress?.Invoke(sheetData.Key);

                        var workSheet = package.Workbook.Worksheets.Add(sheetData.Key);
                        var cells = workSheet.Cells;
                        var rows = sheetData.Value.GetLength(0);
                        var rStart = sheetData.Value.GetLowerBound(0);
                        var colums = sheetData.Value.GetLength(1);
                        var cStart = sheetData.Value.GetLowerBound(1);
                        for (var i = 1; i <= rows; i++)
                        {
                            for (var j = 1; j <= colums; j++)
                                cells[i, j].Value = sheetData.Value[i + rStart - 1, j + cStart - 1];
                        }
                        return true;
                    });
                    package.Save();
                }

                // 删除之前的excel文件
                if (File.Exists(fileName))
                    File.Delete(fileName);
                File.Move(tmpFile, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static bool WriteExcelSheet(string fileName, Dictionary<string, object[,]> sheetData, bool isFormat)
        {
            try
            {
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);

                if (File.Exists(fileName))
                    File.Copy(fileName, tempFile);

                using (var package = new ExcelPackage(new FileInfo(tempFile)))
                {
                    foreach (var kvp in sheetData)
                    {
                        var workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == kvp.Key);
                        if (workSheet == null)
                            workSheet = package.Workbook.Worksheets.Add(kvp.Key);

                        // 影响单元格格式
                        //if (workSheet.Dimension != null)
                        //    workSheet.Cells.Clear();
                        object[,] datas = kvp.Value;
                        var cells = workSheet.Cells;
                        var rows = datas.GetLength(0);
                        var rStart = datas.GetLowerBound(0);
                        var colums = datas.GetLength(1);
                        var cStart = datas.GetLowerBound(1);
                        for (var i = 1; i <= rows; i++)
                        {
                            for (var j = 1; j <= colums; j++)
                                cells[i, j].Value = datas[i + rStart - 1, j + cStart - 1];
                        }
                        if (isFormat)
                        {
                            FormatExcelSheet(workSheet, rows);
                        }
                    }
                    package.SaveAs(new FileInfo(fileName));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 写入一个excel文件的sheet内容，不修改其他sheet
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheetData"></param>
        /// <returns></returns>
        public static bool WriteExcelSheet(string fileName, string sheetName, object[,] sheetData, bool isFormat)
        {
            try
            {
                var tempFile = Path.GetTempFileName();
                File.Delete(tempFile);

                if (File.Exists(fileName))
                    File.Copy(fileName, tempFile);

                using (var package = new ExcelPackage(new FileInfo(tempFile)))
                {
                    var workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName);
                    if (workSheet == null)
                        workSheet = package.Workbook.Worksheets.Add(sheetName);

                    // 影响单元格格式
                    //if (workSheet.Dimension != null)
                    //    workSheet.Cells.Clear();

                    var cells = workSheet.Cells;
                    var rows = sheetData.GetLength(0);
                    var rStart = sheetData.GetLowerBound(0);
                    var colums = sheetData.GetLength(1);
                    var cStart = sheetData.GetLowerBound(1);
                    for (var i = 1; i <= rows; i++)
                    {
                        for (var j = 1; j <= colums; j++)
                            cells[i, j].Value = sheetData[i + rStart - 1, j + cStart - 1];
                    }
                    if (isFormat)
                    {
                        FormatExcelSheet(workSheet, rows);
                    }
                    package.SaveAs(new FileInfo(fileName));
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Log.Error(ex.ToString());
                return false;
            }
        }

        public static void ModifiyExcel(string fileName,
            Func<string, Dictionary<string, string>> sheetComments,
            Func<string, Dictionary<string, string[]>> sheetEnumList,
            List<string> warningList)
        {
            using (var package = new ExcelPackage(new FileInfo(fileName)))
            {
                var dataWrited = false;
                var formulaRanges = new Dictionary<string, string>();
                for (var i = 0; i < package.Workbook.Worksheets.Count; i++)
                {
                    var workSheet = package.Workbook.Worksheets[i];
                    if (workSheet.Name == FormulaSheeet) continue;

                    var comments = sheetComments(workSheet.Name);
                    var enmunList = sheetEnumList(workSheet.Name);
                    if (comments == null && enmunList == null) continue;

                    var dimension = workSheet.Dimension;
                    for (var j = 1; j <= dimension.Columns; j++)
                    {
                        var headerCell = workSheet.Cells[1, j];
                        if (comments != null && comments.TryGetValue(headerCell.Text, out var comm))
                        {
                            var excelCommet = headerCell.Comment;
                            if (excelCommet == null)
                                excelCommet = headerCell.AddComment(comm);

                            // 检测一下是否完全一致，不需要修改
                            if (excelCommet.RichText.Text != comm)
                            {
                                excelCommet.RichText.Clear();
                                excelCommet.RichText.Add(comm);
                                excelCommet.AutoFit = true;
                                dataWrited = true;
                            }
                        }

                        // 检测是否需要写入列表提示数据。
                        if (dimension.Rows > 2 &&
                            enmunList != null &&
                            enmunList.TryGetValue(headerCell.Text, out var list))
                        {
                            var listKey = string.Join(",", list);
                            var letter = GetColumeLetter(j);
                            var address = $"{letter}3:{letter}{dimension.Rows}";
                            var exist = workSheet.DataValidations[address];
                            var validList = exist as OfficeOpenXml.DataValidation.Contracts.IExcelDataValidationList;
                            // 由于excel的限制，只能走公式模式
                            if (listKey.Length >= MaxListFormulaChar)
                            {
                                if (!formulaRanges.TryGetValue(listKey, out var formulaRange))
                                {
                                    if (WriteExcelFormulaList(package.Workbook.Worksheets, FormulaSheeet, list, formulaRanges.Count + 1, out formulaRange))
                                        dataWrited = true;

                                    formulaRanges[listKey] = formulaRange;
                                }

                                if (exist != null)
                                {
                                    if (validList != null && validList.Formula.ExcelFormula == formulaRange)
                                        continue;

                                    // 删除额外的来
                                    var foundError = false;
                                    while (exist != null)
                                    {
                                        if (!workSheet.DataValidations.Remove(exist))
                                        {
                                            var msg = "Fail to replace data validation\n" +
                                                $"Excel: {Path.GetFileName(fileName)}\n" +
                                                $"Sheet: {workSheet.Name}\n" +
                                                $"Columne: {headerCell.Text}\n" +
                                                $"Address: {exist.Address}";
                                            //throw new Exception(msg);
                                            warningList.Add(msg);
                                            foundError = true;
                                            break;
                                        }
                                        exist = workSheet.DataValidations[address];
                                    }
                                    if (foundError) continue;
                                }
                                validList = workSheet.DataValidations.AddListValidation(address);
                                validList.Formula.ExcelFormula = formulaRange;
                            }
                            else
                            {
                                if (exist != null)
                                {
                                    // 检测一下是否完全一致，不需要修改
                                    if (validList != null && string.Join(",", validList.Formula.Values) == listKey)
                                        continue;

                                    // 删除额外的来
                                    var foundError = false;
                                    while (exist != null)
                                    {
                                        if (!workSheet.DataValidations.Remove(exist))
                                        {
                                            var msg = "Fail to replace data validation\n" +
                                                $"Excel: {Path.GetFileName(fileName)}\n" +
                                                $"Sheet: {workSheet.Name}\n" +
                                                $"Columne: {headerCell.Text}\n" +
                                                $"Address: {exist.Address}";
                                            //throw new Exception(msg);
                                            warningList.Add(msg);
                                            foundError = true;
                                            break;
                                        }
                                        exist = workSheet.DataValidations[address];
                                    }
                                    if (foundError) continue;
                                }
                                validList = workSheet.DataValidations.AddListValidation(address);
                                foreach (var option in list)
                                    validList.Formula.Values.Add(option);

                                // 通用设置
                                validList.ShowErrorMessage = true;
                                validList.ErrorStyle = OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.information;
                                validList.AllowBlank = true;
                                validList.HideDropDown = false;
                            }
                            dataWrited = true;
                        }
                    }
                }
                if (dataWrited) package.Save();
            }
        }

        /// <summary>
        /// 写入其他页卡的注释罗
        /// </summary>
        /// <param name="excelWorksheets"></param>
        /// <param name="sheet"></param>
        /// <param name="list"></param>
        /// <param name="colume"></param>
        /// <param name="formulaRange"></param>
        /// <returns></returns>
        static bool WriteExcelFormulaList(ExcelWorksheets excelWorksheets, string sheet, string[] list, int colume, out string formulaRange)
        {
            var dataWrited = false;
            var formulaSheet = excelWorksheets[sheet];
            if (formulaSheet == null)
            {
                formulaSheet = excelWorksheets.Add(sheet);
                formulaSheet.Cells["A1"].Value = "Auto generate by TableTools";
                formulaSheet.Hidden = eWorkSheetHidden.VeryHidden;
            }

            var fPrefix = GetColumeLetter(colume);
            for (var i = 0; i < list.Length; i++)
            {
                var fCell = formulaSheet.Cells[$"{fPrefix}{i + 2}"];
                if (fCell.Text != list[i])
                {
                    dataWrited = true;
                    fCell.Value = list[i];
                }
            }

            formulaRange = $"{sheet}!{fPrefix}$2:{fPrefix}${list.Length + 1}";

            return dataWrited;
        }
    }
}
