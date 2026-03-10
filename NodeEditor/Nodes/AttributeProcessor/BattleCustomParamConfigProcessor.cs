using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;


namespace NodeEditor
{
    internal class BattleCustomParamConfigProcessor : NodeEditorBaseProcessor<BattleCustomParamConfig>
    {

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                switch (member.Name)
                {
                    case nameof(config.ExtraParams):
                        {
                            //attributes.Add(new ListDrawerSettingsAttribute
                            //{
                            //    HideAddButton = true,
                            //    HideRemoveButton = true,
                            //    ShowFoldout = true,
                            //    DraggableItems = false,
                            //    NumberOfItemsPerPage = 50,
                            //});

                            //if (!string.IsNullOrEmpty(anno.tips))
                            //{
                            //    var labelText = attributes.Find((attr) => { return attr is LabelTextAttribute; }) as LabelTextAttribute;
                            //    {
                            //        labelText.Text = anno.tips;
                            //    }
                            //}
                            break;
                        }
                    default:
                        break;
                }
            }

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

    }
}
