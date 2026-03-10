using GameApp.Event;
using GameApp.Native.Battle;
using GraphProcessor;
using System;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEditor;

namespace NodeEditor
{
    /// <summary>
    /// 事件监听处理
    /// </summary>
    public partial class NodeEditorManager
    {
        private static bool isOpenDialogCheck = false;
        private static IEnumerator enumeratorRepaint;

        [EditorMethodRegister(typeof(InitializeOnLoadMethodAttribute), "NodeGraph-技能编辑器相关")]
        private static void InitOnLoad()
        {
            //Log.Debug("NodeEditorManager.Event InitOnLoad");

            ExcelManager.OnExcelChanged += OnExcelChanged;
            ConfigGraphWindow.OnOpenWindow += OnOpenWindow;
            ConfigGraphWindow.OnCloseWindow += OnCloseWindow;
            NodeEditorWindow.OnOpenWindow += OnOpenWindow;
            NodeEditorWindow.OnCloseWindow += OnCloseWindow;
            TableAnnotation.OnChangedJson += OnChangedTableAnnotation;
            NodeEditorHelper.OnTableValueChange += OnTableValueChange;
            GameApp.NodeEditorManager.OnNodeEditorEvent += OnNodeEditorEvent;
            GameApp.NodeEditorManager.OnNodeEditorEvent_DebugBreak += OnNodeDebugBreak;
            GameApp.NodeEditorManager.OnNodeEditorEvent__DebugInit += OnNodeDebugInit;
        }

       

        /// <summary>
        /// 响应NodeEditorEvent
        /// </summary>
        /// <param name="type"></param>
        private static void OnNodeEditorEvent(NodeEditorEventType type)
        {
            switch (type)
            {
                case NodeEditorEventType.KeyCodeDown_J:
                    {
                        var win = Utils.GetEnoughWindow<ConfigGraphWindow>((w) =>
                        {
                            return w.hasFocus;
                        });
                        switch (win)
                        {
                            case SkillEditor.SkillGraphWindow winSkill:
                                winSkill.TestSKill(false);
                                break;
                            case AIEditor.AIGraphWindow winAI:
                                winAI.TestAI(false);
                                break;
                        }
                    }
                    break;
                case NodeEditorEventType.KeyCodeDown_G:

                    break;
                case NodeEditorEventType.KeyCodeDown_F5:
                    {
                        var win = Utils.GetEnoughWindow<ConfigGraphWindow>((w) =>
                        {
                            return w.hasFocus;
                        });
                        win?.NodeDebugContinue();
                    }
                    break;
                case NodeEditorEventType.KeyCodeDown_F9:
                    {
                        var win = Utils.GetEnoughWindow<ConfigGraphWindow>((w) =>
                        {
                            return w.hasFocus;
                        });
                        win?.NodeDebugStateSwitch();
                    }
                    break;
                case NodeEditorEventType.KeyCodeDown_F10:
                    {
                        var win = Utils.GetEnoughWindow<ConfigGraphWindow>((w) =>
                        {
                            return w.hasFocus;
                        });
                        win?.NodeDebugRunNext();
                    }
                    break;
            }
        }

        private static void OnNodeDebugInit()
        {
            BattleWrapper.UnityEditorDebug_ReInitConfig();

            var configGraphWindows = Utils.GetAllWindow<ConfigGraphWindow>();
            var length = configGraphWindows.Length;
            
            for (int i = 0; i < length; i++)
            {
                var window = configGraphWindows[i];
                var graph = window.GetGraph();
                if (graph == null)
                    continue;
                foreach (var node in graph.nodes)
                {
                    if (!(node is IConfigBaseNode iConfigNode))
                        continue;
                    var id = iConfigNode.GetConfigID();

                    var configName = iConfigNode.GetConfigName();
                    var configType = Utils.GetConfigNodeType(configName);
                    if (node.debug && configType != 0)
                    {
                        BattleWrapper.UnityEditorDebug_AddDebugConfigId(configType, id);
                        var bpDic = node.GetDebugBreakpointDic();
                        foreach (var bpData in bpDic)
                        {
                            if (bpData.Value == null)
                                continue;
                            BattleWrapper.UnityEditorDebug_RefreshDebugCondition(configType, id, bpData.Value.ParamIndex, bpData.Value.Enable, bpData.Value.OpType, bpData.Value.CValue);
                        }
                    }
                }
            }
        }

