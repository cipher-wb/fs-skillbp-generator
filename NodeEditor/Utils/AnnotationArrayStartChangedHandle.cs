#if UNITY_EDITOR
using GameApp.SDKMgr;
using LitJson;
using Newtonsoft.Json;
using NodeEditor;
using NodeEditor.SkillEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TableDR;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static NodeEditor.Extensions;

namespace NodeEditor
{
    public class AnnotationArrayStartChangedHandle
    {
        /// <summary>
        /// 不定长数组参数，如果有修改ArrayStart(从第几个参数开始时不定长的参数)时，需要重置节点的线条
        /// </summary>
        /// <param name="nodeName">paramsAnn的节点名称-NodeName</param>
        /// <param name="configName">paramsAnn的配置表名称ConfigName</param>
        /// <param name="arrayStartOld">修改前ArrayStart的值</param>
        /// <param name="arrayStartNew">修改后ArrayStart的值</param>
        /// <param name="paramsAnnOld">修改前参数注解列表</param>
        /// <param name="paramsAnnNew">修改后参数注解列表</param>
        public static void CheckAndHandlerGraphEdges(ParamsAnnotation anno, string nodeName, string configName, int arrayStartOld, int arrayStartNew, List<TParamAnnotation> paramsAnnOld, List<TParamAnnotation> paramsAnnNew)
        {
            if (arrayStartNew == arrayStartOld)
            {
                return;
            }

            string[] pathSavesJsons = {
                "Assets/Thirds/NodeEditor/AIEditor/Saves/Jsons",
                "Assets/Thirds/NodeEditor/SkillEditor/Saves/Jsons",
                "Assets/Thirds/NodeEditor/GamePlayEditor/Saves/Jsons",
                "Assets/Thirds/NodeEditor/NpcEventEditor/Saves/Jsons"
            };

            // 查找相关graph，如果打开的话先让关闭处理
            ConfigGraphWindow[] configWin = Utils.GetAllWindow<ConfigGraphWindow>();
            if (configWin != null && configWin.Length > 0)
            {
                for (int i = 0; i < configWin.Length; i++)
                {
                    string info = File.ReadAllText(configWin[i].GetGraph().path);
                    if (info.Contains(nodeName) && info.Contains(configName))
                    {
                        configWin[i].Close();
                    }
                }
            }

            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("已修复连线的节点:");

            for (int i = 0; i < pathSavesJsons.Length; i++)
            {
                var graghFiles = Directory.GetFiles(pathSavesJsons[i], "*.json", SearchOption.AllDirectories);
                foreach (var file in graghFiles)
                {
                    if (string.IsNullOrEmpty(file))
                    {
                        continue;
                    }
                    var fileInfo = new FileInfo(file);
                    if (fileInfo == null || !fileInfo.Exists)
                    {
                        continue;
                    }
                    string info = File.ReadAllText(fileInfo.FullName);
                    if (info.Contains(nodeName) && info.Contains(configName))
                    {
                        bool ret = HandleGraghFile(fileInfo.FullName, info, nodeName, configName, arrayStartOld, arrayStartNew, paramsAnnOld, paramsAnnNew);
                        sbLog.AppendLine(fileInfo.FullName);
                    }
                }
            }
            Log.Info(sbLog.ToString());
            if (EditorUtility.DisplayDialog("提示", sbLog.ToString(), "复制日志"))
            {
                GUIUtility.systemCopyBuffer = sbLog.ToString();
            }
        }

