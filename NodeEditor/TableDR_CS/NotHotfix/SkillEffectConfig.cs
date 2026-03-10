#if UNITY_EDITOR

using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Funny.Base.Utils;

namespace TableDR
{
    public partial class SkillEffectConfig
    {
        private Action paramsChanged;
        public event Action OnParamsChanged
        {
            add
            {
                paramsChanged -= value;
                paramsChanged += value;
            }
            remove { paramsChanged -= value; }
        }

        private void CustomAddFunction_Params_TSET_NUM_CALCULATE()
        {
            Params.GetListRef().Add(new TParam());
            this.paramsChanged?.Invoke();
        }
        private void CustomRemoveIndexFunction_Params_TSET_NUM_CALCULATE(int index)
        {
            if (Params.Count <= 1)
            {
                Debug.LogError("节点-数值运算, 需保留最少1个参数!");
                return;
            }
            Params.GetListRef().RemoveAt(index);
            this.paramsChanged?.Invoke();
        }
        private void OnTitleBarGUI_Params_TSET_NUM_CALCULATE()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                // 添加两个
                if (this.Params?.Count >= 3)
                {
                    int changeNum = 2;
                    while (changeNum-- > 0)
                    {
                        //this.Params.Add(new TParam());
                        Sirenix.Utilities.ListExtensions.SetLength(Params.GetListRef(), this.Params.Count + 1, () => new TParam());
                    }
                }
            }
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.X))
            {
                // 删除2个
                int changeNum = 2;
                while (changeNum-- > 0)
                {
                    if (this.Params?.Count > 3)
                    {
                        Sirenix.Utilities.ListExtensions.SetLength(Params.GetListRef(), this.Params.Count - 1);
                    }
                }
            }
        }

        private void CustomRemoveIndexFunction_Params_TSET_SWITCH_EXECUTE(int index)
        {
            if (index <= 3)
            {
                Debug.LogError("节点-Switch分支执行, 前三个不允许删除!");
                return;
            }

            if(index%2 == 0 || index!= (this.Params.Count - 1))
            {
                Debug.LogError("节点-Switch分支执行, 只能从下往上逐个删除!");
                return;
            }

            Params.GetListRef().RemoveAt(index);
            Params.GetListRef().RemoveAt(index - 1);
            this.paramsChanged?.Invoke();
        }

        private void CustomAddFunction_Params_TSET_SWITCH_EXECUTE()
        {
            Params.GetListRef().Add(new TParam());
            Params.GetListRef().Add(new TParam());
            this.paramsChanged?.Invoke();
        }

        private string GetSkillEventParamNames()
        {
            if (this.Params.Count <= 0)
            {
                return "错误参数个数配置";
            }
            int eventId = this.Params[0].Value;
            var eventConfig = SkillEventConfigManager.Instance.GetItem(eventId);
            if (eventConfig != null)
            {
                return $"{eventConfig.ParamNames}{eventConfig.SubTypeNames}";
            }
            return "请配置正确的技能消息ID...";
        }

        private bool CheckSkillEventParamWarning()
        {
            if (this.Params.Count < 11)
            {
                return false;
            }
            int eventId = this.Params[0].Value;
            var eventConfig = SkillEventConfigManager.Instance.GetItem(eventId);
            if (eventConfig == null)
            {
                return false;
            }

            if (eventConfig.EventSubType.Count == 0)
            {
                return false;
            }

            int subType = this.Params[9].Value;
            int subTypeValue = this.Params[10].Value;

            if (subType == 0 || subTypeValue == 0)
            {
                if (subType == 0)
                {
                    this.Params[9].RefreshDisplay(this.Params[9].GetDisplayName(), $"注意：为了性能,建议填写事件子类型{eventConfig.SubTypeNames}");
                }

                if (subTypeValue == 0)
                {
                    this.Params[10].RefreshDisplay(this.Params[10].GetDisplayName(), "注意：为了性能,请填写事件子类型的值");
                }

                return true;
            }

            return false;
        }

        private bool CheckSkillEventParamError()
        {
            if (this.Params.Count < 11)
            {
                return true;
            }
            int eventId = this.Params[0].Value;
            var eventConfig = SkillEventConfigManager.Instance.GetItem(eventId);
            if (eventConfig == null)
            {
                return false;
            }

            int subType = this.Params[9].Value;
            int subTypeValue = this.Params[10].Value;
            if (eventConfig.EventSubType.Count == 0)
            {
                if (subType != 0 || subTypeValue != 0)
                {
                    this.Params[9].SetValue(0);
                    this.Params[10].SetValue(0);
                }
                return false;
            }

            if (subType != 0)
            {
                if (!eventConfig.EventSubType.Contains((TSkillEventSubType)subType))
                {
                    this.Params[9].RefreshDisplay(this.Params[9].GetDisplayName(), $"注意：当前事件没有该子类型{eventConfig.SubTypeNames}");
                    return true;
                }

                if (subTypeValue == 0)
                {
                    this.Params[10].RefreshDisplay(this.Params[10].GetDisplayName(), "注意：请填写事件子类型的值");
                    return true;
                }
            }

            return false;
        }

        private bool MoveToTargetPosCheckError()
        {
            if (this.Params.Count < 3)
            {
                return false;
            }

            if (this.Params[1].ParamType == TParamType.TPT_NULL)
            {
                this.Params[1].RefreshDisplay(this.Params[1].GetDisplayName(), "若使用绝对坐标请注意ai的通用性!");
                return true;
            }

            if (this.Params[2].ParamType == TParamType.TPT_NULL)
            {
                this.Params[2].RefreshDisplay(this.Params[2].GetDisplayName(), "若使用绝对坐标请注意ai的通用性!");
                return true;
            }

            return false;
        }
    }
}

#endif
