#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using Funny.Base.Utils;
using UnityEngine;

namespace TableDR
{
    public partial class BattleCustomParamConfig_ParamInfo
    {

        private void OnBeginListElement_OptionParams(int index)
        {
            SirenixEditorGUI.BeginBox(GetOptionParamsName(index));
        }

        private void OnEndListElement_OptionParams(int index)
        {
            SirenixEditorGUI.EndBox();
        }

        private string GetOptionParamsName(int index)
        {
            switch (OptionType)
            {
                case BattleCustomParamConfig_TParamOptionType.TPOT_QTE:
                    return GetQTEParamName(index);
            }
            return "参数键值对集合";
        }

        private void OnValueChange_ParamOptionTypeType()
        {
            if(ParamList == null)
            {
                ParamList = new System.Collections.Generic.List<BattleCustomParamConfig_ParamVK>();
                ParamList.GetListRef().Add(new BattleCustomParamConfig_ParamVK());
            }
            switch (OptionType)
            {
                case BattleCustomParamConfig_TParamOptionType.TPOT_GAMEOBJECT_VISIBLE: // GameObject可见性设置
                    {

                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_UIWINDOW_VISIBLE: // UI窗口可见性设置
                    {

                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_SET_LAYER: // 设置Layer
                    {

                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_CHILD_VISIBLE: // 设置子对象可见性
                    {

                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_PLAYVIDEO: // 播放视频
                    {
                        ((List<BattleCustomParamConfig_ParamVK>)ParamList).Clear();
                        for (int i = 0; i < 3; ++i)
                        {
                            var paramVK = new BattleCustomParamConfig_ParamVK();
                            paramVK.SetParamName(GetPlayVideoParamName(i));
                            ((List<BattleCustomParamConfig_ParamVK>)ParamList).Add(paramVK);
                        }
                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_SHOWSKILLSLOTEFFECT: // 显示技能槽特效
                    {

                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_QTE: // 快速反应事件
                    {
                        ((List<BattleCustomParamConfig_ParamVK>)ParamList).Clear();
                        for (int i = 0; i < 8; ++i)
                        {
                            var paramVK = new BattleCustomParamConfig_ParamVK();
                            paramVK.SetParamName(GetQTEParamName(i));
                            ((List<BattleCustomParamConfig_ParamVK>)ParamList).Add(paramVK);
                        }
                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_MARQUEE: // 跑马灯提示
                    {
                        ((List<BattleCustomParamConfig_ParamVK>)ParamList).Clear();
                        for (int i = 0; i < 3; ++i)
                        {
                            var paramVK = new BattleCustomParamConfig_ParamVK();
                            paramVK.SetParamName(GetMarqueeParamName(i));
                            ((List<BattleCustomParamConfig_ParamVK>)ParamList).Add(paramVK);
                        }
                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_MINGDAN:
                    {
                        ((List<BattleCustomParamConfig_ParamVK>)ParamList).Clear();
                        for (int i = 0; i < 3; ++i)
                        {
                            var paramVK = new BattleCustomParamConfig_ParamVK();
                            paramVK.SetParamName(GetNingDan(i));
                            ((List<BattleCustomParamConfig_ParamVK>)ParamList).Add(paramVK);
                        }
                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_ENV_STAGE_CTR:
                    {
                        ((List<BattleCustomParamConfig_ParamVK>)ParamList).Clear();
                        for (int i = 0; i < 1; ++i)
                        {
                            var paramVK = new BattleCustomParamConfig_ParamVK();
                            paramVK.SetParamName(GetEvoStage(i));
                            ((List<BattleCustomParamConfig_ParamVK>)ParamList).Add(paramVK);
                        }
                    }
                    break;
                case BattleCustomParamConfig_TParamOptionType.TPOT_POSTPROCESS:
                    {
                        ((List<BattleCustomParamConfig_ParamVK>)ParamList).Clear();
                        for (int i = 0; i < 2; ++i)
                        {
                            var paramVK = new BattleCustomParamConfig_ParamVK();
                            paramVK.SetParamName(GetPostProcess(i));
                            ((List<BattleCustomParamConfig_ParamVK>)ParamList).Add(paramVK);
                        }
                    }
                    break;
            }
            NodeEditorHelper.OnValueChange(nameof(BattleCustomParamConfig_TParamOptionType));
        }

        private string GetPlayVideoParamName(int index)
        {
            switch (index)
            {
                case 0:
                    return "路径(StreamingAssets/Video/*)";
                case 1:
                    return "播完后,延迟关闭时间[毫秒]";
                case 2:
                    return "是否暂停逻辑[0/1]";
                default:
                    return "无效参数";
            }
        }
        private string GetQTEParamName(int index)
        {
            switch (index)
            {
                case 0:
                    return "点击次数";
                case 1:
                    return "QTE时间(帧数)";
                case 2:
                    return "出现位置_X";
                case 3:
                    return "出现位置_Y";
                case 4:
                    return "图标样式";
                case 5:
                    return "提示文字";
                case 6:
                    return "成功后执行";
                case 7:
                    return "失败后执行";
                default:
                    return "无效参数";
            }
        }
        private string GetMarqueeParamName(int index)
        {
            switch (index)
            {
                case 0:
                    return "文本配置（TextConfig）";
                case 1:
                    return "持续时间（毫秒）";
                case 2:
                    return "文本类型（0小标题|1大标题）";
                default:
                    return "无效参数";
            }
        }

        private string GetNingDan(int index)
        {
            switch (index)
            {
                case 0:
                    return "参数一";
                case 1:
                    return "参数二";
                case 2:
                    return "枚举值";
                default:
                    return "无效参数";
            }

        }

        private string GetEvoStage(int index)
        {
            switch (index)
            {
                case 0:
                    return "阶段值";
                default:
                    return "无效参数";
            }

        }

        private string GetPostProcess(int index)
        {
            switch (index)
            {
                case 0:
                    return "需要开启的后处理类型";
                default:
                    return "是否开启（0关闭/1开启）";
            }

        }
    }

    public partial class BattleCustomParamConfig_ParamVK
    {
        public void SetParamName(string name)
        {
            ParamName = name;
        }
    }
}
#endif