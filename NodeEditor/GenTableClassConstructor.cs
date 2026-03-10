#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDR;
using UnityEditor;
using NodeEditor;
using UnityEngine;

namespace NodeEditor
{
    public class GenTableClassConstructor
    {
        private static StringBuilder sbHotfix;
        private static StringBuilder sbHotHotfix;
        private static ProjectConfig projectConfig;

        //[MenuItem("Tools/给导表代码的默认值加构造函数")]
        /// <summary>
        /// 表格构造函数生成以放到打表工具的模版中，此函数弃用
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static void GenTableClass()
        {
            var configFile = Constants.ConfigJsonPath;
            if (!File.Exists(configFile))
                throw new FileNotFoundException($"找不到文件: {configFile}");

            projectConfig = Utils.ReadFromJson<ProjectConfig>(configFile);
            if (projectConfig == null)
                throw new NullReferenceException($"加载配置文件失败: {configFile}");

            var template = AssetDatabase.LoadAssetAtPath<TextAsset>(Constants.TableDefaultValueTemplatePath);

            int index_start = template.text.IndexOf("#start_class");
            int index_end = template.text.IndexOf("#end_class");

            string classtemplate = template.text.Substring(index_start, (index_end - index_start));
            classtemplate = classtemplate.Replace("#start_class", "//auto gen");
           
            string head = template.text.Substring(0, index_start);
            string end = template.text.Substring(index_end+10, template.text.Length - (index_end + 10));

            sbHotfix = new StringBuilder();
            sbHotHotfix = new StringBuilder();
            if(projectConfig.SplitConfig)
            {
                var configPath = Path.Combine(Path.GetDirectoryName(configFile), projectConfig.SplitConfigDir);
                if (Directory.Exists(configPath))
                {
                    projectConfig.Tables.Clear();

                    // 加载目录下所有的表格json文件。
                    Directory.GetFiles(configPath, "*.json").Foreach(tableConfigFile =>
                    {
                        var table = Utils.ReadFromJson<Table>(tableConfigFile);
                        // 跳过不是合法表格的配置文件。
                        if (table == null || string.IsNullOrEmpty(table.Name) || table.Members.Count == 0)
                            return;
                        projectConfig.Tables.Add(table.Name, table);
                    });
                }
            }
            foreach (var kvp in projectConfig.Tables)
            {
                GenTableClass(kvp.Value, classtemplate);
            }

            foreach (var subClass in projectConfig.Classes)
            {
                GenSubClass(subClass, classtemplate);
            }

            StringBuilder sbAllHotfix = new StringBuilder();
            sbAllHotfix.AppendLine(head);
            sbAllHotfix.Append(sbHotfix);
            sbAllHotfix.Append(end);

            StringBuilder sbAllNotHotfix = new StringBuilder();
            sbAllNotHotfix.AppendLine(head);
            sbAllNotHotfix.Append(sbHotHotfix);
            sbAllNotHotfix.Append(end);

            Utils.WriteAllText(Application.dataPath + "/Scripts/TableCache/Hotfix/TableDefaultValue.Hotfix.cs", sbAllHotfix.ToString());
            Utils.WriteAllText(Application.dataPath + "/Scripts/TableCache/NotHotfix/TableDefaultValue.NotHotfix.cs", sbAllNotHotfix.ToString());

            sbHotfix = null;
            sbHotHotfix = null;
            projectConfig = null;
        }

        private static void GenTableClass(Table table,string classtemplate)
        {
            if(!HasDefaultVal(table))
            {
                return;
            }

            StringBuilder sb = table.IsHotfix ? sbHotfix : sbHotHotfix;
            string str = new string(classtemplate);
            str = str.Replace("#Name", table.Name);

            StringReader stringReader = new StringReader(str);
            string lineStr = stringReader.ReadLine();
            while (!string.IsNullOrEmpty(lineStr))
            {
                if(lineStr.Contains("#defaultVal"))
                {
                    for (int i = 0; i < table.Members.Count; i++)
                    {
                        var item = table.Members[i];
                        if (string.IsNullOrEmpty(item.DefaultValue))
                        {
                            continue;
                        }
                        string memberStr = GetDefaultValStr(item,table);
                        sb.AppendLine(lineStr.Replace("#defaultVal", memberStr));
                    }
                }
                else
                {
                    sb.AppendLine(lineStr);
                }
                lineStr = stringReader.ReadLine();
            }
            sb.AppendLine();
        }

        private static void GenSubClass(SubClass subClass, string classtemplate)
        {
            if (!HasDefaultVal(subClass))
            {
                return;
            }

            StringBuilder sb = subClass.IsHotfix ? sbHotfix : sbHotHotfix;
            string str = new string(classtemplate);
            str = str.Replace("#Name", subClass.Name);

            StringReader stringReader = new StringReader(str);
            string lineStr = stringReader.ReadLine();
            while (!string.IsNullOrEmpty(lineStr))
            {
                if (lineStr.Contains("#defaultVal"))
                {
                    for (int i = 0; i < subClass.Members.Count; i++)
                    {
                        var item = subClass.Members[i];
                        if (string.IsNullOrEmpty(item.DefaultValue))
                        {
                            continue;
                        }
                        string memberStr = GetDefaultValStr(item,null);
                        sb.AppendLine(lineStr.Replace("#defaultVal", memberStr));
                    }
                }
                else
                {
                    sb.AppendLine(lineStr);
                }
                lineStr = stringReader.ReadLine();
            }
        }

