using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public interface INodeCustomInspector
    {
        /// <summary>
        /// 检查错误
        /// </summary>
        void CheckError();

        /// <summary>
        /// Type改变调用
        /// </summary>
        void SetDefault();

        /// <summary>
        /// 配置表恢复到检视面板状态
        /// </summary>
        void ConfigToData();
    }
}
