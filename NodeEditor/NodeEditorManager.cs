using System.Text;

namespace NodeEditor
{
    /// <summary>
    /// 统一管理节点编辑器（SKillEditor||AIEditor...）
    /// </summary>
    public sealed partial class NodeEditorManager : Singleton<NodeEditorManager>
    {
        public static bool IsInEditor
        {
            get
            {
                return SkillEditor.SkillEditorManager.IsInEditor
                    || AIEditor.AIEditorManager.IsInEditor
                    || GamePlayEditor.GamePlayEditorManager.IsInEditor
                    || NpcEventEditor.NpcEventEditorManager.IsInEditor
                    || MapAnimEditor.MapAnimEditorManager.IsInEditor;
            }
        }

        /// <summary>
        /// 检查编辑器准备工作是否正常
        /// </summary>
        public bool IsValid() { return IsValid(out _); }

        /// <summary>
        /// 检查编辑器准备工作是否正常
        /// </summary>
        public static bool IsValid(out string errorMessage)
        {
            errorMessage = null;
            // 调用各个模块，保证编辑器正常
            try
            {
                // 加载表格
                DesignTable.Load();
                // 表格信息
                _ = ExcelManager.Inst;
                // 描述信息
                _ = TableAnnotation.Inst;
                // 人员信息
                _ = TemplateManager.Inst;
                // ID信息
                _ = ConfigIDManager.Inst;
                return true;
            }
            catch (System.Exception ex)
            {
                errorMessage = $"检查表格初始化异常，请检查\n{ex}";
                Log.Fatal(errorMessage);
            }
            return false;
        }
    }
    // TODO ---------------备忘列表-------------------------------------------------------------
    // 自动导出C++代码，包括注释等
    // 节点生命周期整理
    // 文件监听整理（json改动刷新graph窗口等）
    // 模板节点端口连线报错检测，或给予不可拖拽节点
    // TParam CustomValueDrawer?
    // 【已支持】节点颜色配置
    // 编辑器关闭，窗口恢复
    // 【已支持】窗口添加svn提交按钮
    // 未导出的表格节点类型，去除暴露port，如子弹节点：音效端口
    // 模板graph修改，保存同步到所有引用文件
    // 创建特效延迟帧数
    // 批量导出graph
    // 筛选节点，单位类型容易漏填（处理方式待考虑：默认敌军？不配置默认全部类型？节点显示类型？保存提示？）
    // 子弹节点ModelConfig检测
    // 条件判断整合：阵营判断，属性判断，状态判断，SkillTag->技能参数，
    // 枚举flag优化，类型改为Flag位组合，如单位类型
    // 支持Config所有property端口是否导出配置及模块group等配置
    // 所有Graph整合类型获取，便于批处理操作
    // 节点配置，删除节点，对应自动生成代码清理
    // 模板节点方法整合
    // 借鉴shadergraph，subgraph，ToSubGraph
    // 正则匹配ConfigJson直接导表，不加载graph
    // 。。。
}
