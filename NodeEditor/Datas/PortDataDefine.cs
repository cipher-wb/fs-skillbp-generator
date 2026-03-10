using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace NodeEditor.SkillEditor
{
    // 暂时未用到
    [Serializable, Obsolete]
    public class PortDataDefine
    {
        [LabelText("标识，变量名")]
        public string identifier;

        [LabelText("端口名称")]
        public string displayName;

        [LabelText("端口类型限制")]
        public string displayType;

        [LabelText("是否列表")]
        public bool acceptMultipleEdges;
    }
}
