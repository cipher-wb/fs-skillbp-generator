using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class NpcTalkConfigNode
    {

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][对话][演员{Config.NpcIndex}]");
        }

        /// <summary>
        /// 当连接到ID的时候，刷新对白组ID
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            if(Config.IntervalSecs.Count > 2)
            {
                var intervalSecs = new List<int>();
                for(int i = 0; i < 2; i++)
                {
                    intervalSecs.Add(Config.IntervalSecs[i]);
                }
                SetConfigValue(nameof(Config.IntervalSecs), intervalSecs);
            }

            if(edges.Count > 0 )
            {
                var edge = edges.First();
                //NpcTalkGroupConfigNode.ID 同步到 NpcTalkConfigNode.TalkGroupID
                if (edge.outputNode != null && edge.outputNode is NpcTalkGroupConfigNode outputConfigBaseNode)
                {
                    //刷新对白组ID
                    SetConfigValue(nameof(Config.TalkGroupID), outputConfigBaseNode.ID);
                }
                else
                {
                    SetConfigValue(nameof(Config.TalkGroupID), null);
                }
            }
        }
    }
}
