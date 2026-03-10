using GraphProcessor;
using System;
using UnityEditor;
using System.IO;

namespace NodeEditor.NpcEventEditor
{
    [Serializable]
    public partial class NpcEventGraph : ConfigGraph
    {
        public override string Version => Constants.GamePlayEditor.Version;

        /// <summary>
        /// npc事件ID
        /// </summary>
        public int NpcEventID
        {
            get
            {
                var splitArray = FileName.Split('_');
                if (splitArray.Length > 2 && int.TryParse(splitArray[1], out int npcEventID))
                {
                    return npcEventID;
                }
                return 0;
            }
        }
    }
}
