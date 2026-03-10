using System.Collections.Generic;
using UnityEngine;

namespace GraphProcessor
{
    // TODO 整理代码，有点乱，统一GraphElement
    // UnReDo
    public partial class BaseGraphView
    {
        public enum URDControlType
        {
            URDCT_NodePos = 1,              //改变坐标
            URDCT_ConfigNodeChange = 2,     //Config节点修改
            URDCT_AddNode = 3,              //增删节点
            URDCT_EdgeCountChange = 4,      //连线数量变化
            URDCT_AddGroup = 5,             //增加Group
            URDCT_RemoveGroup = 6,          //移除Group
            URDCT_GroupChange = 7,          //Group信息修改
            URDCT_ElementsToRemove = 8,     //节点移除
            URDCT_ElementsToPaste = 9,      //节点粘贴

            URDCT_StickyNote_Add,           // 便笺-添加
            URDCT_StickyNote_Move,          // 便笺-移动
        }
        public class NodeUnReDo
        {
            public string NodeID;
            public URDControlType ControlType;
            public string OldValue;
            public string NewValue;

            public NodeUnReDo(string id, URDControlType ctype, string oValue, string nValue)
            {
                NodeID = id;
                ControlType = ctype;
                OldValue = oValue;
                NewValue = nValue;
            }

            public string Tostring()
            {
                return string.Format("{0}|{1}|{2}|{3}", NodeID, (int)ControlType, OldValue, NewValue);
            }
        }

        private int unReDoRecordCount = 1000;
        protected List<NodeUnReDo> UnReDoInfoList;
        protected int CurrentUnReDoIndex = 0;

        public void AddUnReDoInfo(string id, URDControlType ctype, string oValue, string nValue)
        {
            if (string.IsNullOrEmpty(oValue) && string.IsNullOrEmpty(nValue))
                return;
            //Debug.Log($"--## UnRedo Add ========= {CurrentUnReDoIndex}, ID:{id}, Type:{ctype}, Old:{oValue}, New:{nValue}");
            if (ctype == URDControlType.URDCT_EdgeCountChange)  //连线数量变化  OldValue为断开的连线，NewValue为新建的连线
            {
                var unredo = UnReDoInfoList != null ? UnReDoInfoList.Find((nurDo) => { return nurDo.NodeID == id; }) : null;
                if (unredo != null)
                {
                    if (!string.IsNullOrEmpty(oValue))
                    {
                        if (string.IsNullOrEmpty(unredo.OldValue))
                            unredo.OldValue = oValue;
                        else if (!unredo.OldValue.Contains(oValue))
                            unredo.OldValue = string.Format("{0}|{1}", unredo.OldValue, oValue);
                    }
                    if (!string.IsNullOrEmpty(nValue))
                    {
                        if (string.IsNullOrEmpty(unredo.NewValue))
                            unredo.NewValue = nValue;
                        else if (!unredo.NewValue.Contains(nValue))
                            unredo.NewValue = string.Format("{0}|{1}", unredo.NewValue, nValue);
                    }
                }
                else
                {
                    unredo = new NodeUnReDo(id, ctype, oValue, nValue);
                    AddUnReDoInfo(unredo);
                }
            }
            else if (ctype == URDControlType.URDCT_NodePos)
            {
                var lastUnReDo = UnReDoInfoList != null && CurrentUnReDoIndex > 0 ? UnReDoInfoList[CurrentUnReDoIndex - 1] : null;
                if (lastUnReDo != null && lastUnReDo.NodeID == id)
                {
                    var oVArr = oValue.Split(":");
                    var oNodeID = oVArr[0];
                    if (!lastUnReDo.OldValue.Contains(oNodeID))
                        lastUnReDo.OldValue = $"{lastUnReDo.OldValue}|{oValue}";
                    var nVArr = nValue.Split(":");
                    var nNodeID = nVArr[0];
                    if (lastUnReDo.NewValue.Contains(nNodeID))
                    {
                        var valueArr = lastUnReDo.NewValue.Split("|");
                        for (var i = 0; i < valueArr.Length; ++i)
                        {
                            if (valueArr[i].Contains(nNodeID))
                            {
                                lastUnReDo.NewValue = lastUnReDo.NewValue.Replace(valueArr[i], nValue);
                                break;
                            }
                        }
                    }
                    else
                    {
                        lastUnReDo.NewValue = $"{lastUnReDo.NewValue}|{nValue}";
                    }
                    lastUnReDo.NewValue = nValue;
                }
                else
                {
                    NodeUnReDo unredo = new NodeUnReDo(id, ctype, oValue, nValue);
                    AddUnReDoInfo(unredo);
                }
            }
            else if (ctype == URDControlType.URDCT_GroupChange)
            {
                var lastUnReDo = UnReDoInfoList != null && CurrentUnReDoIndex > 0 ? UnReDoInfoList[CurrentUnReDoIndex - 1] : null;
                if (lastUnReDo != null && lastUnReDo.NodeID == id)
                {
                    lastUnReDo.NewValue = nValue;
                }
                else
                {
                    NodeUnReDo unredo = new NodeUnReDo(id, ctype, oValue, nValue);
                    AddUnReDoInfo(unredo);
                }
            }
            else
            {
                NodeUnReDo unredo = new NodeUnReDo(id, ctype, oValue, nValue);
                AddUnReDoInfo(unredo);
            }
        }

