using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal class BattleCameraShakeConfigProcessor : NodeEditorBaseProcessor<BattleCameraShakeConfig>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                switch (member.Name)
                {
                    case nameof(config.ID):
                        break;
                    case nameof(config.Name):
                        break;
                    case nameof(config.Frequency_X):
                        attributes.Add(new ShowIfAttribute("EnableX"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("X轴",false, order: 9));
                        break;
                    case nameof(config.Range_X):
                        attributes.Add(new ShowIfAttribute("EnableX"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("X轴",false, order: 9));
                        break;
                    case nameof(config.Samplings_X):
                        attributes.Add(new ShowIfAttribute("EnableX"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("X轴",false, order: 9));
                        break;
                    case nameof(config.Time_X):
                        attributes.Add(new ShowIfAttribute("EnableX"));
                        attributes.Add(new PropertyRangeAttribute(0, 100000f));
                        attributes.Add(new BoxGroupAttribute("X轴", false, order: 9));
                        attributes.Add(new SuffixLabelAttribute("毫秒"));
                        break;
                    case nameof(config.Curve_X):
                        attributes.Add(new HideIfAttribute("@true"));
                        break;
                    case nameof(config.Frequency_Y):
                        attributes.Add(new ShowIfAttribute("EnableY"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Y轴", false, order: 10));
                        break;
                    case nameof(config.Range_Y):
                        attributes.Add(new ShowIfAttribute("EnableY"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Y轴", false, order: 10));
                        break;
                    case nameof(config.Samplings_Y):
                        attributes.Add(new ShowIfAttribute("EnableY"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Y轴", false, order: 10));
                        break;
                    case nameof(config.Time_Y):
                        attributes.Add(new ShowIfAttribute("EnableY"));
                        attributes.Add(new PropertyRangeAttribute(0, 100000f));
                        attributes.Add(new BoxGroupAttribute("Y轴", false, order: 10));
                        attributes.Add(new SuffixLabelAttribute("毫秒"));
                        break;
                    case nameof(config.Curve_Y):
                        attributes.Add(new HideIfAttribute("@true"));
                        break;
                    case nameof(config.Frequency_Z):
                        attributes.Add(new ShowIfAttribute("EnableZ"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Z轴", false, order:11));
                        break;
                    case nameof(config.Range_Z):
                        attributes.Add(new ShowIfAttribute("EnableZ"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Z轴", false, order:11));
                        break;
                    case nameof(config.Samplings_Z):
                        attributes.Add(new ShowIfAttribute("EnableZ"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Z轴", false, order: 11));
                        break;
                    case nameof(config.Time_Z):
                        attributes.Add(new ShowIfAttribute("EnableZ"));
                        attributes.Add(new PropertyRangeAttribute(0, 100000f));
                        attributes.Add(new BoxGroupAttribute("Z轴", false, order: 11));
                        attributes.Add(new SuffixLabelAttribute("毫秒"));
                        break;
                    case nameof(config.Curve_Z):
                        attributes.Add(new HideIfAttribute("@true"));
                        break;
                    case nameof(config.Frequency_Yaw):
                        attributes.Add(new ShowIfAttribute("EnableYaw"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Yaw", false, order: 12));
                        break;
                    case nameof(config.Range_Yaw):
                        attributes.Add(new ShowIfAttribute("EnableYaw"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Yaw", false, order: 12));
                        break;
                    case nameof(config.Samplings_Yaw):
                        attributes.Add(new ShowIfAttribute("EnableYaw"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Yaw", false, order: 12));
                        break;
                    case nameof(config.Time_Yaw):
                        attributes.Add(new ShowIfAttribute("EnableYaw"));
                        attributes.Add(new PropertyRangeAttribute(0, 100000f));
                        attributes.Add(new BoxGroupAttribute("Yaw", false, order: 12));
                        attributes.Add(new SuffixLabelAttribute("毫秒"));
                        break;
                    case nameof(config.Curve_Yaw):
                        attributes.Add(new HideIfAttribute("@true"));
                        break;
                    case nameof(config.Frequency_Pitch):
                        attributes.Add(new ShowIfAttribute("EnablePitch"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Pitch", false, false, order: 13));
                        break;
                    case nameof(config.Range_Pitch):
                        attributes.Add(new ShowIfAttribute("EnablePitch"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Pitch", false, order: 13));
                        break;
                    case nameof(config.Samplings_Pitch):
                        attributes.Add(new ShowIfAttribute("EnablePitch"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Pitch", false, order: 13));
                        break;
                    case nameof(config.Time_Pitch):
                        attributes.Add(new ShowIfAttribute("EnablePitch"));
                        attributes.Add(new PropertyRangeAttribute(0, 100000f));
                        attributes.Add(new BoxGroupAttribute("Pitch", false, order: 13));
                        attributes.Add(new SuffixLabelAttribute("毫秒"));
                        break;
                    case nameof(config.Curve_Pitch):
                        attributes.Add(new HideIfAttribute("@true"));
                        break;
                    case nameof(config.Frequency_Roll):
                        attributes.Add(new ShowIfAttribute("EnableRoll"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Roll", false, order: 14));
                        break;
                    case nameof(config.Range_Roll):
                        attributes.Add(new ShowIfAttribute("EnableRoll"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Roll", false, order: 14));
                        break;
                    case nameof(config.Samplings_Roll):
                        attributes.Add(new ShowIfAttribute("EnableRoll"));
                        attributes.Add(new PropertyRangeAttribute(0, 100f));
                        attributes.Add(new BoxGroupAttribute("Roll", false, order: 14));
                        break;
                    case nameof(config.Time_Roll):
                        attributes.Add(new ShowIfAttribute("EnableRoll"));
                        attributes.Add(new PropertyRangeAttribute(0, 100000f));
                        attributes.Add(new BoxGroupAttribute("Roll", false, order: 14));
                        attributes.Add(new SuffixLabelAttribute("毫秒"));
                        break;
                    case nameof(config.Curve_Roll):
                        attributes.Add(new HideIfAttribute("@true"));
                        break;
                }
                var delayedProperty = attributes.Find((attr) => { return attr is DelayedPropertyAttribute; }) as DelayedPropertyAttribute;
                attributes.Remove(delayedProperty);
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}
