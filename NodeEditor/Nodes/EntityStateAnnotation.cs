using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using System.Text;

namespace NodeEditor
{
    [Serializable, HideReferenceObjectPicker]
    public class TEntityStateAnnotation
    {
        #region 
        public static List<string> ModuleAnnos = new List<string>()
        {
            "",
            "单",
            "组",
            "程序",
        };
        private IEnumerable<ValueDropdownItem> GetModuleNames()
        {
            foreach (var moduleAnno in ModuleAnnos)
            {
                yield return new ValueDropdownItem(moduleAnno, moduleAnno);
            }
        }
        #endregion

        [FoldoutGroup("$Title"), LabelText("Title"), DelayedProperty]
        [HideIf("@true")]
        public string Title
        {
            get
            {
                string msg = ((int)EnumType).ToString() + "【";
                if (IsCanWrite) msg += "改";
                if (IsCanRead) msg += "读";
                msg += "】";
                if (!string.IsNullOrEmpty(ModuleName))
                {
                    msg += ModuleName + "/";
                }
                msg += Name;

                return msg;
            }
        }

        [FoldoutGroup("$Title"), LabelText("状态名"), DelayedProperty]
        [ReadOnly]
        public string Name;

        [FoldoutGroup("$Title"), LabelText("状态枚举"), DelayedProperty]
        [ReadOnly]
        public TableDR.TEntityState EnumType;

        [FoldoutGroup("$Title"), LabelText("分类"), ValueDropdown("GetModuleNames")]
        public string ModuleName = "";

        [FoldoutGroup("$Title"), LabelText("允许 [修改]"), DelayedProperty]
        public bool IsCanWrite = true;

        [FoldoutGroup("$Title"), LabelText("允许 [获取]"), DelayedProperty]
        public bool IsCanRead = true;

        [FoldoutGroup("$Title"), LabelText("备注说明"), DelayedProperty]
        public string Desc = "";

        public TEntityStateAnnotation(string name, TableDR.TEntityState enumType)
        {
            this.Name = name;
            this.EnumType = enumType;
        }

        public string GetDesc()
        {
            var desc = string.Empty;
            var moduleName = string.Empty;
            if (!string.IsNullOrEmpty(Desc))
            {
                desc = $"({Desc})";
            }
            if (!string.IsNullOrEmpty(ModuleName))
            {
                moduleName = ModuleName + "/[" + ModuleName + "] ";
            }
            return moduleName + ((int)EnumType).ToString() + "-" + Name + desc;
        }
    }

    [Serializable, HideMonoScript]
    public sealed class EntityStateAnnotation : ISetting
    {
        public const string name = "状态配置";

        #region 运行时辅助变量
        //[PropertyOrder(-2)]
        [InfoBox("@sbError.ToString()", InfoMessageType.Error), HideIf("@sbError.Length == 0"), LabelText("【注意：上述错误信息，请尽快解决】"), HideReferenceObjectPicker, ShowInInspector]
        private StringBuilder sbError = new StringBuilder();

        /// <summary>
        /// 保存弹框显示信息
        /// </summary>
        private StringBuilder sbSaveInfo = new StringBuilder();

        #endregion

        #region 记录数据
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, NumberOfItemsPerPage = 50)]
        public List<TEntityStateAnnotation> EntityStateAnnos = new List<TEntityStateAnnotation>();
        #endregion

        public EntityStateAnnotation() { }

        public void PresetParams()
        {
            sbError.Length = 0;

            try
            {
                var descs2type = new Dictionary<string, TableDR.TEntityState>();
                var typeT = typeof(TableDR.TEntityState);
                foreach (var enumValue in Enum.GetValues(typeT))
                {
                    var type = (TableDR.TEntityState)enumValue;
                    var desc = Utils.GetEnumDescription(type);
                    descs2type.Add(desc, type);
                }

                var types = descs2type.Values;
                var descs = descs2type.Keys;
                // 清理被删除的类型
                EntityStateAnnos.RemoveAll(p =>
                {
                    var res = !types.Contains(p.EnumType);
                    if (res)
                    {
                        sbError.AppendLine($"【删除】状态名： {p.Title}");
                    }
                    return res;
                });
                foreach (var item in descs2type)
                {
                    var anno = EntityStateAnnos.Find(p => { return p.EnumType == item.Value; });
                    if (anno == null)
                    {
                        // 未找到配置信息,直接New一个
                        var data =  new TEntityStateAnnotation(item.Key, item.Value);
                        EntityStateAnnos.Add(data);
                        sbError.AppendLine($"【新增】状态名： {data.Title}");
                    }
                    // 刷新状态名
                    else
                    {
                        anno.Name = item.Key;
                    }
                }

                // TODO 强制刷新下数据
                UpdateVDList();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }
        public bool SaveSetting(StringBuilder saveInfo)
        {
            UpdateVDList();
            return true;
        }
        public string PathSetting => Constants.SkillEditor.PathTableAnnotation;
        public List<string> PathCommitSetting => new List<string> { PathSetting };

        /// <summary>
        /// 刷新读写的状态列表
        /// </summary>
        public void UpdateVDList()
        {
            //Debug.LogError("UpdateVDList");

            TableDR.CustomEnumUtility.VD_TEntityState_Write.Clear();
            TableDR.CustomEnumUtility.VD_TEntityState_Read.Clear();

            // 字典查找快一点
            Dictionary<int, TEntityStateAnnotation> stateDic = new Dictionary<int, TEntityStateAnnotation>();
            foreach (var item in EntityStateAnnos)
            {
                stateDic.Add((int)item.EnumType, item);
            }

            // 按模块顺序添加
            var typeT = typeof(TableDR.TEntityState);
            for (int i = 0; i < TEntityStateAnnotation.ModuleAnnos.Count; ++i)
            {
                // 按模块顺序添加
                int index = (i + 1) % TEntityStateAnnotation.ModuleAnnos.Count;
                string matchModuleName = TEntityStateAnnotation.ModuleAnnos[index];

                foreach (var enumValue in Enum.GetValues(typeT))
                {
                    var type = (TableDR.TEntityState)enumValue;
                    var desc = ((int)type).ToString() + "-" + Utils.GetEnumDescription(type);
                    var moduleName = "";

                    bool isCanWrite = true;
                    bool isCanRead = true;
                    if (stateDic.TryGetValue((int)type, out TEntityStateAnnotation item))
                    {
                        moduleName = item.ModuleName;
                        isCanWrite = item.IsCanWrite;
                        isCanRead = item.IsCanRead;
                        desc = item.GetDesc();
                    }

                    if (matchModuleName == moduleName)
                    {
                        if (isCanWrite)
                            TableDR.CustomEnumUtility.VD_TEntityState_Write.Add(desc, type);
                        if (isCanRead)
                            TableDR.CustomEnumUtility.VD_TEntityState_Read.Add(desc, type);
                    }
                }
            }
        }
    }
}
