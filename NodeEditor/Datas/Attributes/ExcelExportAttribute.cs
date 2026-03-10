using System;

namespace NodeEditor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExcelExportAttribute : Attribute
    {
        public bool IsExport { get; private set; } = true;
        public bool IsOnlyEditorMember { get; private set; } = false;
        public ExcelExportAttribute(bool isExport, bool isOnlyEditorMember)
        {
            IsExport = isExport;
            IsOnlyEditorMember = isOnlyEditorMember;
        }
    }
}
