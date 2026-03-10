# 封神技能编辑器 - 技能配置模块深度分析报告

基于 NodeEditor 框架，技能配置编辑器（SkillEditor）深度融合了 Unity 的 NodeGraphProcessor（用于流程式视图）和 Sirenix Odin Inspector（用于属性检视面板），形成了一套高自动化、数据驱动的工业化生产工具。

以下是针对 `SkillEditor` 模块的 10 个维度的详细技术分析：

---

## 1. UI组件分析 (UI Components)
技能编辑器的 UI 由两大部分构成：**节点拓扑图 (GraphView)** 与 **检视面板 (Inspector)**。
*   **图谱视图 (基于 UIElements)**：
    *   `SkillGraphWindow`：编辑器主窗口，包含顶部工具栏 (`ConfigGraphToolbarView`)、主编辑区 (`SkillGraphView`) 和右下角小地图 (`MiniMap`)。
    *   `NodeView`：代表单个配置表的节点，具有 Input/Output 端口，支持连线拖拽。
*   **检视面板 (基于 Odin Inspector)**：
    *   **折叠组 (FoldoutGroup)**：如 `[FoldoutGroup("基础信息")]`、`[FoldoutGroup("技能数据")]`，用于将海量的技能属性分类折叠，避免视觉疲劳。
    *   **下拉菜单 (ValueDropdown)**：如 `Attribute_CustomValueDrawerAttribute_SkillSubType`，根据主类型动态生成子类型的下拉列表。
    *   **动态列表 (ListDrawerSettings)**：如 `Attribute_ComboCDList`，针对连招 CD 等数组结构，提供自定义的添加/删除按钮和折叠样式 (`HideAddButton = false, ShowFoldout = true`)。
    *   **自定义绘制器 (CustomValueDrawer)**：如指示器参数 `SkillIndicatorParam`，利用 `OnBeginListElementGUI` 将无意义的数组索引转换为语义化名称（如“拖动圈-外圆半径”）。
    *   **按钮 (Button)**：如 `[Button("点击-导出数据到Excel")]`，提供一键执行复杂逻辑的快捷入口。

## 2. 数据结构定义 (Data Structures)
技能数据的底层载体是自动生成的 C# 数据类，主要由配表结构映射而来：
*   **主体定义**：`SkillConfig` 类，包含数百个字段。例如基础属性（ID、Name、Desc）、时间控制（CdTime、SkillCastFrame、SkillBufferFrame、SkillBaseDuration）、类型枚举（CdType、SkillMainType、SkillSubType）。
*   **复合结构**：如 `SkillComboCD` 类，专门用于存储连招或多段技能的独立 CD 结构（包含 CDTime、CastFrame、BufferFrame、BaseDuration）。
*   **标签/参数列表**：大量使用 `List<int>` (如 `SkillIndicatorParam`) 和自定义类 `SkillTagInfo`（如 `SkillDamageTagsList`，`SkillTagsList`），用于挂载各种动态 Buff 或 特效 ID。
*   **存储格式**：在编辑器内存中以 C# Object 存在。保存时，经由 `Newtonsoft.Json` 序列化为巨大的一体化 JSON 字符串（挂载在 `ConfigBaseNode` 内部）。导出时，映射回 Excel 的行列结构。

## 3. 交互逻辑 (Interaction Logic)
用户交互与数据刷新的绑定极为紧密：
*   **属性修改触发**：策划在 Inspector 中修改某个下拉框时（如修改 `CdType`），Odin 触发 `[OnValueChanged("OnValueChange_CdType")]` 宏，后端代码会自动清理或初始化 `ComboCdList` 数据，实现了 UI 级联更新。
*   **指示器联动**：修改 `SkillIndicatorType` 时，会触发 `OnValueChange_TIndicatorType`，自动清空并依据新类型（如扇形、矩形、双圆）重新填充 `SkillIndicatorParam` 数组的默认长度和默认值。
*   **图谱连线**：拖拽节点连线本质上是**赋值引用**。将 Buff 节点的 Output 连入 技能节点的 Input，底层的 `OnAfterEdgeConnected` 事件会被触发，从而将 Buff 的 ID 写入技能数据的列表中，实现了“连线即配表”的交互。

## 4. 状态管理 (State Management)
*   **全局生命周期状态**：通过 `EditorFlag` 位枚举管理编辑器的运行状态（如 `Saving`、`GraphSaveFlags`、`DisplayChanged`）。
*   **防丢机制 (Backup)**：`UpdateBackup()` 方法每隔 60 秒（或强制标记变动时）检查 `graphChangeFlag`，异步写入备份 JSON，极大防止了编辑器崩溃导致的数据丢失。
*   **运行/调试状态**：包含对战斗核心的回调。状态变量 `IsPauseGame` 和 `DebugingNode` 能够让编辑器在 Unity 运行时直接打断点（F9/F10），并高亮当前执行的节点（`FocusSelect`）。
*   **窗口缓存**：`ConfigGraphWindow.CacheOpenedWindows` 全局静态 HashSet 维护所有打开的子窗口，确保跨窗口的模板或引用的同步刷新。

