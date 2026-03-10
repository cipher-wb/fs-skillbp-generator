using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using TableDR;
#if !NodeExport
using UnityEngine;
#endif

namespace NodeEditor
{
    public class ExternalExportUtil
    {
        /// <summary>
        /// svn日志类型
        /// </summary>
        public enum SvnOpt
        {
            None = -1,

            ADD = 0,

            UPDATE = 1,

            DELETE = 2,

            Max
        }

        /// <summary>
        /// jenkins导出时记录json文件的版本号,key:版本号文本,value：json文件的路径
        /// </summary>
        static Dictionary<string, List<string>> versionForDic = null;
        static string unknowVersion_ = "unknowVersion_";
        /// <summary>
        /// 记录ai,gamalplay,skill,mapai,mapevent等svn版本号,只记录最大的
        /// </summary>
        static Dictionary<string, (string jsonPath,long svnVersion)> fileName2SvnVersion = null;

        static string BuildPath=> Application.dataPath + "/../../Build";
        /// <summary>
        /// 用于检测是否有相同id的配置，<Config2ID,文件路径>
        /// </summary>
        static Dictionary<string, string> repeatIdCheckDic = null;
        /// <summary>
        /// 有重复d的配置的错误信息
        /// </summary>
        static List<string> repeatIdErrors = null;
        /// <summary>
        /// 错误类型，工具通知使用
        /// </summary>
        static string errorType = string.Empty;
        /// <summary>
        /// 错误的额外信息，会推送到钉钉群
        /// </summary>
        static string errorExtrMsg = "没有";

        /// <summary>
        /// jenkins导出时记录json文件的版本号,key:版本号文本,value：json文件的路径
        /// </summary>
        public static bool ExportJsonToExcel()
        {
            var tmp = DesignTable.EditorConfigManagers;
            if (tmp.Count != DesignTable.EditorConfigManagerTypes.Count)
            {
                errorType = $"加载表格Manager出错";
                SaveErrorTypeToFile();
                return false;
            }

            bool state = false;
            List<string> jsonPaths = new List<string>();
            try
            {
                Log.Info("ExportByJenkins Start");
                repeatIdCheckDic = new Dictionary<string, string>();
                repeatIdErrors = new List<string>();
                //从svn日志文件中收集需要导出的json文件
                HashSet<string> allSkillJsonPath = LoadAllSkillJsonFile();
                List<string> svnDelFiles = new List<string>();
                List<string> svnAddFiles = new List<string>();
                CollectLogFile($"{BuildPath}/NodeEditor", jsonPaths, svnAddFiles, svnDelFiles,out var hasSkillEffectJson);
                //bool syncAllJsonFile = false;
                //if (svnAddFiles.Count > 0) //有新增文件
                //{
                //    syncAllJsonFile = true;
                //    foreach (var addPath in svnAddFiles)
                //    {
                //        allSkillJsonPath.Add(addPath);
                //    }
                //}

                //if (svnDelFiles.Count > 0) //有删除文件
                //{
                //    syncAllJsonFile = true;
                //    foreach (var rmPath in svnDelFiles)
                //    {
                //        allSkillJsonPath.Remove(rmPath);
                //    }
                //}

                //if (syncAllJsonFile)
                //{
                //    // 将所有json路径写入到文件中
                //    SaveAllSkillJsonFile(allSkillJsonPath);
                //}

                Log.Info("Start Invoke ExportGraph2Excel");
                //执行导出到excel
                if (jsonPaths.Count > 0)
                {
                    // 如果jsonPaths中没有任何技能相关的文件那么直接使用jsonPaths，否则使用allSkillJsonPath，并将jsonPaths中非技能文件放入到allSkillJsonPath中
                    if (!hasSkillEffectJson)
                    {
                        state = ExportNodeJson2Excel(jsonPaths);
                    }
                    else
                    {
                        foreach (var item in jsonPaths)
                        {
                            allSkillJsonPath.Add(item);
                        }
                        // 战斗相关所有导出的，那么开启下导出前清理老数据开关
                        state = ExportNodeJson2Excel(allSkillJsonPath.ToList(), null, true);
                    }
                }
                else
                {
                    state = true;
                }
                Log.Info("End Invoke ExportGraph2Excel");
                //删除日志文件，不在这里删除了放到shell脚本中删除
                //DeltLogFile($"{BuildPath}/NodeEditor");
                //写入shell将要获取的提交文件
                if (state)
                {
                    WriteMessageToFile();
                }
                else
                {
                    SaveErrorTypeToFile();
                }
                return state;
            }
            catch (Exception e)
            {
                Log.Exception(e);
                errorExtrMsg = e?.ToString();
                errorType = $"代码异常,详情看日志";
            }
            SaveErrorTypeToFile();
            return false;
        }

