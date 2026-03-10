using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphProcessor
{
    public class BaseContentZoomer : ContentZoomer
    {
        protected override void RegisterCallbacksOnTarget()
        {
            //base.RegisterCallbacksOnTarget();

            if (!(base.target is GraphView))
            {
                throw new InvalidOperationException("Manipulator can only be added to a GraphView");
            }

            base.target.RegisterCallback<WheelEvent>(OnWheel);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            //base.UnregisterCallbacksFromTarget();
            base.target.UnregisterCallback<WheelEvent>(OnWheel);
        }

        private static float CalculateNewZoom(float currentZoom, float wheelDelta, float zoomStep, float referenceZoom, float minZoom, float maxZoom)
        {
            if (minZoom <= 0f)
            {
                Debug.LogError($"The minimum zoom ({minZoom}) must be greater than zero.");
                return currentZoom;
            }

            if (referenceZoom < minZoom)
            {
                Debug.LogError($"The reference zoom ({referenceZoom}) must be greater than or equal to the minimum zoom ({minZoom}).");
                return currentZoom;
            }

            if (referenceZoom > maxZoom)
            {
                Debug.LogError($"The reference zoom ({referenceZoom}) must be less than or equal to the maximum zoom ({maxZoom}).");
                return currentZoom;
            }

            if (zoomStep < 0f)
            {
                Debug.LogError($"The zoom step ({zoomStep}) must be greater than or equal to zero.");
                return currentZoom;
            }

            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
            if (Mathf.Approximately(wheelDelta, 0f))
            {
                return currentZoom;
            }

            double num = Math.Log(referenceZoom, 1f + zoomStep);
            double num2 = (double)referenceZoom - Math.Pow(1f + zoomStep, num);
            double num3 = Math.Log((double)minZoom - num2, 1f + zoomStep) - num;
            double num4 = Math.Log((double)maxZoom - num2, 1f + zoomStep) - num;
            double num5 = Math.Log((double)currentZoom - num2, 1f + zoomStep) - num;
            wheelDelta = Math.Sign(wheelDelta);
            num5 += (double)wheelDelta;
            if (num5 > num4 - 0.5)
            {
                return maxZoom;
            }

            if (num5 < num3 + 0.5)
            {
                return minZoom;
            }

            num5 = Math.Round(num5);
            return (float)(Math.Pow(1f + zoomStep, num5 + num) + num2);
        }

        private void OnWheel(WheelEvent evt)
        {
            if (base.target is GraphView graphView)
            {
                IPanel panel = (evt.target as VisualElement)?.panel;
                // 屏蔽限制，允许其他操作期间也可以滚动
                //if (panel.GetCapturingElement(PointerId.mousePointerId) == null)
                {
                    Vector3 position = graphView.viewTransform.position;
                    Vector3 scale = graphView.viewTransform.scale;
                    Vector2 vector = base.target.ChangeCoordinatesTo(graphView.contentViewContainer, evt.localMousePosition);
                    float x = vector.x + graphView.contentViewContainer.layout.x;
                    float y = vector.y + graphView.contentViewContainer.layout.y;
                    position += Vector3.Scale(new Vector3(x, y, 0f), scale);
                    scale.y = (scale.x = CalculateNewZoom(scale.y, 0f - evt.delta.y, scaleStep, referenceScale, minScale, maxScale));
                    scale.z = 1f;
                    position -= Vector3.Scale(new Vector3(x, y, 0f), scale);
                    position.x = /*GUIUtility.RoundToPixelGrid*/RoundToPixelGrid(position.x);
                    position.y = /*GUIUtility.RoundToPixelGrid*/RoundToPixelGrid(position.y);
                    graphView.UpdateViewTransform(position, scale);
                    evt.StopPropagation();
                }
            }
        }
        private float RoundToPixelGrid(float v)
        {
            float pixelsPerPoint = (float)Screen.currentResolution.width / Screen.width;
            return Mathf.Floor(v * pixelsPerPoint + 0.48f) / pixelsPerPoint;
        }
    }
}