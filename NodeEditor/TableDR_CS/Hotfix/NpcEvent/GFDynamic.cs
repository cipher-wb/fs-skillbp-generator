#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;

namespace TableDR
{
    public partial class GFDynamic
    {
        public GFDynamic()
        {

        }

        public GFDynamic(int dynmaicInt1)
        {
            DynmaicInt1 = dynmaicInt1;
        }

        public GFDynamic(int dynmaicInt1, int dynmaicInt2)
        {
            DynmaicInt1 = dynmaicInt1;
            DynmaicInt2 = dynmaicInt2;
        }

        public GFDynamic(int dynmaicInt1, int dynmaicInt2, int dynmaicInt3)
        {
            DynmaicInt1 = dynmaicInt1;
            DynmaicInt2 = dynmaicInt2;
            DynmaicInt3 = dynmaicInt3;
        }

        public GFDynamic(int dynmaicInt1, int dynmaicInt2, int dynmaicInt3, int dynmaicInt4)
        {
            DynmaicInt1 = dynmaicInt1;
            DynmaicInt2 = dynmaicInt2;
            DynmaicInt3 = dynmaicInt3;
            DynmaicInt4 = dynmaicInt4;
        }

        public GFDynamic Clone1To2()
        {
            return new GFDynamic(DynmaicInt1, DynmaicInt2);
        }
    }
}
#endif
