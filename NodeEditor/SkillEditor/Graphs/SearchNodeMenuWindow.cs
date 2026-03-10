using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using GraphProcessor;

namespace NodeEditor
{
    // TODO: replace this by the new UnityEditor.Searcher package
    class SearchNodeMenuWindow : ScriptableObject, ISearchWindowProvider
    {
        ConfigGraphView graphView;
        Texture2D icon;
        private Dictionary<Color, Texture2D> color2Tex = new Dictionary<Color, Texture2D>();

        public void Initialize(ConfigGraphView graphView, EditorWindow window)
        {
            this.graphView = graphView;
        }

        void OnDestroy()
        {
            if (color2Tex.Count > 0)
            {
                foreach (var kv in color2Tex)
                {
                    if (kv.Value != null)
                    {
                        DestroyImmediate(kv.Value);
                    }
                }
                color2Tex.Clear();
            }
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Search Node"), 0),
            };

            CreateStandardNodeMenu(tree);
            return tree;
        }

        void CreateStandardNodeMenu(List<SearchTreeEntry> tree)
        {
            // Sort menu by alphabetical order and submenus
            var nodeEntries = graphView.FilterSearchNodeMenuEntries()/*.OrderBy(k => k.name)*/;
            //var titlePaths = new HashSet<string>();

            var level = 0;
            foreach (var nodeMenuItem in nodeEntries)
            {
                var nodeName = nodeMenuItem.name;
                var color = nodeMenuItem.node.nodeTarget.color;
                if (!color2Tex.TryGetValue(color, out var tex))
                {
                    // Transparent icon to trick search window into indenting items
                    tex = new Texture2D(1, 1);
                    tex.SetPixel(0, 0, color);
                    tex.Apply();
                    color2Tex.Add(color, tex);
                }
                tree.Add(new SearchTreeEntry(new GUIContent(nodeName, tex))
                {
                    level = level + 1,
                    userData = nodeMenuItem.node,
                });
            }
        }
        // Node creation when validate a choice
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is BaseNodeView nodeView)
            {
                nodeView.FocusSelect();
            }
            return true;
        }
    }
}
