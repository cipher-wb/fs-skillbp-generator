using System.Collections.Generic;
using System.Text;

namespace NodeEditor
{
    public interface ISetting
    {
        bool SaveSetting(StringBuilder saveInfo);

        [Newtonsoft.Json.JsonIgnore]
        string PathSetting { get; }

        [Newtonsoft.Json.JsonIgnore]
        List<string> PathCommitSetting { get; }
    }
}
