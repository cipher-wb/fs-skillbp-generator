using System;

namespace NodeEditor
{
    public sealed class RefPortAnnotation
    {
        public string displayName;
        public Type displayType;
        public string identifier;
        public bool acceptMultipleEdges;
#if UNITY_EDITOR
        public UnityEngine.Color portColor;
#endif
    }
}