        private static bool HandleGraghFile(string filePath, string graphContent, string nodeName, string configName, int arrayStartOld, int arrayStartNew, List<TParamAnnotation> paramsAnnOld, List<TParamAnnotation> paramsAnnNew)
        {
            JsonData jsonData = JsonMapper.ToObject(graphContent);
            Dictionary<string, JsonData> referencesDataDic = new Dictionary<string, JsonData>();
            Dictionary<string, object> configData = new Dictionary<string, object>();
            JsonData references = jsonData["references"];
            var type = typeof(SkillConfig);
            Type configType = TableHelper.GetTableType($"{type.Namespace}.{configName}");
            // 收集所有的节点数据
            foreach (KeyValuePair<string, JsonData> item in references)
            {
                JsonData jsItem = item.Value;
                if (jsItem != null && jsItem.IsObject && jsItem.ContainsKey("data"))
                {
                    var data = jsItem["data"];
                    if (data.ContainsKey("GUID"))
                    {
                        string guid = data["GUID"].ToString();
                        referencesDataDic.Add(guid, jsItem);
                    }
                }
            }
            JsonData edgesArray = jsonData["edges"];
            foreach (var kvp in referencesDataDic)
            {
                string guid = kvp.Key;
                JsonData jsData = kvp.Value;
                if (jsData["type"]["class"].ToString() == nodeName)
                {
                    string configJson = jsData["data"]["ConfigJson"].ToString();
                    object config = JsonConvert.DeserializeObject(configJson, configType);
                    var paramList = config.ExGetValue("Params") as List<TParam>;
                    List<JsonData> edgeList = GetEdgeWithOutPutNode(edgesArray, guid);
                    var removedEdges = SynchParamAndEdge(edgeList, paramList, arrayStartOld, arrayStartNew, paramsAnnOld, paramsAnnNew);
                    for (int i = 0; i < removedEdges.Count; i++)
                    {
                        edgesArray.Remove(removedEdges[i]);
                    }
                    configJson = JsonConvert.SerializeObject(config);
                    jsData["data"]["ConfigJson"] = configJson;
                }
            }
            File.WriteAllText(filePath, jsonData.ToJson());
            return true;
        }

        private static List<JsonData> GetEdgeWithOutPutNode(JsonData edgesArray, string nodeGUID)
        {
            List<JsonData> retList = new List<JsonData>();
            foreach (var edge in edgesArray)
            {
                //string outputPortIdentifier = (edge as JsonData)["outputPortIdentifier"].ToString();
                string outputNodeGUID = (edge as JsonData)["outputNodeGUID"].ToString();
                if (outputNodeGUID == nodeGUID)
                {
                    retList.Add((edge as JsonData));
                }
            }
            return retList;
        }

        /// <summary>
        /// 同步一个node数据的参数和连线，返回需要删除的连线
        /// </summary>
        /// <param name="edgeList"></param>
        /// <param name="paramList"></param>
        /// <param name="arrayStartOld"></param>
        /// <param name="arrayStartNew"></param>
        /// <param name="paramsAnnOld"></param>
        /// <param name="paramsAnnNew"></param>
        /// <returns></returns>
        private static List<JsonData> SynchParamAndEdge(List<JsonData> edgeList, List<TParam> paramList, int arrayStartOld, int arrayStartNew, List<TParamAnnotation> paramsAnnOld, List<TParamAnnotation> paramsAnnNew)
        {
            List<JsonData> removeEdges = new List<JsonData>(edgeList);
            List<TParam> paramTmpList = new List<TParam>(paramList);
            paramList.Clear();
            for (int i = 0; i < arrayStartNew; i++)
            {
                var oldAnn = paramsAnnOld.FirstOrDefault((ann) =>
                {
                    if (ann.Name == paramsAnnNew[i].Name &&
                        ann.RefTypeName == paramsAnnNew[i].RefTypeName &&
                        ann.RefPortTypeNames == paramsAnnNew[i].RefPortTypeNames)
                    {
                        return true;
                    }
                    return false;
                });

                if (oldAnn != null)
                {
                    int index = paramsAnnOld.IndexOf(oldAnn);
                    if (index < arrayStartOld)
                    {
                        paramList.Add(paramTmpList[index]);
                        bool changed = false;
                        edgeList.ForEach(jsEdge =>
                        {
                            if (jsEdge["outputPortIdentifier"].ToString() == index.ToString())
                            {
                                jsEdge["outputPortIdentifier"] = i.ToString();
                                removeEdges.Remove(jsEdge);
                                changed = true;
                            }
                        });
                        if (changed)
                        {
                            edgeList.Clear();
                            edgeList.AddRange(removeEdges);
                        }
                    }
                    else
                    {
                        TParam tmp = new TParam();
                        tmp.SetValue(0);
                        paramList.Add(tmp);
                    }
                }
                else
                {
                    TParam tmp = new TParam();
                    tmp.SetValue(0);
                    paramList.Add(tmp);
                }
            }

            for (int i = arrayStartOld; i < paramTmpList.Count; i++)
            {
                paramList.Add(paramTmpList[i]);
            }

            edgeList.ForEach(jsEdge =>
            {
                if (jsEdge["outputPortIdentifier"].ToString() == arrayStartOld.ToString())
                {
                    jsEdge["outputPortIdentifier"] = arrayStartNew.ToString();
                    removeEdges.Remove(jsEdge);
                }
            });

            return removeEdges;
        }
    }
}

#endif