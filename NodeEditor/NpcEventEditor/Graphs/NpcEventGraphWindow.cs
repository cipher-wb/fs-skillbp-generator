using System;
using CSGameShare.CSMapEventShare;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using GraphProcessor;
using UnityEditor;
using UnityEngine;

namespace NodeEditor.NpcEventEditor
{
    public partial class NpcEventGraphWindow : ConfigGraphWindow
    {
        public override string PathExportExcel { get { return NpcEventEditorManager.Inst.Setting.PathExportExcel; } }

        public override string NameEditor => NpcEventEditorManager.Inst.Name;

        public static string HotUpdatePath => Application.dataPath + "/Thirds/NodeEditor/NpcEventEditor/HotUpdate/";

        /// <summary>
        /// 这个文件提供给NpcEventProxy解析执行
        /// </summary>
        public static readonly string HotUpdateJson = "__HotUpdate.json";

        public string ServerHotUpdate => "ServerHotUpdate";

        protected override void OnDestroy()
        {
            //移除注册运行时事件
            CloseRuntimeMode();

            base.OnDestroy();
        }

        protected override void Update()
        {
            base.Update();

            if(EnableRuntimeMode)
            {
                if (graphView is NpcEventGraphView npcEventGraphView)
                {
                    npcEventGraphView?.UpdateEdgeView();
                }
            }
        }

        protected override void CreateGraphView()
        {
            graphView = new NpcEventGraphView(this);
        }

        protected override void CreateToolbarView()
        {
            m_ToolbarView = new NpcEventGraphToolbarView(this, graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
        }

        protected override void InitializeGraphView(BaseGraphView view)
        {
            // 默认选择第一个节点
            view.Focus();
            view.FrameFirstNode();

            UpdateNodeRefState();
        }

        protected override void AfterInitializeWindow()
        {
            EditorUtility.DisplayProgressBar(TitleName, "初始化...", 0.1f);
            Utils.WatchTime($"{TitleName} 初始化", () =>
            {
                _ = NpcEventEditorManager.Inst;
            });
            base.AfterInitializeWindow();
        }

        /// <summary>
        /// 保存数据->导出exlce->同步数据
        /// </summary>
        public void SaveExportAndSyncData()
        {
            //导出excle
            var result = ExportData(configGraph, PathExportExcel, EditorFlag.SaveToAsset | EditorFlag.SaveToExcel | EditorFlag.DisplayDialog);
            if (!result)
            {
                EditorUtility.DisplayDialog(configGraph.FileName, "导表失败，暂停操作", "好的");
                return;
            }

            //生成执行__HotUpdate.json
            result = CreateHotUpdateFile();
            if (!result)
            {
                EditorUtility.DisplayDialog(configGraph.FileName, "生成热更json失败，暂停操作", "好的");
                return;
            }

            //服务器表格热更
            ExcuteBat(ServerHotUpdate);
        }

        /// <summary>
        /// 创建热更文件
        /// </summary>
        /// <returns></returns>
        public bool CreateHotUpdateFile()
        {
            string path = $"{HotUpdatePath}{HotUpdateJson}";
            using (var file = new StreamWriter(path, append: false))
            {
                var npcEventID = ((NpcEventGraph)configGraph).NpcEventID;
                if (npcEventID != 0)
                {
                    var hotUpdate = new MapEventHotUpdate(MapEventHotUpdateType.Excute | MapEventHotUpdateType.Reload, new List<int>() { npcEventID });
                    file.WriteLine(JsonConvert.SerializeObject(hotUpdate));
                    return true;
                }
            }
            return false;
        }

        private Process ExcuteBat(string cmdName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                UseShellExecute = true,
                WorkingDirectory = HotUpdatePath,
                RedirectStandardInput = false,
                Arguments = $"/k {cmdName}.bat",
            };
            Process proc = new Process { StartInfo = psi };
            proc.Start();
            //proc.WaitForExit();
            proc.Close();
            return proc;
        }
    }
}
