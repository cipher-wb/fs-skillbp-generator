using DG.DemiEditor;
using GameApp;
using GameApp.Battle;
using GameApp.Native.Battle;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TableDR;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace NodeEditor
{
    public partial class ConfigGraphWindow
    {
        #region 场景单位选择
        public List<int> Entitys { get; set; } = new List<int>();
        public int CurIndex { get; set; } = 0;

        public bool isAllConsole = true;

        public int EntityID = 0;
        //根据下标获取角色，当角色死亡时会有问题
        //public int EntityID
        //{
        //    get
        //    {
        //        if (Entitys.Count == 0 || CurIndex >= Entitys.Count)
        //        {
        //            return 0;
        //        }

        //        return Entitys.ExGet(CurIndex, 0);
        //    }
        //}
        public string[] EntityNames
        {
            get
            {
                Entitys.Clear();
                List<string> names = new List<string>();
                Entitys.Add(0);
                names.Add("无");
                Entitys.Add(0);
                names.Add("主控单位");
                var battle = GameApp.AppFacade.BattleManager?.Battle;
                if (battle != null)
                {
                    var controllers = battle.BattleEntityProcessor.GetControllersDic();
                    var fighters = battle.BattleEntityProcessor.GetFightersDic();
                    foreach (var kv in controllers)
                    {
                        var controllerComp = kv.Value;
                        var battleEntity = controllerComp.Entity as BattleEntity;
                        if (battleEntity != null)
                        {
                            var entityName = controllerComp.GetName();
                            Entitys.Add(battleEntity.Id);
                            names.Add(battleEntity.Id + "|" + entityName);
                        }
                    }
                    foreach (var kv in fighters)
                    {
                        var fighterComp = kv.Value;
                        var battleEntity = fighterComp.BattleEntity;
                        if (battleEntity != null)
                        {
                            var entityName = fighterComp.GetName();
                            Entitys.Add(battleEntity.Id);
                            names.Add(battleEntity.Id + "|" + entityName);
                        }
                    }
                }
                return names.ToArray();
            }
        }
        private long battleGuid = 0;
        public int lastIndex;

        private bool IsInSameBattle()
        {
            if (battleGuid == 0)
            {
                return true;
            }
            var battle = GameApp.AppFacade.BattleManager?.Battle;
            if (battle == null)
            {
                battleGuid = 0;
                return true;
            }
            bool ret = battle.BattleGUID == battleGuid;
            if (!ret)
            {
                battleGuid = battle.BattleGUID;
            }
            return ret;
        }

        public void OnSelectionChange()
        {
            if (lastIndex == CurIndex && IsInSameBattle())
            {
                return;
            }

            lastIndex = CurIndex;

            if (Entitys.Count == 0 || CurIndex >= Entitys.Count)
            {
                return;
            }

            EntityID = Entitys.ExGet(CurIndex, 0);

            isAllConsole = CurIndex == 0 && EntityID == 0 ? true : false;

            var keys = allConsoleConfigIdInfoDic.Keys.ToList<string>();
            foreach (var key in keys)
            {
                var logs = allConsoleConfigIdInfoDic[key];
                var logsList = logs.Where(log => log.Contains($"Entity:{EntityID}")).ToList();
                logs = new Queue<string>(logsList);
                allConsoleConfigIdInfoDic[key] = logs;
            }
            HideEdgeFlowPoint(-1);
            ShowEdgeFlowPoint();
            OnRefreshPinnedView?.Invoke();
            var scene = SceneManager.GetActiveScene();
            if (scene != null)
            {
                var sceneObjects = scene.GetRootGameObjects();
                //WWYTODO临时 选择当前场景，后续添加常量
                var entityRoot = sceneObjects.Where(obj => obj.name == "BattleEntityRoot").First();
                if (entityRoot)
                {
                    var objs = entityRoot.transform.GetComponentsInChildren<Transform>();
                    var selection = objs.Where(obj => obj.name.Contains($"(Clone)({EntityID})")).FirstOrDefault();
                    if (selection != null)
                        Selection.activeGameObject = selection.gameObject;
                }
            }
        }
        #endregion

        #region 日志分析
        private static readonly Regex regex_Log = new Regex(@"RunSkillEffectSingleTarget\s*iOriginSkillConfigID:(\d+)\s*iSkillEffectConfigID:(\d+)\s*iResult:(\d+)\s*.+\s*CreateEntity:(\d+)\s*MainEntity:(\d+)\s*TargetEntity:(\d+)");
        private static readonly Regex regex_iOriginSkillConfigID = new Regex("iOriginSkillConfigID:[0-9]* ");
        private static readonly Regex regex_OriginSkillConfigID = new Regex("OriginSkillConfigID:[0-9]* ");
        private static readonly Regex regex_iResult = new Regex("iResult:[0-9]*");
        private static readonly Regex regex_LogicFrame = new Regex($"LogicFrame:[0-9]* ");
        private static readonly Regex regex_Param = new Regex(@"Param:\s*(?:\[(\d+)\]:(\d+)\s*)+");
        public const int LOGMAXCOUNT = 10;

        private bool needHideFlowPoint = false;
        private float flowPointGap = 60f;
        private float flowPointMoveSpeed = 0.003f;

        private BaseNodeView curDebugNode;
        private Dictionary<string, BaseNodeView> logIdNodeDic;
        private Dictionary<string, BaseNodeView> logIdAINodeDic;

        public bool enableConsole = false;
        public bool isDebug = false;
        // 记录所有日志信息<node.GUID, <Logs>>
        public Dictionary<string, Queue<string>> allConsoleConfigIdInfoDic = new Dictionary<string, Queue<string>>();
        public Dictionary<string, string> allAiNodeStateInfoDic = new Dictionary<string, string>();

        public Action<string, BaseNode> LogEvent { get; set; }
        public Action OnRefreshPinnedView { get; set; }

        public static void ClearAllConsole()
        {
            var winodws = Utils.GetAllWindow<ConfigGraphWindow>();
            foreach (var window in winodws)
            {
                window.EnableConsole(false);
                foreach (var node in window.graph.nodes)
                {
                    if (window.graphView.nodeViewsPerNode.TryGetValue(node, out var childNodeView))
                    {
                        if(node.debug)
                            childNodeView.ToggleDebug();
                    }
                }
            }
        }
        public void EnableConsole(bool enable = true)
        {
            if (!(this.graphView is ConfigGraphView configGraphView))
            {
                return;
            }

            configGraphView.MouseCallBackEvent -= MouseCallBackEvent;
            enableConsole = enable;
            var enableConsoleWindows = Utils.GetEnoughWindow<ConfigGraphWindow>(w => w.enableConsole == true);
            AppFacade.GameOverEditorCallback -= GameOverEndClear;
            AppFacade.GameOverEditorCallback += GameOverEndClear;

            OnRefreshPinnedView?.Invoke();

            try
            {
                var battle = AppFacade.BattleManager?.Battle;

                if (enable || enableConsoleWindows != null)
                {
                    AppFacade.EnableEditorConsole = true;
                    if(battle != null)
                        BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_ENABLE_SKILL_EFFECT_LOG, "0", "1", "0");
                }
                else
                {
                    isPauseGame = false;
                    AppFacade.EnableEditorConsole = false;
                    if (battle != null)
                        BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_ENABLE_SKILL_EFFECT_LOG, "0", "0", "0");
                }

                allConsoleConfigIdInfoDic.Clear();

                if (!enableConsole)
                {
                    AppFacade.OnBattleConsoleListenter(OnBattleConsoleAddLog, false);
                    return;
                }

                logIdNodeDic = new Dictionary<string, BaseNodeView>();
                logIdAINodeDic = new Dictionary<string, BaseNodeView>();
                foreach (var kv in this.graphView.nodeViewsPerNode)
                {
                    var node = kv.Key;
                    var nodeView = kv.Value;
                    if (!(node is IConfigBaseNode iConfigNode)) continue;
                    var id = iConfigNode.GetConfigID();

                    if (iConfigNode.GetConfig() != default)
                    {
                        if (iConfigNode.GetConfig() is AITaskNodeConfig)
                        {
                            int taskNodeType = (int)iConfigNode.GetConfig().ExGetValue("TaskNodeType");
                            string logConfigName = $"TaskNodeType-{taskNodeType}:{id}";
                            if (!logIdAINodeDic.ContainsKey(logConfigName) && node is IParamsNode)
                            {
                                logIdAINodeDic.Add(logConfigName, nodeView);
                            }
                        }
                        else
                        {
                            string logConfigName = $"{GetLogConfigIdNameByType(iConfigNode.GetConfig())}:{id}";
                            if (!logIdNodeDic.ContainsKey(logConfigName) && node is IParamsNode)
                            {
                                logIdNodeDic.Add(logConfigName, nodeView);
                            }
                        }
                    }

                }

                AppFacade.OnBattleConsoleListenter(OnBattleConsoleAddLog);
                configGraphView.MouseCallBackEvent += MouseCallBackEvent;
            }
            catch (Exception e)
            {
                Log.Fatal($"EnableConsole error, {e}");
            }
        }
        void GameOverEndClear()
        {
            ConfigGraphWindow[] configGraphWindows = Utils.GetAllWindow<ConfigGraphWindow>();
            foreach (var configGraphWindow in configGraphWindows)
            {
                configGraphWindow.isDebug = false;
                configGraphWindow.enableConsole = false;
                configGraphWindow.enableDmgLog = false;
                configGraphWindow.isPauseGame = false;
                configGraphWindow.logIdNodeDic?.Clear();
                configGraphWindow.logIdAINodeDic?.Clear();
            }
            OnRefreshPinnedView();
        }
        void MouseCallBackEvent(EventType eventType, IMouseEvent mouseEvent)
        {
            switch (eventType)
            {
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
                    break;
                case EventType.MouseUp:
                    ShowEdgeFlowPoint();
                    break;
            }
        }
        private string GetLogConfigIdNameByType<T>(T node) where T : class
        {
            if (node == null)
            {
                return default;
            }

            string typeName = node.GetType().Name;
            typeName = $"i{typeName}ID";
            if (typeName.Contains("Condition"))
            {
                typeName = typeName.Replace("Skill", string.Empty);
            }
            return typeName;
        }
        public void OnBattleConsoleAddLog(string info, uint level)
        {
            if (!enableConsole || (logIdNodeDic == default && logIdAINodeDic == default))
            {
                return;
            }

            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
            {
                return;
            }

            if(enableDmgLog)
            {
                var currFrame = battle.BattleLogicData.CurClientFrame;
                OnDmgLogAdd(currFrame, info);
            }

            int filterEntity = EntityID;
            if (filterEntity <= 0)
            {
                filterEntity = battle.BattlePlayerManager.GetSelfPlayerMainControlEntityId();
            }

            if (!isAllConsole && filterEntity != 0 &&
                !info.Contains($"Entity:{filterEntity}") &&
                !info.Contains($"Entity={filterEntity}"))
            {
                return;
            }

            Match match = regex_Log.Match(info);
            if (match.Success)
            {
                var createEntityID = int.Parse(match.Groups["4"].Value);
                var mainEntityID = int.Parse(match.Groups["5"].Value);
                var targetEntityID = int.Parse(match.Groups["6"].Value);
                if (createEntityID > 0)
                {
                    var createEntity = battle.BattleEntityProcessor.GetBattleEntity(createEntityID);
                    if (createEntity != null)
                    {
                        var camp = (TEntityCamp)createEntity.BattleAttrBaseComp.GetAttr((int)TBattleNatureEnum.TBN_CAMP);
                        info = info.Replace($"CreateEntity:{createEntityID}", $"CreateEntity:{createEntityID}|{createEntity.GetDebugName()}|{camp.GetDescription(false)}");
                    }
                }
                if (mainEntityID > 0)
                {
                    var mainEntity = battle.BattleEntityProcessor.GetBattleEntity(mainEntityID);
                    if (mainEntity != null)
                    {
                        var camp = (TEntityCamp)mainEntity.BattleAttrBaseComp.GetAttr((int)TBattleNatureEnum.TBN_CAMP);
                        info = info.Replace($"MainEntity:{createEntityID}", $"MainEntity:{mainEntityID}|{mainEntity.GetDebugName()}|{camp.GetDescription(false)}");
                    }
                }
                if (targetEntityID > 0)
                {
                    var targetEntity = battle.BattleEntityProcessor.GetBattleEntity(targetEntityID);
                    if (targetEntity != null)
                    {
                        var camp = (TEntityCamp)targetEntity.BattleAttrBaseComp.GetAttr((int)TBattleNatureEnum.TBN_CAMP);
                        info = info.Replace($"TargetEntity:{targetEntityID}", $"TargetEntity:{targetEntityID}|{targetEntity.GetDebugName()}|{camp.GetDescription(false)}");
                    }
                }
            }

            string logInfo = AnalyzeLogInfo(info, out BaseNodeView nodeView);
            string extarContent = "";
            if (string.IsNullOrEmpty(logInfo)) // 检测下是不是ai的日志
            {
                logInfo = AnalyzeAILogInfo(info, out nodeView);
                extarContent = logInfo;
            }

            if (string.IsNullOrEmpty(logInfo) || nodeView == default)
            {
                return;
            }

            var node = nodeView.nodeTarget;
            //格式化显示当前时间
            string time = DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:ffff");
            string logicFrame = $"{battle.BattleLogicData.CurClientFrame}";
            logInfo = $"Time:{time} \nLogicFrame:{logicFrame} {logInfo}\n";

            //暂时只打印技能编辑器日志
            switch (level)
            {
                case 0:
                    if (!string.IsNullOrEmpty(extarContent))
                    {
                        allAiNodeStateInfoDic[node.GUID] = extarContent;
                    }
                    if (!allConsoleConfigIdInfoDic.TryGetValue(node.GUID, out var debugInfos))
                    {
                        debugInfos = new Queue<string>();
                        allConsoleConfigIdInfoDic.Add(node.GUID, debugInfos);
                    }
                    if (debugInfos.Count >= LOGMAXCOUNT)
                    {
                        debugInfos.Dequeue();
                    }
                    debugInfos.Enqueue(logInfo);
                    LogEvent?.Invoke(logInfo, node);
                    SingleShowEdgeFlowPoint(node, debugInfos.Count, extarContent);
                    // 解析日志信息添加tooltips
                    if (node is IParamsNode)
                    {
                        var paramMatchs = Regex.Matches(info, @"\[(\d+)\]:(-?\d+)(?=\s|$)");
                        {
                            Dictionary<string, int> paramValues = new();
                            foreach (Match paramMatch in paramMatchs)
                            {
                                if (paramMatch.Groups.Count >= 3)
                                {
                                    var indexStr = paramMatch.Groups[1].Value;
                                    var paramValueStr = paramMatch.Groups[2].Value;
                                    if (int.TryParse(paramValueStr, out var paramValue))
                                    {
                                        paramValues.Add(indexStr, paramValue);
                                    }
                                    else
                                    {
                                        // 解析失败也给个默认值
                                        paramValues.Add(indexStr, 0);
                                    }
                                }
                            }
                            foreach (var outPort in nodeView.outputPortViews)
                            {
                                if (paramValues.TryGetValue(outPort.portData.identifier, out var paramvalueOut))
                                {
                                    string customTooltip = paramvalueOut.ToString();
                                    if (outPort.portData.displayName.Contains("单位实例ID"))
                                    {
                                        var battleEntity = battle.BattleEntityProcessor.GetBattleEntity(paramvalueOut);
                                        if (battleEntity != null)
                                        {
                                            customTooltip = battleEntity.GetDebugName();
                                        }
                                    }
                                    outPort.CustomTooltip = customTooltip;
                                    //outPort.MarkDirtyRepaint();
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }

            if (node.debug)
            {
                IsPauseGame = true;

                if (this.graphView.nodeViewsPerNode.TryGetValue(node, out var childNodeView) && !childNodeView.focusable)
                {
                    childNodeView.FocusSelect(true);
                }

                curDebugNode = nodeView;
            }
        }       

        private string AnalyzeAILogInfo(string logInfo, out BaseNodeView node)
        {
            string retStr = "";
            node = default;
            try
            {
                if (logInfo.Contains("[AITaskNode]->OnStateChanged"))
                {
                    string infno = logInfo.Trim();
                    string[] infoArr = infno.Split(',');
                    //[AITaskNode]->OnStateChanged,iNodeType:xx1,iEntity=xx2,iNodeConfigId=xx3,eCurState=xx4
                    //logConfigName = $"TaskNodeType-{typeValue}:{id}";
                    string logConfigName = $"TaskNodeType-{infoArr[1].Split('=')[1]}:{infoArr[3].Split('=')[1]}";
                    logIdAINodeDic.TryGetValue(logConfigName, out node);
                    int nodeState = int.Parse(infoArr[4].Split('=')[1]);
                    TAiNodeState aiNodeState = ((TAiNodeState)nodeState);
                    retStr = aiNodeState.GetDescription(false);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            return retStr;
        }
        private string AnalyzeLogInfo(string logInfo, out BaseNodeView nodeView)
        {
            string str = default;
            nodeView = default;

            try
            {
                string skillInfo = regex_iOriginSkillConfigID.Match(logInfo).Value.TrimEnd();
                if (skillInfo.IsNullOrEmpty())
                {
                    skillInfo = regex_OriginSkillConfigID.Match(logInfo).Value;
                }
                int subIndex = logInfo.IndexOf(skillInfo) + skillInfo.Length;
                str = logInfo.Substring(subIndex, logInfo.Length - subIndex).Trim();
                string[] infos = str.Split(' ');
                if (infos.Length <= 0)
                {
                    return default;
                }

                if (!logIdNodeDic.TryGetValue(infos[0], out nodeView))
                {
                    return default;
                }

                str = str.Replace(infos[0], skillInfo);
                //if (node is RefConfigBaseNode refConfigBaseNode)
                //{
                //    str = str.Replace("  ", "\n");
                //    return str;
                //}

                if (!(nodeView.nodeTarget is IParamsNode paramsNode) || paramsNode.GetParamsAnnotation() == default)
                {
                    return default;
                }

                List<TParamAnnotation> paramsAnn = paramsNode.GetParamsAnnotation().paramsAnn;
                var anno = paramsNode.GetParamsAnnotation();
                for (int i = 0; i < paramsAnn.Count; i++)
                {
                    if (!anno.IsArray || (anno.IsArray && i < anno.ArrayStart))
                    {
                        str = str.Replace($"[{i}]", $"[{paramsAnn[i].Name}]");
                    }
                }
                str = str.Replace("  ", "\n");
                PostprocessLogInfo(ref str, nodeView);
            }
            catch
            {
                return str;
            }

            return str;
        }
        private void PostprocessLogInfo(ref string logInfo, BaseNodeView nodeView)
        {
            if (nodeView == null)
            {
                return;
            }
            switch (nodeView.nodeTarget)
            {
                case TSET_USE_SKILL _:
                case TSET_AI_USE_SKILL _:
                    {
                        // 使用技能节点返回值显示使用结果
                        var matchResult = regex_iResult.Match(logInfo);
                        if (matchResult.Success)
                        {
                            var result = matchResult.Value;
                            var resultValue = int.Parse(result.Replace("iResult:", string.Empty));
                            // 暂不不引入新的Proto库引用，减少依赖
                            var resultDesc = BattleUtilsHelper.GetCodeDesc(resultValue);
                            logInfo = regex_iResult.Replace(logInfo, $"$0-{resultDesc}");
                        }
                        break;
                    }
            }
        }
        #endregion


        #region 节点流向
        private void UpdateEdgeFlowPoint()
        {

            if (graphView != null)
            {
                DropdownMenuAction.Status pinnedStatus = graphView.GetPinnedElementStatus<ConfigPinnedView>();
                if (pinnedStatus != DropdownMenuAction.Status.Hidden &&  enableConsole)
                {
                    //ShowEdgeFlowPoint();
                    needHideFlowPoint = true;
                }
                else if (needHideFlowPoint)
                {
                    HideEdgeFlowPoint();
                    needHideFlowPoint = false;
                    //RefreshWindow();
                }
            }
        }
        private void SetEdgeFlowPoint(EdgeView edgeView, int flowPointCount, float eachChunkContainsPercentage, float edgeLength = 0)
        {
            if (edgeView.EdgeFlowPointVisualElements != null && edgeView.EdgeFlowPointVisualElements.Count > 0 &&
            //如果长度发生变化就需要重新计算
            edgeView.EdgeFlowPointVisualElements.Count == flowPointCount)
            {

                //Debug.Log(edgeView.EdgeFlowPointVisualElements[0].GetType());
                for (int i = 0; i < flowPointCount; i++)
                {
                    edgeView.FlowPointProgress[i] += Time.deltaTime * flowPointMoveSpeed;

                    edgeView.EdgeFlowPointVisualElements[i].transform.position =
                        EdgeFlowPointCaculator.GetFlowPointPosByPercentage(
                            Mathf.Repeat(edgeView.FlowPointProgress[i], 1),
                            edgeView.GetPointsAndTangents, edgeLength) -
                        new Vector2(8 * i, 0);
                }
            }
            else
            {
                if (edgeView.EdgeFlowPointVisualElements != null)
                {
                    foreach (var oldFlowPoint in edgeView.EdgeFlowPointVisualElements)
                    {
                        edgeView.Remove(oldFlowPoint);
                    }
                }

                edgeView.EdgeFlowPointVisualElements = new List<VisualElement>();
                edgeView.FlowPointProgress.Clear();

                for (int i = 0; i < flowPointCount; i++)
                {
                    float initalPercentage = eachChunkContainsPercentage * i;

                    VisualElement visualElement = new VisualElement()
                    {
                        name = "EdgeFlowPoint",
                        transform =
                        {
                            position = EdgeFlowPointCaculator.GetFlowPointPosByPercentage(
                                           initalPercentage, edgeView.GetPointsAndTangents, edgeLength) -
                                       new Vector2(8 * i, 0),
                        }
                    };

                    //可以自定义流点颜色，但注意将其alpha通道设置为1
                    //visualElement.style.unityBackgroundImageTintColor = edgeView.serializedEdge.outputNode.color;
                    edgeView.FlowPointProgress.Add(initalPercentage);
                    edgeView.EdgeFlowPointVisualElements.Add(visualElement);
                    edgeView.Add(visualElement);
                }
            }
        }

        public void SingleShowEdgeFlowPoint(BaseNode node, int count, string extraMsg = "")
        {
            try
            {
                if (!graphView.nodeViewsPerNode.TryGetValue(node, out var nodeView))
                {
                    return;
                }

                //循环次数很少但是是否可以避免
                foreach (var inputPort in nodeView.inputPortViews)
                {
                    inputPort.GetEdges().ForEach(edgeView =>
                    {
                        float edgeLength = 0;
                        for (int i = 0; i < edgeView.GetPointsAndTangents.Length - 1; i++)
                        {
                            edgeLength += Vector2.Distance(edgeView.GetPointsAndTangents[i],
                                edgeView.GetPointsAndTangents[i + 1]);
                        }

                        float eachChunkContainsPercentage = flowPointGap / edgeLength;
                        int flowPointCount = (int)(1 / eachChunkContainsPercentage);

                        if (flowPointCount % 2 == 0)
                        {
                            flowPointCount++;
                        }

                        SetEdgeFlowPoint(edgeView, flowPointCount, eachChunkContainsPercentage, edgeLength);
                        if (!string.IsNullOrEmpty(extraMsg))
                        {
                            nodeView.ShowLogExtraMsg(extraMsg);
                        }
                        else
                        {
                            nodeView.ShowLogCounter(count);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
        /// <summary>
        /// 展示FlowPoint，自己计算位置信息
        ///
        /// ---------------------------------------------------------------------------------------
        /// 
        /// 也可以继承Manipulator实现一个控制器，对EdgeView添加AddManipulator，Manipulator即可自动同步位置
        /// 只需要设置Manipulator的left和top间距即可，可参见https://github.com/HalfLobsterMan/3.0_GraphProcessor/blob/56c2928a1790994df4a1f8a9c2a30477bbe6e21d/Editor/Views/BaseEdgeView.cs
        /// </summary>
        public void ShowEdgeFlowPoint()
        {
            foreach (var edgeView in graphView.edgeViews)
            {
                float edgeLength = 0;
                for (int i = 0; i < edgeView.GetPointsAndTangents.Length - 1; i++)
                {
                    edgeLength += Vector2.Distance(edgeView.GetPointsAndTangents[i],
                        edgeView.GetPointsAndTangents[i + 1]);
                }

                float eachChunkContainsPercentage = flowPointGap / edgeLength;
                int flowPointCount = (int)(1 / eachChunkContainsPercentage);

                if (flowPointCount % 2 == 0)
                {
                    flowPointCount++;
                }
                bool isRoot = true;
                if (edgeView.serializedEdge.outputPort != null)
                {
                    if (edgeView.serializedEdge.outputNode != null)
                    {
                        NodeInputPortContainer inputs = edgeView.serializedEdge.outputNode.inputPorts;
                        foreach (var item in inputs)
                        {
                            if (item.GetEdges() != null && item.GetEdges().Count > 0)
                            {
                                isRoot = false;
                                break;
                            }
                        }
                    }
                }

                BaseNode node = null;
                if (isRoot)
                {
                    node = edgeView.serializedEdge.outputNode;
                }
                else
                {
                    node = edgeView.serializedEdge.inputNode;
                }

                if (node == null)
                {
                    continue;
                }

                if (!allConsoleConfigIdInfoDic.TryGetValue(node.GUID, out var logs))
                {
                    continue;
                }

                if (this.graphView.nodeViewsPerNode.TryGetValue(node, out var nodeView) && nodeView.visible)
                {
                    if (node is AITaskNodeConfigNode && allAiNodeStateInfoDic.TryGetValue(node.GUID, out var aiState))
                    {
                        nodeView.ShowLogExtraMsg(aiState);
                    }
                    else
                    {
                        nodeView.ShowLogCounter(logs.Count);
                    }
                }
                else
                {
                    nodeView.ShowLogCounter(0);
                }

                if (logs == null || logs.Count == 0 || !nodeView.visible)
                {
                    continue;
                }

                SetEdgeFlowPoint(edgeView, flowPointCount, eachChunkContainsPercentage, edgeLength);
            }
        }
        public void HideEdgeFlowPoint(int clearFlowTime = 0)
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
            {
                isDebug = false;
                enableConsole = false;
                enableDmgLog = false;
                AppFacade.EnableEditorConsole = false;
            }

            double s = clearFlowTime / 30;
            if(s > 0)
            {
                var keys = allConsoleConfigIdInfoDic.Keys.ToList<string>();
                for (int i = keys.Count - 1; i >= 0; i--)
                {
                    string key = keys[i];
                    var logs = allConsoleConfigIdInfoDic[key];

                    for (var j = logs.Count - 1; j >= 0; j--)
                    {
                        string log = logs.Peek();
                        string time = regex_LogicFrame.Match(log).Value;
                        time = time.Replace("LogicFrame:", string.Empty).TrimEnd();
                        if (string.IsNullOrEmpty(time))
                        {
                            //说明有问题
                            Log.Error($"LogicFrame Match Fail : {log}");
                            continue;
                        }

                        try
                        {
                            int LastLogicFrame = int.Parse(time);
                            LastLogicFrame += clearFlowTime;
                            if (battle.BattleLogicData.CurClientFrame > LastLogicFrame)
                            {
                                logs.Dequeue();
                            }
                            else
                            {
                                // 如果不满足直接结束
                                break;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                }
            }
            else if (clearFlowTime == 0)
            {
                allConsoleConfigIdInfoDic.Clear();
            }
            allAiNodeStateInfoDic.Clear();
            foreach (var node in graphView.nodeViews)
            {
                node.ShowLogCounter();
            }

            foreach (var edgeView in graphView.edgeViews)
            {
                if (edgeView.EdgeFlowPointVisualElements == null) continue;
                foreach (var edgeFlowPoint in edgeView.EdgeFlowPointVisualElements)
                {
                    edgeView.Remove(edgeFlowPoint);
                }

                edgeView.EdgeFlowPointVisualElements.Clear();
            }
        }
        #endregion
    }

    /// <summary>
    /// 伤害公式输出
    /// </summary>
    public partial class ConfigGraphWindow
    {
        #region 伤害输出数据结构
        /// <summary>
        /// 伤害输出节点类
        /// </summary>
        public class DmgLogNode
        {
            public int EffectID;
            public int Level;
            public string InfoDesc;
            public string ReportFomat;
            public List<int> Results;

            public List<DmgLogNode> ChildNodes;

            public DmgLogNode(int effectID, int level, string desc, string report = null)
            {
                EffectID = effectID;
                Level = level;
                InfoDesc = desc;
                ReportFomat = report;
                Results = new List<int>();
                ChildNodes = null;
            }

            public DmgLogNode AddChildNode(int effectID, string desc, string report = null)
            {
                if (ChildNodes == null)
                    ChildNodes = new List<DmgLogNode>();
                DmgLogNode childNode = new DmgLogNode(effectID, Level + 1, desc, report);
                ChildNodes.Add(childNode);
                return childNode;
            }

            public void ResetResult()
            {
                if (Results != null)
                    Results.Clear();
                if (ChildNodes != null && ChildNodes.Count > 0)
                {
                    for (int i = 0; i < ChildNodes.Count; ++i)
                    {
                        var childNode = ChildNodes[i];
                        childNode.ResetResult();
                    }
                }
            }

            public bool CheckEffect(int effectID)
            {
                if (effectID == EffectID)
                    return true;
                else
                {
                    if (ChildNodes != null && ChildNodes.Count > 0)
                    {
                        for (int i = 0; i < ChildNodes.Count; ++i)
                        {
                            var childNode = ChildNodes[i];
                            var check = childNode.CheckEffect(effectID);
                            if (check)
                                return true;
                        }
                    }
                    return false;
                }
            }

            public void AddResult(int effectID, int resultValue)
            {
                if (effectID == EffectID)
                    Results.Add(resultValue);

                if (ChildNodes != null && ChildNodes.Count > 0)
                {
                    for (int i = 0; i < ChildNodes.Count; ++i)
                    {
                        var childNode = ChildNodes[i];
                        childNode.AddResult(effectID, resultValue);
                    }
                }
            }

            public List<string> GetExport(int level = 1, bool showID = true)
            {
                if (Level >= level)
                    return null;
                if (string.IsNullOrEmpty(ReportFomat))
                    return null;
                if (Results.Count <= 0)
                    return null;
                List<string> exportList = new List<string>();
                var expStr = string.Format(ReportFomat, GetDescGroup(showID));
                string lvtitle = GetLevelTitle(Level);
                expStr = string.Format("{0}{1}", lvtitle, expStr);
                exportList.Add(expStr);
                if (ChildNodes != null && ChildNodes.Count > 0)
                {
                    for (int i = 0; i < ChildNodes.Count; ++i)
                    {
                        var childNode = ChildNodes[i];
                        var childExportList = childNode.GetExport(level, showID);
                        if (childExportList != null && childExportList.Count > 0)
                            exportList.AddRange(childExportList);
                    }
                }
                PopupResult();
                return exportList;
            }

            private string GetLevelTitle(int level)
            {
                switch (level)
                {
                    case 0:
                        return "\n";
                    case 1:
                        return "\n  ";
                    case 2:
                        return "\n    ";
                    case 3:
                        return "\n      ";
                    case 4:
                        return "\n        ";
                    default:
                        return "";
                }
            }

            private void PopupResult()
            {
                if (Results.Count > 0)
                    Results.RemoveAt(0);
                if (ChildNodes != null && ChildNodes.Count > 0)
                {
                    for (int i = 0; i < ChildNodes.Count; ++i)
                    {
                        var childNode = ChildNodes[i];
                        childNode.PopupResult();
                    }
                }
            }

            private string[] GetDescGroup(bool showID)
            {
                List<string> descList = new List<string>();
                descList.Add(GetDesc(showID));
                if (ChildNodes != null && ChildNodes.Count > 0)
                {
                    for (int i = 0; i < ChildNodes.Count; ++i)
                    {
                        var childNode = ChildNodes[i];
                        descList.Add(childNode.GetDesc(showID));
                    }
                }
                return descList.ToArray();
            }

            private string GetDesc(bool showID)
            {
                var result = GetResult();
                if (showID)
                    return string.Format("{0}[{1}_{2}]", InfoDesc, EffectID, result);
                else
                    return string.Format("{0}[{1}]", InfoDesc, result);
            }

            public string GetResult()
            {
                return Results.Count > 0 ? Results[0].ToString() : "??";
            }
        }

        /// <summary>
        /// 伤害输出数据类
        /// </summary>
        public class DmgLogData
        {
            public int CurrentFrame;
            //public DmgLogNode DmgStateCheckNode;
            public DmgLogNode FinalAttackNode;
            public DmgLogNode FinalPureNode;

            public DmgLogData()
            {
                InitDmgList();
            }

            private void InitDmgList()
            {
                //if(DmgStateCheckNode == null)
                //{
                //    // 伤害前的状态检查计算 
                //    DmgStateCheckNode = new DmgLogNode(146002322, 0, "伤害前的状态检查计算", "{0}: {1}");
                //    DmgStateCheckNode.AddChildNode(186010405, "")
                //}

                if (FinalAttackNode == null)
                {
                    //1 最终伤害值
                    FinalAttackNode = new DmgLogNode(146005249, 0, "最终伤害值", "<color=yellow>1 {0} </color>");
                    {
                        //1.1 乘区1·攻击力	= a.基础攻击*(1+a.基础攻击加成)+a.附加攻击
                        var dmgNode1 = FinalAttackNode.AddChildNode(146004489, "乘区1·攻击力", "<color=#abcdef>1.1</color> {0}");
                        //1.2 乘区2·防御率	
                        var dmgNode2 = FinalAttackNode.AddChildNode(146004997, "乘区2·防御率", "<color=#abcdef>1.2</color> {0} , {1} , {2} , {3} , {4} , {5} , {6} , {7}");
                        {
                            //1.2.2 F防御率计算(\"计算防御值)
                            dmgNode2.AddChildNode(146004586, "F防御率计算(\"计算防御值)");
                            //1.2.3 破防率
                            dmgNode2.AddChildNode(146004998, "b.破防率");
                            //1.2.3 破防率-临时
                            dmgNode2.AddChildNode(146004999, "b.破防率-临时");
                            //1.2.4 a.通用伤害附加值
                            dmgNode2.AddChildNode(146005239, "a.通用伤害附加值");
                            //1.2.5 b.通用伤害减免值
                            dmgNode2.AddChildNode(146005240, "b.通用伤害减免值");
                            //1.2.6 a.条件伤害附加值
                            dmgNode2.AddChildNode(146005241, "a.条件伤害附加值");
                            //1.2.7 b.条件伤害减免值
                            dmgNode2.AddChildNode(146005242, "b.条件伤害减免值");
                        }
                        //1.3 乘区3·技能系数
                        var dmgNode3 = FinalAttackNode.AddChildNode(146004495, "乘区3·技能系数", "<color=#abcdef>1.3</color> {0} , {1}");
                        {
                            //技能固定值
                            dmgNode3.AddChildNode(66001191, "技能固定值");
                        }
                        //1.4 乘区5·化解乘区
                        var dmgNode4 = FinalAttackNode.AddChildNode(146004496, "乘区5·化解乘区", "<color=#abcdef>1.5</color> {0}");
                        //1.5 乘区4·会心乘区
                        var dmgNode5 = FinalAttackNode.AddChildNode(146004497, "乘区4·会心乘区", "<color=#abcdef>1.4</color> {0}");
                        //1.6 乘区5·伤害加深乘区
                        var dmgNode6 = FinalAttackNode.AddChildNode(146004499, "乘区5·伤害加深乘区", "<color=#abcdef>1.5</color> {0}");
                        //1.7 乘区6·伤害弱化乘区
                        var dmgNode7 = FinalAttackNode.AddChildNode(146004500, "乘区6·伤害弱化乘区", "<color=#abcdef>1.6</color> {0}");
                        //1.8 乘区7·威压乘区
                        var dmgNode8 = FinalAttackNode.AddChildNode(146004501, "乘区7·威压乘区", "<color=#abcdef>1.7</color> {0}");
                        //1.9 乘区8·主类型伤害加深乘区
                        var dmgNode9 = FinalAttackNode.AddChildNode(146004502, "乘区8·主类型伤害加深乘区", "<color=#abcdef>1.8</color> {0}");
                        //1.10 乘区9·子类型伤害加深乘区
                        var dmgNode10 = FinalAttackNode.AddChildNode(146004503, "乘区9·子类型伤害加深乘区", "<color=#abcdef>1.9</color> {0}");
                        //1.11 乘区10·基础类型伤害加深乘区
                        var dmgNode11 = FinalAttackNode.AddChildNode(146004506, "乘区10·基础类型伤害加深乘区", "<color=#abcdef>1.10</color> {0}");
                        //1.12 乘区11·技能标签伤害加深乘区
                        var dmgNode12 = FinalAttackNode.AddChildNode(146004507, "乘区11·技能标签伤害加深乘区", "<color=#abcdef>1.11</color> {0}");
                        //1.13 乘区12·五行增伤乘区
                        var dmgNode13 = FinalAttackNode.AddChildNode(146004508, "乘区12·五行增伤乘区", "<color=#abcdef>1.12</color> {0}");
                        //1.14 乘区13·五行减伤乘区
                        var dmgNode14 = FinalAttackNode.AddChildNode(146004509, "乘区13·五行减伤乘区", "<color=#abcdef>1.13</color> {0}");
                        //1.15 乘区14·异常状态增伤乘区
                        var dmgNode15 = FinalAttackNode.AddChildNode(146004510, "乘区14·异常状态增伤乘区", "<color=#abcdef>1.14</color> {0}");
                        //1.16 乘区15·技能效果·被动增伤乘区
                        var dmgNode16 = FinalAttackNode.AddChildNode(146004511, "乘区15·技能效果·被动增伤乘区", "<color=#abcdef>1.15</color> {0}");
                        //1.17 乘区16·技能效果·被动减伤乘区
                        var dmgNode17 = FinalAttackNode.AddChildNode(146004512, "乘区16·技能效果·被动减伤乘区", "<color=#abcdef>1.16</color> {0}");
                        //1.18 乘区17·技能效果·主动技能增伤乘区
                        var dmgNode18 = FinalAttackNode.AddChildNode(146004513, "乘区17·技能效果·主动技能增伤乘区", "<color=#abcdef>1.17</color> {0}");
                        //1.19 乘区18·战斗用-主动减伤乘区
                        var dmgNode19 = FinalAttackNode.AddChildNode(146004514, "乘区18·战斗用-主动减伤乘区", "<color=#abcdef>1.18</color> {0}");
                        //1.20 乘区19·局内条件增伤乘区
                        var dmgNode20 = FinalAttackNode.AddChildNode(146004515, "乘区19·局内条件增伤乘区", "<color=#abcdef>1.19</color> {0}");
                        //1.21 乘区20·战斗道具增伤乘区
                        var dmgNode21 = FinalAttackNode.AddChildNode(146004516, "乘区20·战斗道具增伤乘区", "<color=#abcdef>1.20</color> {0}");
                        //1.22 乘区21·单位类型增伤乘区
                        var dmgNode22 = FinalAttackNode.AddChildNode(146004517, "乘区21·单位类型增伤乘区", "<color=#abcdef>1.21</color> {0}");
                        //1.23 乘区22·玩法类型增伤增伤乘区
                        var dmgNode23 = FinalAttackNode.AddChildNode(146004518, "乘区22·玩法类型增伤增伤乘区", "<color=#abcdef>1.22</color> {0}");
                        //1.23 乘区22·最终伤害增加乘区
                        var dmgNode24 = FinalAttackNode.AddChildNode(146004519, "乘区22·最终伤害增加乘区", "<color=#abcdef>1.22</color> {0}");
                        //1.23 乘区23·最终伤害减免乘区
                        var dmgNode25 = FinalAttackNode.AddChildNode(146004520, "乘区23·最终伤害减免乘区", "<color=#abcdef>1.233</color> {0}");
                    }
                }
                if (FinalPureNode == null)
                {
                    //3 最终纯粹伤害值 = 技能额外伤害
                    FinalPureNode = new DmgLogNode(66001191, 0, "最终纯粹伤害值", "<color=yellow>3 {0} </color>");
                }
            }

            public void ResetDmgList()
            {
                FinalAttackNode.ResetResult();
            }
            public void AddDmgLog(int currFrame, string info, int logLevel, bool showID)
            {
                if (CurrentFrame != currFrame)
                {
                    ResetDmgList();
                }
                CurrentFrame = currFrame;

                Match match = regex_Log.Match(info);
                if (!match.Success)
                    return;
                var skillID = int.Parse(match.Groups["1"].Value);
                var effectID = int.Parse(match.Groups["2"].Value);
                var resultValue = int.Parse(match.Groups["3"].Value);
                var createEntityID = int.Parse(match.Groups["4"].Value);
                var mainEntityID = int.Parse(match.Groups["5"].Value);
                var targetID = int.Parse(match.Groups["6"].Value);

                var skillConfig = SkillConfigManager.Instance.GetItem(skillID);
                if (skillConfig == null)
                    return;
                var dmgType = skillConfig.DamageType;
                if(dmgType == TSkillDamageType.TSDT_NULL)
                {
                    if (skillConfig.SkillSubType == TBattleSkillSubType.TBSST_ZHAO_SHI)
                        dmgType = TSkillDamageType.TSDT_ZHI_JIE;// : TSkillDamageType.TSDT_FASHU;
                    else if (skillConfig.SkillSubType == TBattleSkillSubType.TBSST_QI_SHU || skillConfig.SkillSubType == TBattleSkillSubType.TBSST_SHEN_TONG)
                        dmgType = TSkillDamageType.TSDT_ZHI_JIE;   // 没有法术伤害了
                    else
                        dmgType = TSkillDamageType.TSDT_REAL;
                }

                if (dmgType == TSkillDamageType.TSDT_ZHI_JIE && FinalAttackNode.CheckEffect(effectID))
                {
                    FinalAttackNode.AddResult(effectID, resultValue);
                    if (effectID == FinalAttackNode.EffectID)
                    {
                        DmgLogExport(FinalAttackNode, skillID, mainEntityID, targetID, logLevel, showID);
                    }
                }
                if(dmgType == TSkillDamageType.TSDT_REAL && FinalPureNode.CheckEffect(effectID))
                {
                    FinalPureNode.AddResult(effectID, resultValue);
                    if (effectID == FinalPureNode.EffectID)
                    {
                        DmgLogExport(FinalPureNode, skillID, mainEntityID, targetID, logLevel, showID);
                    }
                }
            }

            private void DmgLogExport(DmgLogNode finalNode, int skillId, int entityID, int targetID, int logLevel, bool showID)
            {
                var skillConfig = SkillConfigManager.Instance.GetItem(skillId);
                var descConfig = SkillDescConfigManager.Instance.GetItem(skillId, 1);
                var skillName = descConfig != null && !string.IsNullOrEmpty(descConfig.SkillName) ? descConfig.SkillName : skillConfig.SkillName;
                var skillInfo = string.Format("Frame:{0} (EntityID:{1})使用技能:{2}_{3},目标(ID:{4}) —— 伤害计算公式输出：{5}", CurrentFrame, entityID, skillName, skillId, targetID, finalNode.GetResult());
                var dmgLogList = finalNode.GetExport(logLevel, showID);
                for (int i = 0; i < dmgLogList.Count; ++i)
                {
                    var dmgLog = dmgLogList[i];
                    skillInfo = skillInfo + dmgLog;
                }
                Log.Debug(skillInfo);
            }
        }
        #endregion

        public bool enableDmgLog = false;           //伤害输出开关
        public int dmgLogLevel = 2;                 //输出公式等级
        public bool dmgLogIDVisible = false;        //输出是否带技能效果ID
        private DmgLogData dmgLogData;              //伤害输出数据结构

        /// <summary>
        /// 新Log数据塞入
        /// </summary>
        /// <param name="currFrame"></param>
        /// <param name="info"></param>
        private void OnDmgLogAdd(int currFrame, string info)
        {
            if (dmgLogData == null)
                dmgLogData = new DmgLogData();
            dmgLogData.AddDmgLog(currFrame, info, dmgLogLevel, dmgLogIDVisible);
        }
    }
}
