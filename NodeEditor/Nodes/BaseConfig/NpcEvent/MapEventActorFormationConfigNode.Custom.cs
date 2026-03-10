using GraphProcessor;
using NodeEditor.PortType;
using NodeGraphProcessor.Examples;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public partial class MapEventActorFormationConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            if (string.IsNullOrEmpty(Config.Desc))
            {
                SetCustomName($"[设置站位][{Config.ID}]");
            }
            else
            {
                SetCustomName($"[设置站位][{Config.Desc}][{Config.ID}]");
            }
        }

        [Button("获取所有演员列表")]
        private void GeneralActorFormations()
        {
            var linkNodes = GetLinkNodes();
            var actorCount = linkNodes?.Count ?? 0;

            var formations = new List<ActorFormation>();
            for (int i = 0; i < actorCount; i++)
            {
                formations.Add(new ActorFormation(i, 0, 0, 0));
            }

            SetConfigValue("Formations", formations);
        }

        private List<NpcEventLinkConfigNode> GetLinkNodes()
        {
            var eventNode = GetPreviousNode<NpcEventConfigNode>();
            if (eventNode == default) { return default; }

            var nodes = eventNode.GetNextNodes<NpcEventLinkConfigNode>(typeof(ConfigPortType_NpcEventLinkConfig));
            return nodes;
        }
    }
}