## 5. 验证机制 (Validation Mechanisms)
在图谱保存 (`OnSaveCheck`) 或导出前，拥有极其严苛的纠错验证防线：
*   **动作帧逻辑验证**：校验动作帧的时序合法性。要求 `前摇(CastFrame) <= 缓冲区(BufferFrame) <= 基础时长(BaseDuration) <= CD(CdTime)`。连招 CD 类型还会单独校验最后一段的 `BaseDuration` 必须为 0。
*   **关键字段非空验证**：校验 `SkillMainType` 和 `SkillSubType` 是否配置；如果需要 AI 施放，强制校验 `AISkillRange` 是否配置。
*   **逻辑互斥/强关联验证**：如果技能挂载了“解控”的 AI 标签 (`TBSAT_CONTROL_RELEASE`)，强制要求同时勾选技能特性“无视禁止施法”，否则拦截保存。
*   **跨表一致性验证**：加载 `SkillValueConfig`（多等级数值表），对比当前 1 级技能的 `SkillTagsList`（参数列表）是否与数值表严格对齐，防止程序运行时读取不到参数。

## 6. 事件绑定 (Event Binding)
*   **UI层级事件**：完全委托给 Odin 的 Attribute 系统。如 `[OnValueChanged]`、`[HideIf]` 等。
*   **文件系统级事件**：`JsonGraphManager` 监听整个文件夹的 `.json` 变动，自动映射图谱缓存；`ExcelManager` 具有文件占用判定，在导出前会轮询判断 Excel 文件是否被策划打开（`Utils.IsFileOpened`），并弹窗阻塞。
*   **图谱系统级事件**：向图谱挂载 `OnSaveGraphToDiskBefore` 和 `OnSaveGraphToDiskAfter` 委托。Before 阶段主要拦截（利用 `errorInfo` 收集错误并决定是否弹出中断 Dialog），After 阶段负责弹出 `ShowNotification` 悬浮窗提示。

## 7. API接口 (API & Communication)
*   **`SaveData()` / `ExportData()`**：数据固化出口。调用 `ExcelManager.Inst.WriteExcel()` 将内存对象转存为实体 `.csv` 或 `.xlsx`。
*   **SVN 协同接口**：集成 `DevLocker.VersionControl.WiseSVN`，通过 `SaveAndSVNCommit()` 方法实现一键保存并调起 SVN 提交弹窗。
*   **`SyncConfigData()`**：全局同步接口。当修改了被多处引用的“模板节点”时，遍历所有打开的 GraphWindow 并调用该接口，强制内存数据更新到 `DesignTable` 单例中，供给游戏 Runtime 实时读取（HotReload）。
*   **战斗调试接口**：与 `AppFacade.BattleManager` 深度耦合，调用 `BattleWrapper.UnityEditorDebug_RunUntilNextConfig()` 和 `BattleWrapper.UnityEditorDebug_AddDebugConfigId()` 实现编辑器反向控制游戏战斗进程。

## 8. 错误处理 (Error Handling)
*   **收集式异常拦截**：通过 `StringBuilder errorInfo` 和 `AppendSaveRet()` 机制，在 `OnSaveCheck` 阶段收集所有校验异常。校验不通过时，组合报错信息弹出 `EditorUtility.DisplayDialog`，拒绝保存。
*   **冗余容错设计**：在 JSON 反序列化配表数据 `Deserialize()` 时，如果发现本地 `TableTash`（表结构哈希）与当前代码不一致，会自动 `try-catch`。如果反序列化失败，它会自动尝试回读对应 ID 的 Excel 行数据来修复节点，实现了无痛的数据版本迁移。
*   **文件冲突处理**：在打开图谱时，如果检测到 `.json.mine` 等 SVN 冲突文件生成，会使用 `Log.Fatal` 和红框强制弹窗阻止用户操作，避免数据被覆盖。

## 9. 性能优化 (Performance Optimization)
*   **极速序列化/反序列化**：舍弃了 Unity 原生的缓慢序列化，全量改用 `Newtonsoft.Json`。
*   **全局 Hash 缓存加速**：工程中可能有数以千计的技能，`JsonGraphManager` 实现了 `CACHE_DIR` 机制，将所有 Graph 的检索信息（ID 映射表）预先缓存到单个文件中。每次启动只增量加载发生变更的 JSON（通过比对 `LastWriteTime`），极大压缩了打开编辑器的白屏时间。
*   **异步文件 IO 操作**：通过 `Utils.WriteToJsonAsync` 将频繁的自动备份和缓存更新推入后台线程执行，避免卡顿 Unity 主线程。
*   **延迟/按需加载**：只有被激活的 `ConfigGraphWindow` 才会去真正加载 Node 实例和分配材质、渲染 UI。

## 10. 可扩展性 (Extensibility)
*   **全自动管线 (AutoGenerate)**：技能字段增加时（如策划在 Excel 中增加一列“技能护盾值”），无需改写编辑器底层。自动生成工具会重新生成 `SkillConfig.cs`，Odin 会自动渲染出对应的 Inspector 属性。
*   **泛用型枚举绑定**：大量使用了如 `[ValueDropdown("GetCastTargetCondTemplateOptions")]`，下拉菜单内容不是写死的，而是动态从 `SkillTargetCondTemplateConfigManager` 读取。只要增加了新的目标判定条件表，技能下拉框自动扩容。
*   **强大的模板 (Template) 复用**：支持将一段通用的逻辑（如公共的硬直表现或通用 Buff）打包为勾选了 `IsTemplate` 的节点，子技能直接进行参数复写。未来如果添加新的组合机制，可以直接扩展 `TemplateParams` 的反射代理。