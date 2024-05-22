namespace Better.Commons.Runtime.DataStructures.Tree
{
    /// <summary>
    /// Defines a contract for values stored in tree nodes that need to be aware of their corresponding tree node.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the tree node.</typeparam>
    public interface INodeValue<T>
    {
        /// <summary>
        /// Sets the TreeNode that this value is associated with.
        /// </summary>
        /// <param name="node">The TreeNode object that this value is contained within.</param>
        void SetNode(TreeNode<T> node);
    }
}