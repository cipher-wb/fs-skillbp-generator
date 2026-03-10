using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NodeEditor
{
    public class Validation
    {
        public class ValidationFieldPair
        {
            public string SrcField;

            public string DesField;
        }

        public string Table;

        public List<ValidationFieldPair> ValidationFields;
    }
}