        private static void SaveErrorTypeToFile()
        {
            if (string.IsNullOrEmpty(errorType))
            {
                errorType = "未知错误";
            }
            File.WriteAllText($"{BuildPath}/env.properties", $"errorType={errorType}\nerrorExtrMsg={errorExtrMsg}", new UTF8Encoding(false));
        }

        public static void WriteMessageToFile()
        {
            try
            {
                string users = "";
                // 根据版本号记录上传人员信息
                if(fileName2SvnVersion != null)
                {
                    users = GetSvnCmitUserInfo();
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("json->excel自动导表上传");
                if(!string.IsNullOrEmpty(users))
                {
                    sb.AppendLine($"svn cmit users:{users}");
                }
                if (versionForDic != null)
                {
                    foreach (var kvp in versionForDic)
                    {
                        sb.AppendLine(kvp.Key);
                        foreach (string path in kvp.Value)
                        {
                            if (path.EndsWith(".meta"))
                            {
                                continue;
                            }

                            if(path.Contains("Saves/Jsons"))
                            {
                                string filePath = path.Substring(path.IndexOf("Saves/Jsons") + 12);
                                sb.AppendLine(filePath);
                            }

                            if (path.Contains("Saves\\Jsons"))
                            {
                                string filePath = path.Substring(path.IndexOf("Saves\\Jsons") + 12);
                                sb.AppendLine(filePath);
                            }
                        }
                    }
                }
                File.WriteAllText($"{BuildPath}/node_export_cmt.log", sb.ToString(),new UTF8Encoding(false));
                File.WriteAllText($"{BuildPath}/NodeEditor/success.log", "1");
            }
            catch (Exception e1)
            {
                Log.Error(e1.ToString());
            }
        }

        private static string GetSvnCmitUserInfo()
        {
            if (fileName2SvnVersion == null)
            {
                return string.Empty;
            }
            string users = "";
            foreach (var item in fileName2SvnVersion)
            {
                // 先记录在tem_xx文件下，在shell脚本中如果最新excel上传成功后，在将内容替换到item.Key的文件中
                File.WriteAllText($"{BuildPath}/tmp_{item.Key}", item.Value.svnVersion.ToString());

                string oldVersion = string.Empty;
                string recordFile = $"{BuildPath}/{item.Key}";
                if (File.Exists(recordFile))
                {
                    oldVersion = File.ReadAllText(recordFile);
                }
                if (string.IsNullOrEmpty(oldVersion))
                {
                    continue;
                }
                // 执行bat脚本拉取svn上传者信息
                // svn.exe log -r {last_ver}:{cur_ver} path
                string pathPrefix = item.Value.jsonPath;
                string last_ver = oldVersion;
                long cur_ver = item.Value.svnVersion;
                var resultStr = ExcuteCmd($"{Application.dataPath}/../../../Tools/svn/bin/svn.exe", $" log -r {last_ver}:{cur_ver} {pathPrefix}", pathPrefix);
                if (!string.IsNullOrEmpty(resultStr))
                {
                    byte[] buf = Encoding.GetEncoding("GB2312").GetBytes(resultStr);
                    buf = Encoding.Convert(Encoding.GetEncoding("GB2312"), new UTF8Encoding(false), buf);
                    resultStr = new UTF8Encoding(false).GetString(buf);
                }

                if (string.IsNullOrEmpty(resultStr))
                {
                    continue;
                }
                // 解析内容
                //------------------------------------------------------------------------
                //r511668 | noodles | 2025 - 11 - 05 14:04:31 + 0800(周三, 05 11月 2025) | 2 lines
                //【问题描述】：
                //【TAPD单号】:xxxx
                bool isDashedLine = false;
                var arr = resultStr.Split('\n');
                for (int i = 0; i < arr.Length; i++)
                {
                    var line = arr[i];
                    if (line.Contains("-------------------------------------------"))
                    {
                        isDashedLine = true;
                    }
                    else
                    {
                        if (!isDashedLine)
                        {
                            continue;
                        }
                        isDashedLine = false;
                        var arr1 = line.Split('|');
                        if (arr1.Length < 2)
                        {
                            continue;
                        }
                        if (users.Contains(arr1[1]))
                        {
                            continue;
                        }
                        users = $"{users},{arr1[1]}";
                    }
                }
            }
            return users;
        }

        public static string ExcuteCmd(string cmd, string args, string workPath)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("GB2312");
            p.StartInfo.FileName = cmd;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.StartInfo.WorkingDirectory = workPath;
            p.Start();//启动程序
            string outResult = string.Empty;
            while (!p.StandardOutput.EndOfStream)
            {
                outResult = p.StandardOutput.ReadToEnd();
            }
            p.Close();
            return outResult;
        }

