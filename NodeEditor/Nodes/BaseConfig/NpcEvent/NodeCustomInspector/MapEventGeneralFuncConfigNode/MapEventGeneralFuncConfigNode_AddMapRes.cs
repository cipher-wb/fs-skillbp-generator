using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_AddMapRes : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_AddMapRes(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region 物产设置
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("物产")]
        [OnValueChanged("OnChangeMapResData", true), DelayedProperty]
        public AddMapResData MapResData { get; private set; } = new AddMapResData()
        {
            MapResType = TMapResType.TMRT_BOX,
            MapResTable = new TableSelectData(typeof(MapBoxConfig).FullName, 0)
        };

        private void OnChangeMapResData()
        {
            baseNode.Config?.ExSetValue("IntParams1", MapResData.ToIntParams1());

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if (MapResData.MapResTable.ID <= 0)
            {
                baseNode.InspectorError += $"【表格未选择】\n";
            }

            if (MapResData.Count <= 0)
            {
                baseNode.InspectorError += $"【数量错误】\n";
            }

            //相对坐标
            if (MapResData.PosType == CoordType.Relative &&
                (MapResData.Target.TargetType == MapEventTargetType.MapEventTargetType_Null
                || MapResData.Target.TargetType == MapEventTargetType.MapEventTargetType_AllCostar))
            {
                baseNode.InspectorError += $"【相对坐标，目标类型错误】\n";
            }
            //绝对坐标
            else if (MapResData.PosType == CoordType.Absolute && (MapResData.PosX == 0 || MapResData.PosY == 0))
            {
                baseNode.InspectorError += $"【绝对坐标, 坐标错误】\n";
            }

            //范围错误
            if (MapResData.Range <= 0)
            {
                baseNode.InspectorError += $"【随即半径错误】\n";
            }
        }

        public void ConfigToData()
        {
            MapResData.Update(baseNode.Config.IntParams1);
        }

        public void SetDefault()
        {
            MapResData = new AddMapResData()
            {
                MapResType = TMapResType.TMRT_BOX,
                MapResTable = new TableSelectData(typeof(MapBoxConfig).FullName, 0)
            };

            baseNode.Config?.ExSetValue("IntParams1", MapResData.ToIntParams1());
        }
    }
}
