using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace NodeEditor
{
    [HideReferenceObjectPicker]
    [BoxGroup]
    public class NP_BlackBoard
    {
        public Dictionary<string, string> TestEvent = new Dictionary<string, string>();

        public Dictionary<long, long> TestId = new Dictionary<long, long>();
    }
}