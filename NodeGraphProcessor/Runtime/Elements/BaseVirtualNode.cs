using UnityEngine;
using System;

namespace GraphProcessor
{
	[Serializable]
    [Sirenix.OdinInspector.HideReferenceObjectPicker]
    public abstract class BaseVirtualNode
    {
        /// <summary>
        /// 端口类型
        /// </summary>
        public abstract Type PortType { get; }

        /// <summary>
        /// 模板路径
        /// </summary>
        public abstract string JsonAssetPath { get; }
    }
}
