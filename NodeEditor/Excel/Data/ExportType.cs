using System.Collections.Generic;
using System.ComponentModel;

namespace NodeEditor
{
    public class ExportType
    {
        public string Name;

        public bool Exportable;

        public Dictionary<string, bool> Members;
    }
}