        public void AddUnReDoInfo(NodeUnReDo unredo)
        {
            if (UnReDoInfoList == null)
            {
                UnReDoInfoList = new List<NodeUnReDo>();
                UnReDoInfoList.Add(unredo);
                CurrentUnReDoIndex = 1;
                return;
            }
            //如果当前回退下标不在最后，移除当前下标之后的数据
            if (CurrentUnReDoIndex < UnReDoInfoList.Count)
            {
                UnReDoInfoList.RemoveRange(CurrentUnReDoIndex, UnReDoInfoList.Count - CurrentUnReDoIndex);
            }

            UnReDoInfoList.Add(unredo);
            CurrentUnReDoIndex = UnReDoInfoList.Count;

            if (CurrentUnReDoIndex > unReDoRecordCount)
            {
                UnReDoInfoList.RemoveRange(0, CurrentUnReDoIndex - unReDoRecordCount);
                CurrentUnReDoIndex = unReDoRecordCount;
            }
        }

        public void OnNodeUnDo()
        {
            if (CurrentUnReDoIndex < 1)
                return;
            var currUnReDo = UnReDoInfoList[CurrentUnReDoIndex - 1];
            OnUnReDo(currUnReDo.NodeID, currUnReDo.ControlType, currUnReDo.OldValue, currUnReDo.NewValue);
            CurrentUnReDoIndex--;
        }

        public void OnNodeReDo()
        {
            if (CurrentUnReDoIndex >= UnReDoInfoList.Count)
                return;
            var currUnReDo = UnReDoInfoList[CurrentUnReDoIndex];
            OnUnReDo(currUnReDo.NodeID, currUnReDo.ControlType, currUnReDo.NewValue, currUnReDo.OldValue);
            CurrentUnReDoIndex++;
        }

