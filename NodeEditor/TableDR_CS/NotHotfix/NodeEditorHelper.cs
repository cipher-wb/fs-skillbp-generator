using System;

namespace TableDR
{
    public static partial class NodeEditorHelper
    {
        private static Action<string> onTableValueChange;
        public static event Action<string> OnTableValueChange
        {
            add
            {
                onTableValueChange -= value;
                onTableValueChange += value;
            }
            remove { onTableValueChange -= value; }
        }
        public static void OnValueChange(string name)
        {
            onTableValueChange?.Invoke(nameof(TParamType));
        }
    }
}
