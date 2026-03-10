#if UNITY_EDITOR
using System.Collections.Generic;
using Funny.Base.Utils;
using Sirenix.Utilities.Editor;

namespace TableDR
{
    public partial class BulletConfig
    {
        private TParam CustomAddFunction_TracePathParams()
        {
            return new TParam();
        }

        private void OnBeginListElement_TracePathParams(int index)
        {
            SirenixEditorGUI.BeginBox(GetTracePathParamName(index));
        }

        private void OnEndListElement_TracePathParams(int index)
        {
            SirenixEditorGUI.EndBox();
        }

        private string GetTracePathParamName(int index)
        {
            switch (TracePathType)
            {
                case TTracePathType.TTPT_BEZIER:
                    {
                        switch (index)
                        {
                            case 0:
                                return "移动时间(帧数)";
                            case 1:
                                return "结束后处理方式：0销毁子弹 1继续前进 2停留";
                            case 2:
                                return "结束点 - X";
                            case 3:
                                return "结束点 - Y";
                            case 4:
                                return "控制点 1 - X";
                            case 5:
                                return "控制点 1 - Y";
                            case 6:
                                return "控制点 2 - X";
                            case 7:
                                return "控制点 2 - Y";
                        }
                    }
                    break;
                case TTracePathType.TTPT_LINE:
                    {
                        switch (index)
                        {
                            case 0:
                                return "移动时间(帧数)";
                            case 1:
                                return "结束后处理方式：0销毁子弹 1继续前进 2停留";
                            case 2:
                                return "结束点 - X";
                            case 3:
                                return "结束点 - Y";
                        }
                    }
                    break;
                case TTracePathType.TTPT_STEADY:
                    {
                        switch (index)
                        {
                            case 0:
                                return "碰撞开启时间(帧数)";
                            case 1:
                                return "间隔失效时间(帧数)";
                            case 2:
                                return "间隔起效时间(帧数)";
                            case 3:
                                return "是否依附移动";
                        }
                    }
                    break;
                case TTracePathType.TTPT_TURNBACK:
                    {
                        switch (index)
                        {
                            case 0:
                                return "出发阶段时间(帧数)";
                            case 1:
                                return "停留阶段时间(帧数)";
                            case 2:
                                return "出发阶段初速度";
                            case 3:
                                return "折返阶段初速度";
                            case 4:
                                return "折返阶段加速度";
                            case 5:
                                return "命中向前时间(帧数)";
                            case 6:
                                return "停留阶段是否朝目标移动";
                            case 7:
                                return "停留阶段朝目标移动的速度";
                            case 8:
                                return "停留阶段朝目标移动的转向速度";
                            case 9:
                                return "是否禁止更新面向";
                            case 10:
                                return "是否返回目标点位置";
                        }
                    }
                    break;
                case TTracePathType.TTPT_BOUNCE:
                    {
                        switch (index)
                        {
                            case 0:
                                return "弹射次数";
                            case 1:
                                return "弹射目标筛选 0包括自己的友方 1友方 2敌方 3不分阵营";
                            case 2:
                                return "弹射范围，搜索目标半径";
                            case 3:
                                return "弹射规则：0圆内随机，1最近单位，2怪物类型优先级(妖王>妖首>灵妖>异兽>强兽>凡兽)";
                            case 4:
                                return "是否弹射相同目标：1是，0否";
                            case 5:
                                return "弹射抛物线高度";
                            case 6:
                                return "弹射时长(帧数，0:子弹速度不变)";
                            case 7:
                                return "是否同阵营绕身: 1是，0否";
                            case 8:
                                return "特殊可被弹射子弹ID";
                        }
                    }
                    break;
                case TTracePathType.TTPT_PARABOLA:
                    {
                        switch (index)
                        {
                            case 0:
                                return "移动时间(帧数)";
                            case 1:
                                return "结束点 - X";
                            case 2:
                                return "结束点 - Y";
                            case 3:
                                return "抛物线高度";
                        }
                    }
                    break;
            }
            return "无效参数";
        }

        private void OnValueChange_TracePathType()
        {
            TracePathParams.GetListRef().Clear();
            int count = 0;
            switch (TracePathType)
            {
                case TTracePathType.TTPT_BEZIER:
                case TTracePathType.TTPT_TURNBACK:
                    count = 11;
                    break;
                case TTracePathType.TTPT_LINE:
                case TTracePathType.TTPT_STEADY:
                case TTracePathType.TTPT_PARABOLA:
                    count = 4;
                    break;
                case TTracePathType.TTPT_BOUNCE:
                    count = 9;
                    break;
            }
            for(int i = 0; i < count; ++i)
            {
                TracePathParams.GetListRef().Add(new TParam());
            }
            NodeEditorHelper.OnValueChange(nameof(TTracePathType));
        }

        private void OnValueChange_FlyType()
        {
            NodeEditorHelper.OnValueChange(nameof(TBulletFlyType));
        }
        private bool HideIf_FlyType_TracePath()
        {
            return FlyType != TBulletFlyType.TBFT_TRACE_PATH;
        }
    }
}
#endif