        private void OnUnReDo(string nodeID, URDControlType controlType, string newValue, string oldValue)
        {
            switch (controlType)
            {
                case URDControlType.URDCT_NodePos:
                    {
                        var posDataArr = newValue.Split("|");
                        for (var i = 0; i < posDataArr.Length; ++i)
                        {
                            var posSet = posDataArr[i].Split(":");
                            var nodeGuid = posSet[0];
                            var posData = posSet[1];
                            var nodeView = nodeViews.Find(n => n.nodeTarget.GUID == nodeGuid);
                            if (nodeView != null)
                            {
                                var rect = nodeView.GetPosition();
                                var posStr = posData.Replace("(", "").Replace(")", "");
                                var posArr = posStr.Split(",");
                                var posX = posArr.Length > 0 ? float.Parse(posArr[0]) : rect.x;
                                var posY = posArr.Length > 1 ? float.Parse(posArr[1]) : rect.y;
                                rect.position = new Vector2(posX, posY);
                                nodeView.initializing = true;
                                nodeView.SetPosition(rect);
                            }
                        }
                    }
                    break;
                case URDControlType.URDCT_ConfigNodeChange:
                    {
                        var nodeView = nodeViews.Find(n => n.nodeTarget.GUID == nodeID);
                        if (nodeView != null)
                        {
                            nodeView.initializing = true;
                            nodeView.ResetConfigNodeView(newValue);
                            nodeView.initializing = false;
                        }
                    }
                    break;
                case URDControlType.URDCT_AddNode:
                    {
                        initializing = true;
                        if (string.IsNullOrEmpty(newValue)) //数据是空，执行删节点
                        {
                            var nodeView = nodeViews.Find(n => n.nodeTarget.GUID == nodeID);
                            if (nodeView != null)
                            {
                                RemoveNode(nodeView.nodeTarget);
                            }
                        }
                        else    //有节点数据，执行增加节点
                        {
                            var nodeEle = JsonUtility.FromJson<JsonElement>(newValue);
                            var node = JsonSerializer.DeserializeNode(nodeEle);
                            if (node != null)
                            {
                                node.OnNodeRecreated();
                                node.OnNodePostionUpdated();
                                var nodeView = AddNode(node);
                                nodeView.ResetConfigNodeView(null);
                            }
                        }
                        initializing = false;
                    }
                    break;
                case URDControlType.URDCT_EdgeCountChange:
                    {
                        initializing = true;
                        if (!string.IsNullOrEmpty(oldValue))
                        {
                            var disconnectArr = oldValue.Split("|");
                            foreach (var disconnect in disconnectArr)
                            {
                                var edge = JsonUtility.FromJson<SerializableEdge>(disconnect);
                                edge.Deserialize(graph);
                                var edgeGUID = edge.GUID;
                                graph.Disconnect(edgeGUID);
                            }
                        }
                        if (!string.IsNullOrEmpty(newValue))
                        {
                            var connectArr = newValue.Split("|");
                            foreach (var connect in connectArr)
                            {
                                var edge = JsonUtility.FromJson<SerializableEdge>(connect);
                                edge.Deserialize(graph);
                                var inputNode = edge.inputNode;
                                var outputNode = edge.outputNode;
                                if (inputNode != null && outputNode != null)
                                {
                                    var inputPort = inputNode.GetPort(edge.inputPort.fieldName, edge.inputPortIdentifier);
                                    var outputPort = outputNode.GetPort(edge.outputPort.fieldName, edge.outputPortIdentifier);

                                    var newEdge = SerializableEdge.CreateNewEdge(graph, inputPort, outputPort);
                                    if (nodeViewsPerNode.TryGetValue(inputNode, out var oldInputNodeView) && nodeViewsPerNode.TryGetValue(outputNode, out var oldOutputNodeView))
                                    {
                                        var edgeView = CreateEdgeView();
                                        edgeView.userData = newEdge;
                                        edgeView.input = oldInputNodeView.GetPortViewFromFieldName(newEdge.inputFieldName, newEdge.inputPortIdentifier);
                                        edgeView.output = oldOutputNodeView.GetPortViewFromFieldName(newEdge.outputFieldName, newEdge.outputPortIdentifier);

                                        Connect(edgeView);
                                    }
                                }
                            }
                        }
                        initializing = false;
                    }
                    break;
                case URDControlType.URDCT_AddGroup:
                    {
                        var groupView = groupViews.Find(n => n.group.GUID == nodeID);
                        if (groupView != null)
                        {
                            RemoveGroupView(groupView);
                        }
                    }
                    break;
                case URDControlType.URDCT_RemoveGroup:
                    {
                        initializing = true;
                        if (!string.IsNullOrEmpty(oldValue))
                        {
                            var groupEle = JsonUtility.FromJson<JsonElement>(oldValue);
                            var group = JsonSerializer.Deserialize<Group>(groupEle);
                            AddGroup(group);
                        }
                        initializing = false;
                    }
                    break;
                case URDControlType.URDCT_GroupChange:
                    {
                        initializing = true;
                        var groupView = groupViews.Find(n => n.group.GUID == nodeID);
                        if (groupView != null)
                        {
                            var doGroup = JsonUtility.FromJson<Group>(newValue);
                            if (doGroup != null)
                            {
                                groupView.initializing = true;
                                groupView.SetPosition(doGroup.position);
                                groupView.UpdateGroupColor(doGroup.color);
                                groupView.TitleLabel.text = doGroup.title;
                                groupView.initializing = false;
                            }
                        }
                        initializing = false;
                    }
                    break;
                case URDControlType.URDCT_ElementsToRemove:
                    {
                        initializing = true;
                        if (!string.IsNullOrEmpty(oldValue))
                        {
                            var removeArr = oldValue.Split("|");
                            foreach (var removeInfo in removeArr)
                            {
                                var removeEle = JsonUtility.FromJson<JsonElement>(removeInfo);
                                if (removeEle.type.Contains("NodeEditor."))
                                {
                                    var node = JsonSerializer.DeserializeNode(removeEle);
                                    if (node != null)
                                    {
                                        node.OnNodeRecreated();
                                        node.OnNodePostionUpdated();
                                        var nodeView = AddNode(node);
                                        // TODO 检查回退数据丢失问题，重置能解决，看看是啥原因
                                        nodeView.ResetConfigNodeView(null);
                                    }
                                }
                                else if (removeEle.type.Contains("SerializableEdge"))
                                {
                                    var edge = JsonSerializer.Deserialize<SerializableEdge>(removeEle);
                                    edge.Deserialize(graph);
                                    var inputNode = edge.inputNode;
                                    var outputNode = edge.outputNode;
                                    if (inputNode != null && outputNode != null)
                                    {
                                        var inputPort = inputNode.GetPort(edge.inputPort.fieldName, edge.inputPortIdentifier);
                                        var outputPort = outputNode.GetPort(edge.outputPort.fieldName, edge.outputPortIdentifier);

                                        var newEdge = SerializableEdge.CreateNewEdge(graph, inputPort, outputPort);
                                        if (nodeViewsPerNode.TryGetValue(inputNode, out var oldInputNodeView) && nodeViewsPerNode.TryGetValue(outputNode, out var oldOutputNodeView))
                                        {
                                            var edgeView = CreateEdgeView();
                                            edgeView.userData = newEdge;
                                            edgeView.input = oldInputNodeView.GetPortViewFromFieldName(newEdge.inputFieldName, newEdge.inputPortIdentifier);
                                            edgeView.output = oldOutputNodeView.GetPortViewFromFieldName(newEdge.outputFieldName, newEdge.outputPortIdentifier);

                                            Connect(edgeView);
                                        }
                                    }
                                }
                                else if (removeEle.type.Contains("Group"))
                                {
                                    var group = JsonSerializer.Deserialize<Group>(removeEle);
                                    AddGroup(group);
                                }

                            }
                        }
                        initializing = false;
                    }
                    break;
                case URDControlType.URDCT_ElementsToPaste:
                    {
                        initializing = true;
                        if (!string.IsNullOrEmpty(oldValue))
                        {
                            List<string> edgeList = new List<string>(), nodeList = new List<string>(), groupList = new List<string>();
                            var pasteArr = oldValue.Split("|");
                            foreach (var pasteItem in pasteArr)
                            {
                                var itemInfos = pasteItem.Split(":");
                                if (itemInfos.Length != 2)
                                    continue;
                                var pType = itemInfos[0];
                                var pUID = itemInfos[1];
                                if (pType == "edge")
                                {
                                    edgeList.Add(pUID);
                                }
                                if (pType == "node")
                                {
                                    nodeList.Add(pUID);
                                }
                                if (pType == "group")
                                {
                                    groupList.Add(pUID);
                                }
                            }
                            foreach (var edgeId in edgeList)
                            {
                                graph.Disconnect(edgeId);
                            }
                            foreach (var nodeId in nodeList)
                            {
                                var nodeView = nodeViews.Find(n => n.nodeTarget.GUID == nodeId);
                                if (nodeView != null)
                                {
                                    RemoveNode(nodeView.nodeTarget);
                                }
                            }
                            foreach (var groupId in groupList)
                            {
                                var groupView = groupViews.Find(n => n.group.GUID == groupId);
                                if (groupView != null)
                                {
                                    graph.RemoveGroup(groupView.group);
                                    UpdateSerializedProperties();
                                    RemoveElement(groupView);
                                }
                            }
                        }
                        initializing = false;
                    }
                    break;
                case URDControlType.URDCT_StickyNote_Add:
                    {
                        initializing = true;
                        if (string.IsNullOrEmpty(newValue)) //数据是空，执行删节点
                        {
                            var view = stickyNoteViews.Find(n => n.note.GUID == nodeID);
                            if (view != null)
                            {
                                RemoveStickyNote(view.note);
                            }
                        }
                        else    //有节点数据，执行增加节点
                        {
                            var nodeEle = JsonUtility.FromJson<JsonElement>(newValue);
                            var node = JsonSerializer.Deserialize<StickyNote>(nodeEle);
                            if (node != null)
                            {
                                var nodeView = AddStickyNote(node);
                            }
                        }
                        initializing = false;
                        break;
                    }
                case URDControlType.URDCT_StickyNote_Move:
                    {
                        var posDataArr = newValue.Split("|");
                        for (var i = 0; i < posDataArr.Length; ++i)
                        {
                            var posSet = posDataArr[i].Split(":");
                            var nodeGuid = posSet[0];
                            var posData = posSet[1];
                            var view = stickyNoteViews.Find(n => n.note.GUID == nodeGuid);
                            if (view != null)
                            {
                                var rect = view.GetPosition();
                                var posStr = posData.Replace("(", "").Replace(")", "");
                                var posArr = posStr.Split(",");
                                var posX = posArr.Length > 0 ? float.Parse(posArr[0]) : rect.x;
                                var posY = posArr.Length > 1 ? float.Parse(posArr[1]) : rect.y;
                                rect.position = new Vector2(posX, posY);
                                view.initializing = true;
                                view.SetPosition(rect);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
