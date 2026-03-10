using System;
using System.Text;

namespace NodeEditor
{
    /// <summary>
    /// html相关公共方法
    /// </summary>
    public static class HtmlUtils
    {
        public struct AnntationNodeData
        {
            public string Title;
            public string Desc;
            public string Content;
        }
        public static string GenerateNodeHtml(string title, string desc, string content, string type = "", string head = "")
        {
            var result =
                $"{head}<div class=\"content-section-node\" data-type=\"{type}\">\n" +
                $"{head}    <h3 style=\"white-space: pre-wrap;\">{EncodeEnter(title)}</h3>\n" +
                $"{head}    <h4 style=\"white-space: pre-wrap;\">{EncodeEnter(desc)}</h4>\n" +
                $"{head}        <p style=\"white-space: pre-wrap;\">{EncodeEnter(content)}</p>\n" +
                $"{head}</div>";
            return result;
        }
        private static void BuildDirectory(StringBuilder html, string[] parts, int index)
        {
            if (index >= parts.Length) return;

            bool hasChildren = index < parts.Length - 1;
            string indent = new string(' ', (index + 2) * 4);

            html.AppendLine($"{indent}<li{(hasChildren ? " class=\"collapsed\"" : "")}>");
            html.AppendLine($"{indent}    <span>{parts[index]}</span>");

            if (hasChildren)
            {
                html.AppendLine($"{indent}    <ul>");
                BuildDirectory(html, parts, index + 1);
                html.AppendLine($"{indent}    </ul>");
            }

            html.AppendLine($"{indent}</li>");
        }
        // TODO
        public static string GenerateDirectoryHtml(string directoryPath, string head = "")
        {
            if (string.IsNullOrEmpty(directoryPath)) return string.Empty;

            directoryPath.Replace("\\", "/");
            var parts = directoryPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var html = new StringBuilder();
            BuildDirectory(html, parts, 0);
            return html.ToString();
        }

        public static string EncodeEnter(string input)
        {
            return input.Replace("\n", "<br/>");
        }

        public static string EncodeToHtml(string input)
        {
            return input.Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("&", "&amp;")
                        .Replace("'", "&apos;")
                        .Replace("\"", "&quot;");
        }
    }
}