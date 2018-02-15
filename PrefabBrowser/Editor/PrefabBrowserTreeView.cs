using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace HypnagogiaGames.PrefabBrowser
{
    public class PrefabBrowserTreeView : TreeView
    {
        class PrefabViewItem : TreeViewItem
        {
            public GameObject gameObject;
        }

        PrefabViewItem root;
        List<PrefabViewItem> allItems;
        private GameObject targetObject;

        public PrefabBrowserTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
            // are created from data. Here we create a fixed set of items. In a real world example,
            // a data model should be passed into the TreeView and the items created from the model.

            // This section illustrates that IDs should be unique. The root item is required to 
            // have a depth of -1, and the rest of the items increment from that.

            if (targetObject != null)
            {
                int id = 0;
                root = new PrefabViewItem { id = id++, depth = -1, displayName = "Root" };
                allItems = new List<PrefabViewItem>();

                allItems.Add(new PrefabViewItem { id = id++, depth = 0, displayName = targetObject.name, gameObject = targetObject });
                AddChildren(id, 1, targetObject.transform);
            }
            else
            {
                root = new PrefabViewItem { id = 0, depth = -1, displayName = "No object selected" };
                allItems = new List<PrefabViewItem>();
            }

            // Utility method that initializes the TreeViewItem.children and .parent for all items. 
            SetupParentsAndChildrenFromDepths(root, allItems.Select(x => (TreeViewItem)x).ToList());

            // Return root of the tree
            return root;
        }

        private int AddChildren(int id, int depth, Transform parent)
        {
            if (parent.childCount <= 0)
                return id;

            foreach (Transform child in parent)
            {
                allItems.Add(new PrefabViewItem { id = id++, depth = depth, displayName = child.name, gameObject = child.gameObject });

                id = AddChildren(id, depth + 1, child);
            }

            return id;
        }

        internal void SetTarget(GameObject targetObject)
        {
            this.targetObject = targetObject;
            Reload();
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            if (allItems != null)
            {
                var items = allItems.Where(x => selectedIds.Contains(x.id));
                var objects = items.Select(x => x.gameObject).ToArray();
                UnityEditor.Selection.objects = objects;
            }
        }
    }
} 