        private static string GetDefaultValStr(Member member,Table table)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(member.Name);
            sb.Append(" = ");
            switch (member.Type)
            {
                case "Int32":
                    {
                        if (member.HasSeaperater)
                        {
                            GenValuteTypeList(sb, "int", member);
                        }
                        else
                        {
                            sb.Append(member.DefaultValue);
                            sb.Append(";");
                        }
                    }
                    break;
                case "Int64":
                    {
                        if (member.HasSeaperater)
                        {
                            GenValuteTypeList(sb, "long", member);
                        }
                        else
                        {
                            sb.Append(member.DefaultValue);
                            sb.Append(";");
                        }
                    }
                    break;
                case "String":
                    {
                        if (member.HasSeaperater)
                        {
                            GenValuteTypeList(sb, "string", member);
                        }
                        else
                        {
                            sb.Append(member.DefaultValue);
                            sb.Append(";");
                        }
                    }
                    break;
                case "Single":
                    {
                        if (member.HasSeaperater)
                        {
                            GenValuteTypeList(sb, "float", member);
                        }
                        else
                        {
                            sb.Append(member.DefaultValue);
                            sb.Append(";");
                        }
                    }
                    break;
                case "Double":
                    {
                        if (member.HasSeaperater)
                        {
                            GenValuteTypeList(sb, "double", member);
                        }
                        else
                        {
                            sb.Append(member.DefaultValue);
                            sb.Append(";");
                        }
                    }
                    break;
                case "Boolean":
                    {
                        if (member.HasSeaperater)
                        {
                            GenValuteTypeList(sb, "bool", member);
                        }
                        else
                        {
                            sb.Append(member.DefaultValue.ToLower());
                            sb.Append(";");
                        }
                    }
                    break;
                default: // 子类型
                    {
                        SubClass subClass = GetClassByName(member.Type,projectConfig.Classes);
                        bool isLocalEnum = false;
                        if (subClass == null && table!=null)
                        {
                            subClass = GetClassByName(member.Type, table.Classes);
                            if(subClass!=null)
                            {
                                isLocalEnum = true;
                            }
                        }
                        if (subClass == null)
                        {
                            sb.Clear();
                            sb.Append($"//无法找到子类型:{member.Type}");
                            Debug.LogError($"无法找到子类型:{member.Type}");
                            break;
                        }
                        if(subClass.Enum)
                        {
                            GenEmumType(sb,subClass, member, isLocalEnum ? table : null);
                        }
                        else
                        {
                            sb.Clear();
                            sb.Append($"//不支持子类型作为属性时带默认值,字段:{member.Name},子类型:{member.Type}");
                            Debug.LogError($"不支持子类型作为属性时带默认值,字段:{member.Name},子类型:{member.Type}");
                        }
                    }
                    break;
            }
            return sb.ToString();
        }

        private static SubClass GetClassByName(string subClassName, List<SubClass> classes)
        {
            if(classes == null)
            {
                return null;
            }
            foreach (var item in classes)
            {
                if(item.Name == subClassName)
                {
                    return item;
                }
            }
            return null;
        }

        private static void GenValuteTypeList(StringBuilder sb,string typeStr, Member member)
        {
            if (member.HasSeaperater)
            {
                sb.Append($"new List<{typeStr}>()");
                sb.Append("{");
                if (member.Seaperator == ',')
                {
                    sb.Append(member.DefaultValue.ToLower());
                }
                else
                {
                    sb.Append(member.DefaultValue.Replace(member.Seaperator, ',').ToLower());
                }
                sb.Append("};");
            }
            else
            {
                sb.Append(member.DefaultValue.ToLower());
                sb.Append(";");
            }
        }

        private static void GenEmumType(StringBuilder sb,SubClass subClass,Member member,Table table = null)
        {
            if (!member.HasSeaperater)
            {
                if (table != null)
                {
                    sb.Append(table.Name + "_" + subClass.Name + "." + GetEnumStr(subClass, member.DefaultValue) + ";");
                }
                else
                {
                    sb.Append(subClass.Name + "." + GetEnumStr(subClass, member.DefaultValue) + ";");
                }
               
                return;
            }

            if(table != null)
            {
                sb.Append($"new List<{table.Name}_{subClass.Name}>()");
            }
            else
            {
                sb.Append($"new List<{subClass.Name}>()");
            }
           
            sb.Append("{");
            string[] arr = member.DefaultValue.Split(member.Seaperator);
            for (int i = 0; i < arr.Length; i++)
            {
                string s = arr[i];
                sb.Append(subClass.Name+"."+GetEnumStr(subClass, s));
                if (i < (arr.Length - 1))
                {
                    sb.Append(",");
                }
            }
            sb.Append("};");
        }

        private static string GetEnumStr(SubClass subClass,string enumStrVal)
        {
            foreach(var item in subClass.EnumInfos)
            {
                if(item.Name == enumStrVal || item.Value.ToString() == enumStrVal || item.Text == enumStrVal)
                {
                    return item.Name;
                }
            }
            return string.Empty;
        } 

        private static bool HasDefaultVal(Table table)
        {   
            for (int i = 0; i < table.Members.Count; i++)
            {
                ExcelMember excelMember = table.Members[i];
                if(!string.IsNullOrEmpty(excelMember.DefaultValue))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool HasDefaultVal(SubClass subClass)
        {
            if (subClass.Enum)
            {
                return false;
            }
            for (int i = 0; i < subClass.Members.Count; i++)
            {
                Member member = subClass.Members[i];
                if (!string.IsNullOrEmpty(member.DefaultValue))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
#endif