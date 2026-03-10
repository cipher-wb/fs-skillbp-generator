using GameApp;
using GameApp.Event;
using GraphProcessor;
using HotFix.Game.EventArgs;
using HotFix.Game.MapEvent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor.NpcEventEditor
{
    public partial class NpcEventGraphWindow
    {
        public bool EnableRuntimeMode { get; private set; } = false;

        /// <summary>
        /// 注册运行时事件
        /// </summary>
        private void RegisterRuntimeEvent()
        {
            AppFacade.Event.Subscribe(NpcEventDataUpdateArgs.EventId, OnNpcEventDataUpdate);
        }

        /// <summary>
        /// 取消注册运行时事件
        /// </summary>
        private void UnRegisterRuntimeEvent()
        {
            AppFacade.Event.Unsubscribe(NpcEventDataUpdateArgs.EventId, OnNpcEventDataUpdate);
        }

        private void OnNpcEventDataUpdate(object sender, GameEngineEventArgs args)
        {
            if (graphView is NpcEventGraphView npcEventGraphView)
            {
                var arg = args as NpcEventDataUpdateArgs;

                //必须是同一个事件才能刷新
                if (arg.EventID.ConfigID != npcEventGraphView.EventID) { return; }

                npcEventGraphView.AddRuntimeData(new NpcEventRuntimeData()
                {
                    RuntimeType = NpcEventRuntimeType.UpdateActorActionID,
                    ActorIndex = arg.ActorIndex,
                    ActionID = arg.ActionID,
                });
            }
        }

        /// <summary>
        /// 开启运行模式
        /// </summary>
        public void OpenRuntimeMode()
        {
            if (graphView is NpcEventGraphView npcEventGraphView)
            {
                EnableRuntimeMode = true;

                if (Application.isPlaying)
                {
                    UnRegisterRuntimeEvent();
                    RegisterRuntimeEvent();

                    npcEventGraphView.OpenRuntimeMode();
                }
                else
                {

                    npcEventGraphView.ShowAllEdgeFlow();
                }
            }
        }

        /// <summary>
        /// 关闭运行模式
        /// </summary>
        public void CloseRuntimeMode()
        {
            EnableRuntimeMode = false;

            if (graphView is NpcEventGraphView npcEventGraphView)
            {
                npcEventGraphView.ClearAllEdgeFlow();
            }

            if (Application.isPlaying)
            {
                UnRegisterRuntimeEvent();
            }
        }
    }
}