        private static HashSet<string> LoadAllSkillJsonFile()
        {
            HashSet<string> hashSet = new HashSet<string>();
            string allJsonFilePath = $"{BuildPath}/AllSkillJosn.txt";
            //if (File.Exists(allJsonFilePath))
            //{
            //    string[] allFilePaths = File.ReadAllLines(allJsonFilePath);
            //    foreach (var jsonFilePath in allFilePaths)
            //    {
            //        hashSet.Add(jsonFilePath);
            //    }
            //    return hashSet;
            //}

            StreamWriter streamWriter = new StreamWriter(allJsonFilePath);
            CollectJsonFilesByGraphType("AIGraph", hashSet, streamWriter);
            CollectJsonFilesByGraphType("SkillGraph", hashSet, streamWriter);
            CollectJsonFilesByGraphType("GamePlayGraph", hashSet, streamWriter);
            streamWriter.Flush();
            streamWriter.Close();
            return hashSet;
        }

        private static void SaveAllSkillJsonFile(HashSet<string> hashSet)
        {
            string allJsonFilePath = $"{BuildPath}/AllSkillJosn.txt";
            StreamWriter streamWriter = new StreamWriter(allJsonFilePath);
            foreach (var item in hashSet)
            {
                streamWriter.WriteLine(item);
            }
            streamWriter.Flush();
            streamWriter.Close();
        }

