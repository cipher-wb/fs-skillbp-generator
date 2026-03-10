using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;
using GraphProcessor;

namespace NodeEditor
{
    [Serializable, HideReferenceObjectPicker]
    public class ConfigStackNode : BaseStackNode
    {
        public ConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }

        public Type ConfigType { get; protected set; }
    }

    [Serializable]
    public partial class ConfigStackNode<T> : ConfigStackNode where T : class, new()
    {
        public ConfigStackNode(Vector2 position = default, string title = "") : base(position, title)
        {

        }

        public override void Enable()
        {
            ConfigType = typeof(T);
        }
    }
}