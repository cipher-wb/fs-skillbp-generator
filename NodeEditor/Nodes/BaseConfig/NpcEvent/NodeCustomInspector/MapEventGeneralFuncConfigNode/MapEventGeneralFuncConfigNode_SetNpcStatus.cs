using Sirenix.OdinInspector;
using System.Collections.Generic;
using Funny.Base.Utils;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_SetNpcStatus : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_SetNpcStatus(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region RoleExp
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("境界")]
        [OnValueChanged("OnNpcStateDataChanged", true), DelayedProperty]
        [ValueDropdown("@MapEventGeneralFuncConfigNode.VDL_State")]
        public int NpcState { get; private set; }

        private void OnNpcStateDataChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int>() { NpcState });

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(NpcState == 0)
            {
                baseNode.InspectorError += "【境界未选择】";
            }
        }

        public void ConfigToData()
        {
            baseNode.Config.IntParams1?.ForEach(npcState =>
            {
                NpcState = npcState;
            });
        }

        public void SetDefault()
        {

        }
    }
}
