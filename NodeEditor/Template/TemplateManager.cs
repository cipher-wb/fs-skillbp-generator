using System;
using System.Collections.Generic;
using System.IO;

namespace NodeEditor
{
    public class TemplateManager : Singleton<TemplateManager>
    {
        // TODO 整合模板相关处理

        public TemplateManager()
        {
            ParsePersonnelIds();
        }

        #region 人员信息解析

        private Dictionary<int, string> personnelIds = new Dictionary<int, string>();
        private DateTime lastWriteTime;

        public bool TryGetPersonnelName(int ip, out string name)
        {
            ParsePersonnelIds();
            return personnelIds.TryGetValue(ip, out name);
        }

        /// <summary>
        /// 从HTML内容中解析人员ID查询信息，支持一个人员对应任意数量的ID
        /// </summary>
        private void ParsePersonnelIds()
        {
            try
            {
                var curWriteTime = File.GetLastWriteTime(Constants.AnnotationHtmlTemplatePath);
                if (lastWriteTime == curWriteTime)
                {
                    return;
                }
                personnelIds.Clear();
                var htmlContent = Utils.ReadAllText(Constants.AnnotationHtmlTemplatePath);
                // 查找包含"人员ID查询："的div内容
                int startOffset = htmlContent.IndexOf("<h3>人员ID查询：</h3>");
                if (startOffset == -1) return;

                // 查找ul标签开始位置
                int ulStart = htmlContent.IndexOf("<ul>", startOffset);
                if (ulStart == -1) return;

                // 查找ul标签结束位置
                int ulEnd = htmlContent.IndexOf("</ul>", ulStart);
                if (ulEnd == -1) return;

                // 提取ul内的所有li内容
                string liContent = htmlContent.Substring(ulStart, ulEnd - ulStart);

                // 提取所有<li>标签
                var liItems = ExtractLiItems(liContent);

                foreach (string item in liItems)
                {
                    if (item.Contains("："))
                    {
                        // 分割姓名和ID
                        string[] parts = item.Split('：');
                        if (parts.Length == 2)
                        {
                            string name = parts[0].Trim();
                            string idText = parts[1].Trim();

                            // 处理任意数量的ID（用|分隔）
                            string[] idArray = idText.Split('|');

                            foreach (string singleId in idArray)
                            {
                                if (int.TryParse(singleId, out int id))
                                {
                                    AddOrUpdatePersonnel(id, name);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"ParsePersonnelIds error, ex:{ex}");
            }
        }

        /// <summary>
        /// 添加或更新人员信息，支持一个ID对应多个姓名
        /// </summary>
        private void AddOrUpdatePersonnel(int id, string name)
        {
            if (personnelIds.TryGetValue(id, out var existingNames))
            {
                // 如果ID已存在，检查是否已包含该姓名
                if (!existingNames.Contains(name))
                {
                    // 用"|"分隔多个姓名
                    personnelIds[id] = existingNames + "|" + name;
                }
            }
            else
            {
                // 如果ID不存在，直接添加
                personnelIds.Add(id, name);
            }
        }

        /// <summary>
        /// 反向查询：根据姓名查找对应的所有ID
        /// </summary>
        public Dictionary<string, List<int>> GetIdsByName(Dictionary<int, string> PersonnelIds)
        {
            var nameToIds = new Dictionary<string, List<int>>();

            foreach (var kvp in PersonnelIds)
            {
                int id = kvp.Key;
                string names = kvp.Value;

                // 分割多个姓名
                string[] nameArray = names.Split('|');

                foreach (string name in nameArray)
                {
                    if (!nameToIds.ContainsKey(name))
                    {
                        nameToIds[name] = new List<int>();
                    }

                    if (!nameToIds[name].Contains(id))
                    {
                        nameToIds[name].Add(id);
                    }
                }
            }

            return nameToIds;
        }
        private List<string> ExtractLiItems(string ulContent)
        {
            var items = new List<string>();
            int currentPos = 0;

            while (true)
            {
                int liStart = ulContent.IndexOf("<li>", currentPos);
                if (liStart == -1) break;

                int liEnd = ulContent.IndexOf("</li>", liStart);
                if (liEnd == -1) break;

                string item = ulContent.Substring(liStart + 4, liEnd - liStart - 4).Trim();
                items.Add(item);

                currentPos = liEnd + 5;
            }

            return items;
        }
    }
    #endregion
}