        private static void OnNodeDebugBreak(int nodeType, int nodeID)
        {
            var typeName = Utils.GetConfigNodeTypeName(nodeType);
            var debugNode = ConfigDataUtils.GetSingleOpenEditorConfigNode(typeName, nodeID);    //开启的技能配置中找节点
            if(debugNode == null)
            {
                // 未打开的技能配置，打开
                debugNode = ConfigDataUtils.GetAndOpenEditorConfigNode(typeName, nodeID);
            }
            if (debugNode == null)
            {
                BattleWrapper.UnityEditorDebug_RunUntilNextConfig();
                return;
            }
            var debugWnd = ConfigDataUtils.GetSingleOpenConfigGraphWindow(debugNode);
            if(debugWnd == null)
            {
                BattleWrapper.UnityEditorDebug_RunUntilNextConfig();
                return;
            }
            if (!debugWnd.hasFocus)
                debugWnd.Focus();
            debugWnd.SetNodeDebugBreak(debugNode);
            
        }

        /// <summary>
        /// 响应全局按键（屏蔽，与C++调试冲突）
        /// </summary>
        //private void OnGlobalKeyCallback(GlobalInput.GlobalInputKeyCode code, bool isDown)
        //{
        //    if (Application.isPlaying && Application.isFocused && GlobalInput.GetKeyDown(GlobalInput.GlobalInputKeyCode.J))
        //    {
        //        var win = Utils.GetWindow<SkillGraphWindow>();
        //        if (win != null)
        //        {
        //            win.TestSKill(false);
        //        }
        //    }
        //}


        /// <summary>
        /// 响应表格变动
        /// </summary>
        /// <param name="changedFiles">改表文件列表</param>
        private static void OnExcelChanged(List<string> changedFiles)
        {
            try
            {
                //var nodeEditorWindow = Utils.GetAllWindow<NodeEditorWindow>();
                // 刷新所有打开技能编辑器
                var configGraphWindows = Utils.GetAllWindow<ConfigGraphWindow>();
                var length = configGraphWindows.Length;
                // 表格变动，弹窗提示需要重新初始化
                if (length > 0 && !isOpenDialogCheck)
                {
                    isOpenDialogCheck = true;
                    //WWYTODO根据策划要求，暂时屏蔽刷新，之后和日志进行整合
                    //if (EditorUtility.DisplayDialog(name, "表格变动，需要重新刷新数据", "好的"))
                    //{

                    //}
                    isOpenDialogCheck = false;
                    for (int i = 0; i < length; i++)
                    {
                        var window = configGraphWindows[i];
                        EditorUtility.DisplayProgressBar(name, $"刷新技能编辑器[{i}/{length}]:{window.titleContent.text}", (float)i / length);
                        window.RefreshWindow();
                        window.ShowNotification($"刷新技能编辑器[{i}/{length}]:{window.titleContent.text}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// 响应窗口打开
        /// </summary>
        /// <param name="win">窗口实例</param>
        private static void OnOpenWindow(INodeEditorWindow win)
        {
            // 加载表格
            DesignTable.Load();
            // 提前初始化ConfigIDManager信息
            if (win is ConfigGraphWindow)
            {
                _ = ConfigIDManager.Inst;
            }
        }

        /// <summary>
        /// 响应窗口关闭
        /// </summary>
        /// <param name="win">窗口实例</param>
        private static void OnCloseWindow(INodeEditorWindow win)
        {
            if (win is ConfigGraphWindow configGraphWindow)
            {
                configGraphWindow.SaveFlags = EditorFlag.GraphSaveFlags;
            }

            bool hasWin = Utils.HasOpenWindow<EditorWindow>((w) =>
            {
                return w is INodeEditorWindow;
            });
            if (!hasWin)
            {
                // TODO
            }
        }

        /// <summary>
        /// 响应表格声明文件变动
        /// </summary>
        private static void OnChangedTableAnnotation()
        {
            // 监听声明文件变化，刷新所有技能编辑器窗口
            // 使修改内容直接生效
            var wins = Utils.GetAllWindow<ConfigGraphWindow>();
            foreach (var win in wins)
            {
                win.RefreshWindow();
            }
        }

        /// <summary>
        /// 响应表数据修改
        /// </summary>
        /// <param name="paramName">修改参数名</param>
        private static void OnTableValueChange(string paramName)
        {
            switch (paramName)
            {
                case nameof(TParamType):
                case nameof(TIndicatorType):
                    if (enumeratorRepaint == null)
                    {
                        try
                        {
                            if (Selection.activeObject is NodeInspectorObject nodeInspector)
                            {
                                enumeratorRepaint = EditorCoroutineRunner.StartEditorCoroutine(IEForceRepaint());
                            }
                        }
                        catch (Exception e)
                        {

                            Log.Error(e.ToString());
                        }
                    }
                    break;
            }
        }
        private static IEnumerator IEForceRepaint()
        {
            var select = Selection.activeObject;
            Selection.activeObject = null;
            yield return 0;
            Selection.activeObject = select;
            enumeratorRepaint = null;
        }
    }
}
