#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;

namespace TableDR
{
    public partial class BehaviorConfig
    {
        public static Sirenix.OdinInspector.ValueDropdownList<string> VD_BehaviorType = new Sirenix.OdinInspector.ValueDropdownList<string>()
            {
                {"-", ""},
                {"对话-ShowDialog", "ShowDialog"},
                {"次级对话-NewSecondaryDialogBehavior", "NewSecondaryDialogBehavior"},
                {"黑屏转场-ShowDialogMask", "ShowDialogMask"},
            };

        private bool IsShowProperty(InspectorProperty property)
        {
            switch (property.Name)
            {
                case nameof(BaseID):
                case nameof(Type):
                    return true;
            }
            var propertyDesc = GetPropertyDesc(property.Name);
            return !string.IsNullOrEmpty(propertyDesc);
        }

        private string GetPropertyDesc(string propertyName)
        {
            switch (Type)
            {
                case "ShowDialog":
                    {
                        switch (propertyName)
                        {
                            case nameof(IntArg1): return "说话者ID";
                            case nameof(IntArg3): return "说话者位置";
                            case nameof(IntArg5): return "旁听者ID";
                            case nameof(Loc1Editor): return "对话内容";
                        }
                        break;
                    }
                case "NewSecondaryDialogBehavior":
                    {
                        switch (propertyName)
                        {
                            case nameof(IntArgs): return "说话者ID";
                            case nameof(IntArg4): return "持续时间-毫秒";
                            case nameof(BoolArg1): return "是否战斗里也显示";
                            case nameof(Loc1Editor): return "对话内容";
                        }
                        break;
                    }
                case "ShowDialogMask":
                    {
                        switch (propertyName)
                        {
                            case nameof(IntArg1): return "进入进出1-0";
                            case nameof(IntArg2): return "持续时间-毫秒";
                            case nameof(BoolArg1): return "是否直接结束";
                            case nameof(StrArg1): return "图片路径";
                            case nameof(Loc1Editor): return "居中文字";
                        }
                        break;
                    }
            }
            return string.Empty;
        }
    }
}


#endif