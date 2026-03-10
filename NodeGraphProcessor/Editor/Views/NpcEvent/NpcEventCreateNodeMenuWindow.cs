using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace GraphProcessor
{
    public class NpcEventCreateNodeMenuWindow : CreateNodeMenuWindow
    {
        /// <summary>
        /// 创建节点列表
        /// </summary>
        /// <param name="tree"></param>
        protected override void CreateEdgeNodeMenu(List<SearchTreeEntry> tree)
        {
            var entries = NodeProvider.GetEdgeCreationNodeMenuEntry((edgeFilter.input ?? edgeFilter.output) as PortView, graphView.graph);

            var nodePaths = NodeProvider.GetNodeMenuEntries(graphView.graph);

            var menuItems = entries.Select(port => (port:port, pathAndType:nodePaths.Where(nodePath => nodePath.type == port.nodeType)));

            var sortMenuItems = new List<(NodeProvider.PortDescription port, string path)>();
            foreach (var menuItem in menuItems)
            {
                foreach (var pathAndType in menuItem.pathAndType)
                {
                    if(outputPortView == default)
                    {
                        continue; 
                    }

                    var nodeView = outputPortView.owner;
                    if (!nodeView.SpecificNodeFiltering(outputPortView, pathAndType.path, pathAndType.type))
                    {
                        continue;
                    }

                    var item = (port:menuItem.port, path: pathAndType.path);
                    if (!sortMenuItems.Contains(item))
                    {
                        sortMenuItems.Add(item);
                    }
                }
            }
            var finalMenuItems = sortMenuItems.OrderBy(items => items.path);

            var titlePaths = new HashSet<string>();

            // Sort menu by alphabetical order and submenus
            foreach (var nodeMenuItem in finalMenuItems)
            {
                var nodePath = nodeMenuItem.path;

                // Ignore the node if it's not in the create menu
                if (String.IsNullOrEmpty(nodePath))
                    continue;

                var nodeName = nodePath;
                var level = 0;
                var parts = nodePath.Split('/');
                if (parts.Length > 1)
                {
                    level++;
                    nodeName = parts[parts.Length - 1];
                    var fullTitleAsPath = "";

                    for (var i = 0; i < parts.Length - 1; i++)
                    {
                        var title = parts[i];
                        fullTitleAsPath += title;
                        level = i + 1;

                        // Add section title if the node is in subcategory
                        if (!titlePaths.Contains(fullTitleAsPath))
                        {
                            tree.Add(new SearchTreeGroupEntry(new GUIContent(title))
                            {
                                level = level
                            });
                            titlePaths.Add(fullTitleAsPath);
                        }
                    }
                }

                tree.Add(new SearchTreeEntry(new GUIContent($"{nodeName}", icon))
                {
                    level = level + 1,
                    userData = nodeMenuItem.port
                });
			}
        }
    }
}