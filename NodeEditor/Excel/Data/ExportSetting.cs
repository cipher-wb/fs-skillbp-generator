using System.Collections.Generic;

namespace NodeEditor
{
    public class ExportSetting
    {
        public string Name;

        public bool DefaultExport;

        public bool DefaultExportTable ;

        public bool DefaultExportSubClass;

        public bool ExportCode;

        public Dictionary<string, ExportType> TypeInfos;

        public Dictionary<string, List<string>> ExcludeSheets;

    }
}
