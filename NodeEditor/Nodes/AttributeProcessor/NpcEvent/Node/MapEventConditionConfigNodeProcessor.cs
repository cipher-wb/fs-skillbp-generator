using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class MapEventConditionConfigNodeProcessor : NodeEditorBaseProcessor<MapEventConditionConfigNode>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if(member.DeclaringType == typeof(ConfigBaseNode<MapEventConditionConfig>))
            {
                attributes?.ForEach(attr =>
                {
                    if(attr is FoldoutGroupAttribute foldoutGroupAttribute)
                    {
                        foldoutGroupAttribute.Expanded = false;
                        foldoutGroupAttribute.Order = -1;
                    }
                });
            }

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}