        public static string NormalizeDirectorySeparator(string directoryPath)
        {
            // 使用正则表达式替换所有的目录分隔符为正斜杠
            return directoryPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static void CollectJsonFilesByGraphType(string graphType, HashSet<string> hashSet, StreamWriter streamWriter)
        {
            string pathAIGraph = GetGraphSavePath(graphType);
            string[] jsonFiles = Directory.GetFiles(pathAIGraph, "*.json", SearchOption.AllDirectories);
            for (int i = 0; i < jsonFiles.Length; i++)
            {
                hashSet.Add(NormalizeDirectorySeparator(jsonFiles[i]));
                streamWriter.WriteLine(jsonFiles[i]);
            }
        }

        /// <summary>
        /// 收集svn日志下的json文件
        /// </summary>
        public static void CollectLogFile(string dir, List<string> jsonPathList,List<string> addFiles,List<string> delFiles,out bool hasSkillEffectJson)
        {
            hasSkillEffectJson = false;
            versionForDic = new Dictionary<string, List<string>>();
            fileName2SvnVersion = new Dictionary<string, (string jsonPath, long svnVersion)>();
            Log.Info($"CollectLogFile from Dir:{dir}");
            DirectoryInfo dirs = new DirectoryInfo(dir);
            FileInfo[] files = dirs.GetFiles();
            long projectLogVersion = 0;
            Dictionary<string,string> projectLogIncludeVersionFile = new Dictionary<string,string>();
            for (int i = 0; i < files.Length; i++)
            {
                string[] lines = File.ReadAllLines(files[i].FullName, Encoding.GetEncoding("GB2312"));
                Log.Info($"Get SvnLogFile Content:{files[i].FullName}");
                string pathPrefix = "";
                // 暂时只记录技能编辑器相关的文件
                bool recordAddAndDel = false;
                //svn项目全盘更新的日志
                bool isProjectLog = false;
                // 记录svn版本号,只记录最大的
                string svnVersionFileName = "";
                if (files[i].Name.StartsWith("ai_svn"))
                {
                    pathPrefix = $"{Application.dataPath}/Thirds/NodeEditor/AIEditor/Saves/";
                    recordAddAndDel = true;
                    hasSkillEffectJson = true;
                    svnVersionFileName = "ai_last_version.txt";
                }
                else if (files[i].Name.StartsWith("gameplay_svn"))
                {
                    pathPrefix = $"{Application.dataPath}/Thirds/NodeEditor/GamePlayEditor/Saves/";
                    recordAddAndDel = true;
                    hasSkillEffectJson = true;
                    svnVersionFileName = "gameplay_last_version.txt";
                }
                else if (files[i].Name.StartsWith("npc_svn"))
                {
                    pathPrefix = $"{Application.dataPath}/Thirds/NodeEditor/NpcEventEditor/Saves/";
                    svnVersionFileName = "npc_version.txt";
                }
                else if (files[i].Name.StartsWith("skill_svn"))
                {
                    pathPrefix = $"{Application.dataPath}/Thirds/NodeEditor/SkillEditor/Saves/";
                    recordAddAndDel = true;
                    hasSkillEffectJson = true;
                    svnVersionFileName = "skill_last_version.txt";
                }
                else if (files[i].Name.StartsWith("mapanim_svn"))
                {
                    pathPrefix = $"{Application.dataPath}/Thirds/NodeEditor/MapAnimEditor/Saves/";
                    svnVersionFileName = "mapai_last_version.txt";
                }
                else if (files[i].Name == "svn.log") // 全盘更的日志
                {
                    pathPrefix = "";
                    isProjectLog = true;
                }
                List<string> curVersionFile = null;
                string curVersionStr = "";
                bool addToDic = false;
                long unknowVersion = 0;
                bool isFindProjectLogJson = false;
                for (int j = 0; j < lines.Length; j++)
                {
                    byte[] buf = Encoding.GetEncoding("GB2312").GetBytes(lines[j]);
                    buf = Encoding.Convert(Encoding.GetEncoding("GB2312"), new UTF8Encoding(false), buf);
                    string contentLine = new UTF8Encoding(false).GetString(buf);
                    if (string.IsNullOrEmpty(contentLine))
                    {
                        continue;
                    }

                    if (!isProjectLog && (contentLine.Contains("At revision") || contentLine.Contains("to revision")))
                    {
                        if (!versionForDic.TryGetValue(contentLine, out var list))
                        {
                            list = new List<string>();
                            if (string.IsNullOrEmpty(curVersionStr) && curVersionFile != null && curVersionFile.Count > 0)
                            {
                                list.AddRange(curVersionFile);
                            }
                            curVersionFile = list;
                            addToDic = true;
                            versionForDic.Add(contentLine, curVersionFile);
                        }
                        else
                        {
                            if (curVersionFile != null && curVersionFile.Count > 0)
                            {
                                list.AddRange(curVersionFile);
                            }
                            curVersionFile = list;
                            addToDic = true;
                        }

                        var arr = contentLine.Split(' ');
                        if(arr != null && arr.Length > 0)
                        {
                            string versionNum = arr[arr.Length - 1];
                            versionNum = versionNum.Replace(".", "");
                            long.TryParse(versionNum, out var verionValue);
                            fileName2SvnVersion.TryGetValue(svnVersionFileName, out var versionInfo);
                            if (verionValue > versionInfo.svnVersion)
                            {
                                fileName2SvnVersion[svnVersionFileName] = (pathPrefix,verionValue);
                            }
                        }
                        curVersionStr = contentLine;
                        continue;
                    }

                    if (contentLine.EndsWith(".json"))
                    {
                        Log.Info($"SvnLogFile Line{j} :{contentLine}");
                        string path = "";
                        SvnOpt svnOpt = SvnOpt.None;
                        if (isProjectLog)
                        {
                            if (contentLine.Contains("Saves\\Jsons"))
                            {
                                isFindProjectLogJson = true;
                                if (contentLine.Contains("AIEditor"))
                                {
                                    projectLogIncludeVersionFile["ai_last_version.txt"] = $"{Application.dataPath}/Thirds/NodeEditor/AIEditor/Saves/"; ;
                                }
                                else if (contentLine.Contains("GamePlayEditor"))
                                {
                                    projectLogIncludeVersionFile["gameplay_last_version.txt"] = $"{Application.dataPath}/Thirds/NodeEditor/GamePlayEditor/Saves/";
                                }
                                else if (contentLine.Contains("NpcEventEditor"))
                                {
                                    projectLogIncludeVersionFile["npc_version.txt"] = $"{Application.dataPath}/Thirds/NodeEditor/NpcEventEditor/Saves/";
                                }
                                else if (contentLine.Contains("SkillEditor"))
                                {
                                    projectLogIncludeVersionFile["skill_last_version.txt"] = $"{Application.dataPath}/Thirds/NodeEditor/SkillEditor/Saves/";
                                }
                                else if (contentLine.Contains("MapAnimEditor"))
                                {
                                    projectLogIncludeVersionFile["mapai_last_version.txt"] = $"{Application.dataPath}/Thirds/NodeEditor/MapAnimEditor/Saves/";
                                }

                                if (contentLine.StartsWith("A"))
                                {
                                    svnOpt = SvnOpt.ADD;
                                }

                                if (contentLine.StartsWith("U"))
                                {
                                    svnOpt = SvnOpt.UPDATE;
                                }

                                if (contentLine.StartsWith("D"))
                                {
                                    svnOpt = SvnOpt.DELETE;
                                    if (!recordAddAndDel && ( contentLine.Contains("SkillEditor") || contentLine.Contains("AIEditor")))
                                    {
                                        delFiles.Add(path);
                                    }
                                }

                                int assetsIndex = contentLine.IndexOf("Assets");
                                path = contentLine.Substring(assetsIndex + 6);
                                path = $"{Application.dataPath}/{path}";
                                path = NormalizeDirectorySeparator(path);
                                if (path.Contains("//"))
                                {
                                    path = path.Replace("//","/");
                                }
                                if (recordAddAndDel && svnOpt == SvnOpt.DELETE)
                                {
                                    // 记录删除的文件
                                    delFiles.Add(path);
                                    continue;
                                }

                                if (recordAddAndDel && svnOpt == SvnOpt.ADD)
                                {
                                    // 记录新增的文件
                                    addFiles.Add(path);
                                }

                                if (svnOpt != SvnOpt.ADD && svnOpt != SvnOpt.UPDATE)
                                {
                                    continue;
                                }
                            }
                            else if (isFindProjectLogJson && unknowVersion == 0 && contentLine.Contains("to revision"))
                            {
                                var arr = contentLine.Split(' ');
                                if (arr != null && arr.Length > 0)
                                {
                                    string versionNum = arr[arr.Length - 1];
                                    versionNum = versionNum.Replace(".", "");
                                    long.TryParse(versionNum, out var verionValue);
                                    unknowVersion = verionValue;
                                    projectLogVersion = unknowVersion > projectLogVersion ? unknowVersion : projectLogVersion;
                                }
                                
                            }
                        }
                        else
                        {
                            int index = contentLine.IndexOf("Jsons");
                            if (index < 0)
                            {
                                continue;
                            }
                            path = pathPrefix + contentLine.Substring(index);
                            path = NormalizeDirectorySeparator(path);
                            if (recordAddAndDel && contentLine.StartsWith("A"))
                            {
                                // 记录新增的文件
                                addFiles.Add(path);
                            }
                            else if (recordAddAndDel && contentLine.StartsWith("D"))
                            {
                                // 记录删除的文件
                                delFiles.Add(path);
                            }
                        }

                        if (File.Exists(path) && !jsonPathList.Contains(path))
                        {
                            Log.Info($"collect json:{path}");
                            jsonPathList.Add(path);
                            if (curVersionFile == null)
                            {
                                addToDic = false;
                                curVersionFile = new List<string>();
                            }
                            curVersionFile.Add(path);
                        }
                    }
                }
                if (!addToDic && curVersionFile != null && curVersionFile.Count > 0)
                {
                    if (versionForDic.TryGetValue("unknowVersion_", out var unkowList))
                    {
                        unkowList.AddRange(curVersionFile);
                    }
                    else
                    {
                        versionForDic.Add("unknowVersion_", new List<string>(curVersionFile));
                    }
                }
                // 全局日志下撸出的svn版本号
                if(projectLogVersion > 0 && projectLogIncludeVersionFile.Count > 0)
                {
                    foreach (var kvp in projectLogIncludeVersionFile)
                    {
                        fileName2SvnVersion.TryGetValue(kvp.Key, out var versionInfo);
                        if (projectLogVersion > versionInfo.svnVersion)
                        {
                            fileName2SvnVersion[kvp.Key] = (kvp.Value, projectLogVersion);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 删除日志文件
        /// </summary>
        /// <param name="dir"></param>
        public static void DeltLogFile(string dir)
        {
            DirectoryInfo dirs = new DirectoryInfo(dir);
            FileInfo[] files = dirs.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i].FullName);
            }
        }

        public static string GetGraphTypeByFileName(string fileName)
        {
            if (fileName.Contains("SkillGraph"))
            {
                return "SkillGraph";
            }
            else if (fileName.Contains("AIGraph"))
            {
                return "AIGraph";
            }
            else if (fileName.Contains("GamePlayGraph"))
            {
                return "GamePlayGraph";
            }
            else if (fileName.Contains("NpcEventGraph"))
            {
                return "NpcEventGraph";
            }
            else if (fileName.Contains("MapAnimGraph"))
            {
                return "MapAnimGraph";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 将node编辑器的json文件导出到excel
        /// </summary>
        /// <param name="jsonPaths"></param>
        public static bool ExportNodeJson2Excel(List<string> jsonPaths, Func<object, bool> configCheck = null, bool clearOldExcelData = false)
        {
            bool result = false;
            try
            {
                if (jsonPaths == null || jsonPaths.Count == 0)
                {
                    Log.Error("ExportGraph2Excel failed, paths is null");
                    return result;
                }

                Dictionary<string, List<string>> cacheExport = new Dictionary<string, List<string>>();

                // 对路径进行分类，分批处理
                var length = jsonPaths.Count;
                var excelNames = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    var path = jsonPaths[i];
                    var fileName = Path.GetFileName(path);
                    string graphType = GetGraphTypeByFileName(fileName);
                    if (string.IsNullOrEmpty(graphType))
                    {
                        Log.Error($"{path} is not a graph file!");
                        //Log.Error($"ExportGraph2Excel failed, can not find matched graph type by name, path: {path}");
                        continue;
                    }

                    if (!cacheExport.TryGetValue(graphType, out var graphList))
                    {
                        cacheExport[graphType] = new List<string>();
                    }
                    cacheExport[graphType].Add(path);
                }
                var isCancel = false;
                Dictionary<string, dynamic> graphSetDic = new Dictionary<string, dynamic>();
                bool allSuccess = true;
                HashSet<string> checkRepeatId = new HashSet<string>();
                foreach (var kv in cacheExport)
                {
                    var graphType = kv.Key;
                    if (!graphSetDic.TryGetValue(graphType, out var graphSet))
                    {
                        graphSet = GetGraphSetting(graphType);
                        graphSetDic.Add(graphType, graphSet);
                    }
                    var excelPath = $"{Constants.ExcelPathPrefix}/{graphSet.ExcelName}";
                    var excelName = graphSet.ExcelName;

                    // 战斗相关编辑器，导出前做下清理
                    if (clearOldExcelData)
                    {
                        var graphTypeName = graphType.ToString();
                        if (graphTypeName == "AIGraph" ||
                            graphTypeName == "SkillGraph")
                        {
                            Utils.WatchTime($"清空Excel数据-{graphTypeName}: {excelName}", () =>
                            {
                                ExcelManager.Inst.CleanSheets(excelPath, (sheetName) =>
                                {
                                    foreach (var configName in DesignTable.EditorConfigNames)
                                    {
                                        if (sheetName.Contains(configName))
                                        {
                                            return true;
                                        }
                                    }
                                    return false;
                                });
                            });
                        }
                    }

                    var pathsExport = kv.Value;
                    var configs = new List<object>();
                    for (int i = 0, lengthExport = pathsExport.Count; i < lengthExport; i++)
                    {
                        var pathExport = pathsExport[i];
                        ProcessGraphJson(graphType, pathExport, (graphName, configList) =>
                        {
#if !NodeExport
                            Log.Info($"导出到Excel...{graphName}:{i}/{lengthExport}");
#endif
                            if (configCheck == null)
                            {
                                configs.AddRange(configList);
                            }
                            else
                            {
                                foreach (var config in configList)
                                {
                                    if (configCheck.Invoke(config))
                                    {
                                        configs.Add(config);
                                    }
                                }
                            }
                        });
                        if (isCancel) goto cancel;
                    }

                    // 如果有重复id的话直接退出
                    if (repeatIdErrors != null && repeatIdErrors.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var errmsg in repeatIdErrors)
                        {
                            Log.Fatal(errmsg);
                            sb.AppendLine(errmsg);
                        }
                        errorExtrMsg = sb.ToString();
                        errorType = "有相同ID的配置,非常严重需要立即处理";
                        return false;
                    }

                    if (configs.Count > 0)
                    {
                        excelNames += $"{excelName}：更新数量[{configs.Count}]\n";
                        if (!File.Exists(excelPath))
                        {
                            Log.Error($"file not exist {excelPath}");
                            errorType = $"文件不存在:{excelPath}";
                            return result;
                        }

                        // Excel数据写入，数据排序，清理空行
                        ExcelManager.Inst.AddExcelMemberRefillableAction("NpcTemplateRuleConfig", (propName, configObj) =>
                        {
                            if (propName == "RoleCommonProperty")
                            {
                                return false;
                            }
                            return true;
                        });

                        // 导出到Excel
                        bool ret = ExcelManager.Inst.WriteExcel(excelPath, configs);
                        ExcelManager.Inst.RemoveExcelMemberRefillableAction("NpcTemplateRuleConfig", default);

                        if (!ret)
                        {
                            errorType = $"写入到excel出错,详情看日志";
                            allSuccess = false;
                        }
                    }
                }
                Log.Info($"已完成导出:\n{excelNames}\n导出数据完成");
                result = allSuccess;
                return result;
            cancel:
                return false;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                errorExtrMsg = ex?.ToString();
                errorType = $"代码异常,详情看日志";
            }
            return result;
        }

        /// <summary>
        /// 解析node的json文件
        /// </summary>
        /// <param name="graphType"></param>
        /// <param name="path"></param>
        /// <param name="action"></param>
        public static void ProcessGraphJson(string graphType, string path, Action<string, List<object>> action)
        {
#if !NodeExport
            Log.Info($"ProcessGraphJson: {path} ");
#endif
            //var graph = LoadGraph(graphType, path);
            string fileText = File.ReadAllText(path);
            dynamic jsonObj = null;
            try
            {
                jsonObj = JsonConvert.DeserializeObject<dynamic>(fileText);
            }
            catch (Exception e)
            {
                Log.Exception(e);
            }
            if (jsonObj == null || jsonObj.references == null)
            {
                Log.Error($"ProcessGraph Error at pat: {path} "); 
                return;
            }
            List<object> configList = new List<object>();
            var skillConifgtype = typeof(SkillConfig);
            // 兼容unity2021版本的格式
            if (jsonObj.references.ContainsKey("RefIds"))
            {
                if (jsonObj.references["version"] != 2)
                {
                    Log.Error($"数据格式和json数据的版本不一致，检测下是否有错!");
                }
                dynamic refIds = jsonObj.references.RefIds;
                foreach (var item in refIds)
                {
                    object config = ParsingConfigJsonFromJsonData(item["data"], skillConifgtype, path);
                    if (config != null)
                    {
                        configList.Add(config);
                    }
                }
            }
            else
            {
                // 兼容unity2020版本的格式
                foreach (var item in jsonObj.references)
                {
                    if (item.Name == "version")
                    {
                        continue;
                    }
                    object config = ParsingConfigJsonFromJsonData(item.Value["data"], skillConifgtype, path);
                    if (config != null)
                    {
                        configList.Add(config);
                    }
                }
            }
            var fileName = Path.GetFileName(path);
            action?.Invoke(fileName, configList);
        }

        private static object ParsingConfigJsonFromJsonData(dynamic jsonData,Type skillConifgtype,string jsonPath)
        {
            var data = jsonData;
            if (data != null && data["ConfigJson"] != null)
            {
                string configJson = data["ConfigJson"].ToString();
                string config2ID = data["Config2ID"].ToString();
                // 有相同id的confi,收集起来导出的时候会报错误异常，默认情况下不做检测repeatIdCheckDic是null的，目前只有工具在执行json导出到excel时才实例化了该变量
                if (repeatIdCheckDic != null)
                {
                    if (repeatIdCheckDic.TryGetValue(config2ID, out var otherJsonPath))
                    {
                        if (repeatIdErrors != null)
                        {
                            repeatIdErrors.Add($"ConfigJson has same ID={config2ID},between {jsonPath}  and  {otherJsonPath}");
                        }
                    }
                    else
                    {
                        repeatIdCheckDic.Add(config2ID, jsonPath);
                    }
                }
                string configTypeName = config2ID.Split('_')[0];
                Type configType = TableHelper.GetTableType($"{skillConifgtype.Namespace}.{configTypeName}");
                object config = null;
                try
                {
                    config = JsonConvert.DeserializeObject(configJson, configType);
                }
                catch (Exception ex)
                {
                    Log.Error($"解析数据失败, ConfigName:{configType.Name}, Path:{jsonPath}\n{ex}");
                    return null;
                }
                if (config is EditorBaseConfig editorBaseConfig)
                {
                    editorBaseConfig.EditorDesc = data["Desc"].ToString();
                    editorBaseConfig.EditorGraphName = Path.GetFileName(jsonPath);
                }
                return config;
            }
            return null;
        }

        /// <summary>
        /// 根据graphType获取存储json文件的路径
        /// </summary>
        /// <param name="graphType"></param>
        /// <returns></returns>
        public static string GetGraphSavePath(string graphType)
        {
            string savePath = string.Empty;
            switch (graphType)
            {
                case "AIGraph":
                    {
                        savePath = Application.dataPath + "/Thirds/NodeEditor/AIEditor/Saves";
                    }
                    break;
                case "SkillGraph":
                    {
                        savePath = Application.dataPath + "/Thirds/NodeEditor/SkillEditor/Saves";
                    }
                    break;
                case "GamePlayGraph":
                    {
                        savePath = Application.dataPath + "/Thirds/NodeEditor/GamePlayEditor/Saves";
                    }
                    break;
                case "NpcEventGraph":
                    {
                        savePath = Application.dataPath + "/Thirds/NodeEditor/NpcEventEditor/Saves";
                    }
                    break;
                case "MapAnimGraph":
                    {
                        savePath = Application.dataPath + "/Thirds/NodeEditor/MapAnimEditor/Saves";
                    }
                    break;
                default:
                    {
                        Log.Info($"GetGraphSavePath error:{graphType}");
                    }
                    break;
            }
            return savePath;
        }

        /// <summary>
        /// 根据graphType获取EditorSetting.json的内容
        /// </summary>
        /// <param name="graphType"></param>
        /// <returns></returns>
        public static dynamic GetGraphSetting(string graphType)
        {
            string settingPath = GetGraphSavePath(graphType) + "/EditorSetting.json";
            string fileText = File.ReadAllText(settingPath);
            var jsonObj = JsonConvert.DeserializeObject<dynamic>(fileText);
            //System.Console.WriteLine(jsonObj.Path);
            return jsonObj;
        }
    }
}
