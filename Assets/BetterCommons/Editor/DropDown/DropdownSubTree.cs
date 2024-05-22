using Better.Commons.EditorAddons.DropDown.Base;
using Better.Commons.Runtime.DataStructures.Tree;
using UnityEngine;

namespace Better.Commons.EditorAddons.DropDown
{
    public class DropdownSubTree : DropdownBase, INodeValue<DropdownBase>
    {
        private TreeNode<DropdownBase> _node;

        public DropdownSubTree(GUIContent content) : base(content)
        {
        }

        public void SetNode(TreeNode<DropdownBase> node)
        {
            _node = node;
        }

        internal override bool Invoke(DropdownWindow downPopup)
        {
            downPopup.SetCurrentDrawItems(_node);
            return false;
        }
    }
}