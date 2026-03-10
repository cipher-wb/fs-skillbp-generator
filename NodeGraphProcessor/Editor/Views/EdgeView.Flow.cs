using UnityEngine.UIElements;
using UnityEngine;
using UnityEditor;

namespace GraphProcessor
{
    public partial class EdgeView
	{
        /// <summary>
        /// 开启关闭数据流
        /// </summary>
        private bool enableFlow = false;
        public bool EnableFlow
        {
            get { return enableFlow; }
            set
            {
                if (enableFlow == value) { return; }

                enableFlow = value;

                if (enableFlow)
                {
                    Add(flowImage);
                }
                else
                {
                    Remove(flowImage);
                }
            }
        }

        private Image flowImage;

        private float FlowSize => 20f;

        private float FlowSpeed => 150f;

        private int flowPhaseIndex;

        private double flowPhaseStartTime;

        private double flowPhaseDuration;

        private float furrentPhaseLength;

        /// <summary>
        /// 初始化数据流图片
        /// </summary>
        private void InitFlowImage()
        {
            flowImage = new Image
            {
                name = "flow-image",
                style = {
                    width = new Length(FlowSize, LengthUnit.Pixel),
                    height = new Length(FlowSize, LengthUnit.Pixel),
                    borderTopLeftRadius = new Length(FlowSize / 2, LengthUnit.Pixel),
                    borderTopRightRadius = new Length(FlowSize / 2, LengthUnit.Pixel),
                    borderBottomLeftRadius = new Length(FlowSize / 2, LengthUnit.Pixel),
                    borderBottomRightRadius = new Length(FlowSize / 2, LengthUnit.Pixel),
                },
            };

            edgeControl.RegisterCallback<GeometryChangedEvent>(OnEdgeControlGeometryChanged);
        }

        /// <summary>
        /// 刷新数据流
        /// </summary>
        public void UpdateFlow()
        {
            if (!enableFlow)
            {
                return;
            }

            // Position
            var posProgress = (EditorApplication.timeSinceStartup - flowPhaseStartTime) / flowPhaseDuration;
            var flowStartPoint = edgeControl.controlPoints[flowPhaseIndex];
            var flowEndPoint = edgeControl.controlPoints[flowPhaseIndex + 1];
            var flowPos = Vector2.Lerp(flowStartPoint, flowEndPoint, (float)posProgress);
            flowImage.transform.position = flowPos - Vector2.one * FlowSize / 2;

            // Color
            flowImage.style.backgroundColor = Color.green;

            // Enter next phase
            if (posProgress >= 0.99999f)
            {
                flowPhaseIndex++;
                if (flowPhaseIndex >= edgeControl.controlPoints.Length - 1)
                {
                    // Restart flow
                    flowPhaseIndex = 0;
                }

                flowPhaseStartTime = EditorApplication.timeSinceStartup;
                furrentPhaseLength = Vector2.Distance(edgeControl.controlPoints[flowPhaseIndex],edgeControl.controlPoints[flowPhaseIndex + 1]);
                flowPhaseDuration = furrentPhaseLength / FlowSpeed;
            }
        }

        /// <summary>
        /// 布局变化后刷新
        /// </summary>
        /// <param name="evt"></param>
        private void OnEdgeControlGeometryChanged(GeometryChangedEvent evt)
        {
            // Restart flow
            flowPhaseIndex = 0;
            flowPhaseStartTime = EditorApplication.timeSinceStartup;
            furrentPhaseLength = Vector2.Distance(edgeControl.controlPoints[flowPhaseIndex],edgeControl.controlPoints[flowPhaseIndex + 1]);
            flowPhaseDuration = furrentPhaseLength / FlowSpeed;
        }
    }
}