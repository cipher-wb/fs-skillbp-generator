#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;

namespace TableDR
{
    public partial class ElementsProperty
    {
        public ElementsProperty(TElementsType elementsType, int level)
        {
            Element = elementsType;
            Level = level;
        }

        public ElementsProperty Clone()
        {
            return new ElementsProperty(Element, Level);
        }

    }
}
#endif
