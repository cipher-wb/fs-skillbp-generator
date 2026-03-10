using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class PlayRoleDialogData
    {
        [LabelText("文本")]
        public string TalkText;

        public PlayRoleDialogData()
        {

        }

        public PlayRoleDialogData(string param, string text)
        {
            ToData(param, text);
        }

        public override string ToString()
        {
            return "";
        }

        public void ToData(string param, string text)
        {
            TalkText = text;
        }
    }

    public class MapEventPerformanceConfigNode_RoleDialog : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_RoleDialog(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlayRoleDialogData perfData = new PlayRoleDialogData();

        private void OnParamChanged()
        {
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.TextEditor), perfData.TalkText);

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;
        }

        public void ConfigToData()
        {
            perfData = new PlayRoleDialogData(baseNode.Config.Param, baseNode.Config.TextEditor);
        }

        public void SetDefault()
        {
            perfData = new PlayRoleDialogData(string.Empty, string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}
