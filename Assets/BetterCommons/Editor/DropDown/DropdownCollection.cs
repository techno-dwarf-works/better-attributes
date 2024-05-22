using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.EditorAddons.DropDown.Base;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.DataStructures.Tree;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.EditorAddons.DropDown
{
    public class DropdownCollection : TreeNode<DropdownBase>
    {
        public void AddItem(GUIContent[] keys, bool state, Action<object> onSelect, object value)
        {
            if (keys == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(keys));
                return;
            }

            var queue = new Queue<GUIContent>(keys);
            var firstKey = queue.Dequeue();
            var item = Children.FirstOrDefault(x => ValidateEquals(x, firstKey));
            if (queue.Count > 0)
            {
                if (item == null)
                {
                    item = AddChild(new DropdownSubTree(new GUIContent(firstKey)));
                    Iterate(item, state, onSelect, value, queue);
                }
                else
                {
                    Iterate(item, state, onSelect, value, queue);
                }
            }
            else
            {
                AddLeaf(this, firstKey, state, onSelect, value);
            }
        }

        private static bool ValidateEquals(TreeNode<DropdownBase> x, GUIContent firstKey)
        {
            return x.Value.Content.text.Equals(firstKey.text);
        }

        private void Iterate(TreeNode<DropdownBase> item, bool state, Action<object> onSelect, object value,
            Queue<GUIContent> queue)
        {
            var bufferItem = item;
            while (queue.Count > 0)
            {
                var bufferKey = queue.Dequeue();
                var first = bufferItem.Children.FirstOrDefault(x => ValidateEquals(x, bufferKey));
                if (first == null)
                {
                    TreeNode<DropdownBase> node;
                    if (queue.Count <= 0)
                        node = AddLeaf(bufferItem, bufferKey, state, onSelect, value);
                    else
                        node = AddBranch(bufferItem, bufferKey);

                    if (queue.Count > 0)
                    {
                        bufferItem = node;
                    }
                }
                else
                {
                    bufferItem = first;
                }
            }
        }

        private static TreeNode<DropdownBase> AddLeaf(TreeNode<DropdownBase> item, GUIContent bufferKey, bool state,
            Action<object> onSelect, object type)
        {
            if (bufferKey.image == null && state)
            {
                bufferKey.image = IconType.Checkmark.GetIcon();
            }
            return item.AddChild(new DropdownItem(bufferKey, onSelect, type));
        }

        private static TreeNode<DropdownBase> AddBranch(TreeNode<DropdownBase> item, GUIContent bufferKey)
        {
            var treeNode = item.AddChild(new DropdownSubTree(bufferKey));
            return treeNode;
        }

        public DropdownCollection(DropdownBase value) : base(value)
        {
        }
    }
}