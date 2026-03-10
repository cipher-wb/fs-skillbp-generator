using UnityEngine;

namespace NodeEditor
{
    internal static partial class Constants
    {
        public const string BytesPackResPath = "Assets/PackRes";
        public const string CSharpTablePath = BytesPackResPath + "/Table";
        public const string CPPTablePath = BytesPackResPath + "/TableCPP";
        public const string NodeEditorPath = "Assets/Thirds/NodeEditor";
        public const string SavePartPath = "Saves/Jsons";
        internal static class SkillEditor
        {

            /// <summary>
            /// 编辑器版本号
            /// </summary>
            public const string Version = "1.0.0";

            /// <summary>
            /// 编辑器路径
            /// </summary>
            public const string Path = "Assets/Thirds/NodeEditor/SkillEditor";
            /// <summary>
            /// 编辑器技能配置数据保存文件夹
            /// </summary>
            public const string PathSaves = Path + "/Saves";

            #region SvnIgnore
            /// <summary>
            /// 编辑器忽略上传路径保存路径
            /// </summary>
            public const string PathSvnIgnore = PathSaves + "/SvnIgnore";
            /// <summary>
            /// 编辑器ID段文件保存路径
            /// </summary>
            public const string PathLocalSettings = PathSvnIgnore + "/LocalID.json";
            /// <summary>
            /// 编辑器查找内容记录
            /// </summary>
            public const string PathSearchResult = PathSvnIgnore + "/SearchResult.log";
            #endregion
            /// <summary>
            /// 编辑器Json资源保存路径
            /// </summary>
            public const string PathSavesJsons = PathSaves + "/Jsons";
            /// <summary>
            /// 技能编辑器信息名
            /// </summary>
            public const string TitleInfo = "技能编辑器信息";
            /// <summary>
            /// 表格说明文件路径
            /// </summary>
            public const string PathTableAnnotation = PathSaves + "/TableAnnotation.json";
            /// <summary>
            /// 表格ID记录文件路径
            /// </summary>
            public const string PathConfigID = PathSaves + "/ConfigID.json";
        }
    }
}
