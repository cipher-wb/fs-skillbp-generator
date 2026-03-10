using Sirenix.OdinInspector;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_SetVariable : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_SetVariable(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("变量")]
        [OnValueChanged("OnChangedVariableData", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddVariableData")]
        public List<SetVariableData> SetVariableDatas = new List<SetVariableData>();

        private SetVariableData OnAddVariableData()
        {
            return new SetVariableData(0, 0, SymbolType.Set);
        }

        private void OnChangedVariableData()
        {
            var dyList = new List<GFDynamic>();
            SetVariableDatas?.ForEach(data =>
            {
                var dyData = new GFDynamic();
                dyData.ExSetValue("DynmaicInt1", data.VariableData.ID);
                dyData.ExSetValue("DynmaicInt2", data.Value);
                dyData.ExSetValue("DynmaicInt3", (int)(data.SymbolType));
                dyList.Add(dyData);
            });
            baseNode.Config?.ExSetValue("DynamicClass1", dyList);

            CheckError();
        }

        public void ConfigToData()
        {
            SetVariableDatas.Clear();
            baseNode.Config.DynamicClass1?.ForEach(gfData =>
            {
                var tableData = new SetVariableData(gfData.DynmaicInt1, gfData.DynmaicInt2, (SymbolType)(gfData.DynmaicInt3));
                SetVariableDatas.Add(tableData);
            });
        }

        public void SetDefault()
        {

        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(SetVariableDatas.Count == 0)
            {
                baseNode.InspectorError += "【没有需要设置的变量】";
            }

            SetVariableDatas?.ForEach(data =>
            {
                baseNode.AddInspectorErrorTableNotSelect(data.VariableData);
            });
        }
    }
}
