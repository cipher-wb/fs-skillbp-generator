using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 表演节点
    /// </summary>
    public partial class MapEventPerformanceConfigNode
    {
        /// <summary>
        /// 类型映射
        /// </summary>
        private Dictionary<TPerformanceType, Type> performanceTypeMapping = new Dictionary<TPerformanceType, Type>()
        {
            { TPerformanceType.TPerformanceType_Move, typeof(MapEventPerformanceConfigNode_Move) },
            { TPerformanceType.TPerformanceType_Wait, typeof(MapEventPerformanceConfigNode_Wait) },
            { TPerformanceType.TPerformanceType_CustomModel, typeof(MapEventPerformanceConfigNode_CreateCustomModel) },
            { TPerformanceType.TPerformanceType_RemovePerformancer, typeof(MapEventPerformanceConfigNode_RemovePerformancer) },
            { TPerformanceType.TPerformanceType_PlayAnim, typeof(MapEventPerformanceConfigNode_PlayAnim) },
            { TPerformanceType.TPerformanceType_PlayEmoji, typeof(MapEventPerformanceConfigNode_PlayEmoji) },
            { TPerformanceType.TPerformanceType_PlayBubble, typeof(MapEventPerformanceConfigNode_PlayBubble) },
            { TPerformanceType.TPerformanceType_RoleDialog, typeof(MapEventPerformanceConfigNode_RoleDialog) },
            { TPerformanceType.TPerformanceType_Rotate, typeof(MapEventPerformanceConfigNode_Rotate) },
            { TPerformanceType.TPerformanceType_PlaySound, typeof(MapEventPerformanceConfigNode_PlaySound) },
            { TPerformanceType.TPerformanceType_StopSound, typeof(MapEventPerformanceConfigNode_StopSound) },
            { TPerformanceType.TPerformanceType_FadeInOut, typeof(MapEventPerformanceConfigNode_FadeInOut) },
            { TPerformanceType.TPerformanceType_SpecialMove , typeof(MapEventPerformanceConfigNode_SpecialMove)}
        };

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, HideLabel, ShowIf("@performanceType != TPerformanceType.TPerformanceType_Null")]
        [InfoBox("@InspectorError", InfoMessageType.Error, "IsExitInspectorError")]
        //[BoxGroup("参数设置", false, true, order:1)]
        public INodeCustomInspector NodeCustomInspector { get; private set; }

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            var title = $"[{Config.ID}][表演][{EnumUtility.GetDescription(Config.Type, false)}]";
            SetCustomName(title);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            RestorePerformanceType();
        }

        public override bool OnSaveCheck()
        {
            if (!base.OnSaveCheck()) { return false; }

            if (IsExitInspectorError)
            {
                AppendSaveMapEventRet(InspectorError);
                return false;
            }

            return true;
        }

        #region 行为类型
        [Sirenix.OdinInspector.ShowInInspector, LabelText("类型"), HideReferenceObjectPicker]
        [FoldoutGroup("行为类型", true, 0)]
        [OnValueChanged("OnChangedPerformanceType"), DelayedProperty]
        private TPerformanceType performanceType = TPerformanceType.TPerformanceType_Null;

        private void OnChangedPerformanceType()
        {
            Config?.ExSetValue("Type", performanceType);

            OnRefreshCustomName();

            if(performanceTypeMapping.TryGetValue(performanceType, out var inspectorType))
            {
                NodeCustomInspector = Activator.CreateInstance(inspectorType, this) as INodeCustomInspector;
                NodeCustomInspector.SetDefault();
                NodeCustomInspector.CheckError();
            }
        }

        private void RestorePerformanceType()
        {
            performanceType = Config?.Type ?? TPerformanceType.TPerformanceType_Null;

            if (performanceTypeMapping.TryGetValue(performanceType, out var inspectorType))
            {
                NodeCustomInspector ??= Activator.CreateInstance(inspectorType, this) as INodeCustomInspector;
                NodeCustomInspector.ConfigToData();
                NodeCustomInspector.CheckError();
            }
        }
        #endregion
    }
}
