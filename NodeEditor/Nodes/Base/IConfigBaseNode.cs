using System.Text;

namespace NodeEditor
{
    public interface IConfigBaseNode
    {
        int GenerateID(bool reset);     // 分配ID reset = true重新根据graph命名分配ID， false 根据IP自增
        string GetConfigName();         // 获取表格名：SkillConfig
        int GetConfigID();              // 获取表格ID：Config.ID
        int GetID();                    // 获取ID：获取缓存的inputNode ID
        object GetConfig();             // 获取表格对象
        string GetConfigJson();         // 获取表格Json数据
        string GetTableTash();          // 获取表格版本
        bool OnPostProcessing();        // 后处理
        string GetNodeSearchName();    // 节点查找名字
        bool OnSaveCheck();             // 节点检查

        // TODO 抽出模板接口
        //bool IsTemplate { get; set; }   // 是否模板
    }
}
