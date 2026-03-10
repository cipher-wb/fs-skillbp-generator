using Sirenix.OdinInspector;
using static NodeEditor.MapEventPerformanceConfigNode;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class PlayCustomModelData
    {
        [LabelText("自定义ID")]
        public int CustomID;

        [HideReferenceObjectPicker, LabelText("模型ID")]
        public TableSelectData ModelTable;

        /// <summary>
        /// 自定义对象坐标类型
        /// 1.固定点 
        /// 2.跟随演员
        /// </summary>
        [LabelText("坐标类型")]
        public ModelPositionType PositionType;

        [LabelText("固定点坐标"), ShowIf("@PositionType == MapEventPerformanceConfigNode.ModelPositionType.Position")]
        public Vector2 Position;

        [LabelText("朝向")]
        public int Yaw = 180;

        public PlayCustomModelData() { }

        public PlayCustomModelData(string param)
        {
            ToData(param);
        }

        //自定义ID|特效ID|特效类型|坐标X|坐标Y
        public override string ToString()
        {
            return $"{CustomID}|{ModelTable.ID}|{(int)PositionType}|{(int)Position.x}|{(int)Position.y}|{Yaw}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param)) 
            {
                ModelTable = new TableSelectData(typeof(ModelConfig).FullName, 0);
                PositionType = ModelPositionType.Position;
                return; 
            }

            var split = param.Split('|');
            if (split.Length < 3) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1)
                || !int.TryParse(split[2], out var param2)) { return; }

            CustomID = param0;

            //获取特效
            ModelTable = new TableSelectData(typeof(ModelConfig).FullName, param1);
            ModelTable.OnSelectedID();

            //特效类型
            PositionType = (ModelPositionType)param2;

            //固定点
            if (PositionType == ModelPositionType.Position)
            {
                if (split.Length > 4
                    && int.TryParse(split[3], out var param3) 
                    && int.TryParse(split[4], out var param4))
                {
                    Position = new Vector2(param3, param4);
                }
            }

            //朝向
            if (split.Length > 5 && int.TryParse(split[5], out var param5))
            {
                Yaw = param5;
            }
        }
    }

    public class MapEventPerformanceConfigNode_CreateCustomModel : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_CreateCustomModel(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlayCustomModelData perfData = new PlayCustomModelData();

        private void OnParamChanged()
        {
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;
        }

        public void ConfigToData()
        {
            perfData = new PlayCustomModelData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PlayCustomModelData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}
