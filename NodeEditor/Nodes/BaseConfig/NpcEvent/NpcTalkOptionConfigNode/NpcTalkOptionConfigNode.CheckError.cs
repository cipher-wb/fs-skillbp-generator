using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcTalkOptionConfigNode定义类
    /// </summary>
    public partial class NpcTalkOptionConfigNode
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        private bool IsExitInspectorError => !string.IsNullOrEmpty(InspectorError);

        public string InspectorError { get; set; }

        public override bool OnSaveCheck()
        {
            CheckError();

            if (IsExitInspectorError)
            {
                AppendSaveMapEventRet(InspectorError);
                return false;
            }

            return base.OnSaveCheck();
        }

        /// <summary>
        /// 检查错误
        /// </summary>
        public void CheckError()
        {
            TalkOptionData?.CheckError();
        }
    }
}
