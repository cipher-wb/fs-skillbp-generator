using GraphProcessor;
using HotFix.Game.Data;
using HotFix.Game.MapEvent;
using System.Collections.Generic;
using System.Linq;

namespace NodeEditor.NpcEventEditor
{
    public partial class NpcEventGraphView
    {
        private Queue<NpcEventRuntimeData> processRuntimeQueue= new Queue<NpcEventRuntimeData>();

        public void UpdateEdgeView()
        {
            foreach (var edge in edgeViews)
            {
                edge.OnUpdate();
            }
        }

        public void OpenRuntimeMode()
        {
            AddRuntimeData(new NpcEventRuntimeData() { RuntimeType = NpcEventRuntimeType.Start });
        }

        public void CloseRuntimeMode()
        {
            AddRuntimeData(new NpcEventRuntimeData() { RuntimeType = NpcEventRuntimeType.Close });
        }

        public void AddRuntimeData(NpcEventRuntimeData data)
        {
            processRuntimeQueue.Enqueue(data);

            ExcuteRuntimeData();
        }

        public void ExcuteRuntimeData()
        {
            if(processRuntimeQueue.Count <= 0) { return; }

            var data = processRuntimeQueue.Dequeue();
            switch(data.RuntimeType)
            {
                case NpcEventRuntimeType.Start:
                    {
                        OnRuntimeStart();
                    }
                    break;
                case NpcEventRuntimeType.UpdateActorActionID:
                    {
                        OnUpdateActorActionID(data);
                    }
                    break;
            }
        }

        /// <summary>
        /// 启动运行时
        /// </summary>
        public void OnRuntimeStart()
        {
#if UNITY_EDITOR
            if (EventID == 0) { return; }

            var debugMapEventData = GameUser.MapEventSummary.DebugMapEventData;
            if(debugMapEventData == default) { return; }

            var actionIDList = debugMapEventData.GetRuntimeActionList();
            if(actionIDList == default) { return; }

            var nodeList = new List<BaseNode>();

            //添加所有角色的行为节点
            foreach (var runtimeActionID in actionIDList)
            {
                var actionNode = GetNpcEventActionNode(runtimeActionID);
                if (actionNode == default) { continue; }

                //添加自己和子行为
                actionNode.GetRuntimeNodes(nodeList);

                //添加父节点
                actionNode.GetParentNodes(nodeList);
            }

            //设置edge数据流
            foreach (var edge in edgeViews)
            {
                if (nodeList.Any(node => node.GUID == edge.serializedEdge.inputNode.GUID))
                {
                    edge.EnableFlow = true;
                }
            }
#endif
        }

        /// <summary>
        /// 刷新演员行为
        /// </summary>
        public void OnUpdateActorActionID(NpcEventRuntimeData data)
        {
            var actionNode = GetNpcEventActionNode(data.ActionID);
            if (actionNode == default) { return; }

            var nodeList = new List<BaseNode>();
            //添加自己和子行为
            actionNode.GetRuntimeNodes(nodeList);

            //添加父节点
            actionNode.GetParentNodes(nodeList);

            //设置edge数据流
            foreach (var edge in edgeViews)
            {
                if (nodeList.Any(node => node.GUID == edge.serializedEdge.inputNode.GUID))
                {
                    edge.EnableFlow = true;
                }
            }
        }

        public void ClearAllEdgeFlow()
        {
            foreach(var edge in edgeViews)
            {
                edge.EnableFlow = false;
            }
        }

        public void ShowAllEdgeFlow()
        {
            foreach (var edge in edgeViews)
            {
                edge.EnableFlow = true;
            }
        }
    }
}
