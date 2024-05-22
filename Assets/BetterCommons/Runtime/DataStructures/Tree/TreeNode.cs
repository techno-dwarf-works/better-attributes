using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Better.Commons.Runtime.DataStructures.Tree
{
    /// <summary>
    /// Represents a node in a tree structure with a value and a list of child nodes.
    /// </summary>
    /// <typeparam name="T">The type of the value stored in the node.</typeparam>
    public class TreeNode<T>
    {
        // Holds the value of the node
        private protected readonly T _value;

        // List to hold child nodes
        private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();

        /// <summary>
        /// Initializes a new instance of the TreeNode class with the specified value.
        /// </summary>
        /// <param name="value">The value to store in the node.</param>
        public TreeNode(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the child node at the specified index.
        /// </summary>
        /// <param name="i">The zero-based index of the child node to get.</param>
        /// <returns>The child node at the specified index.</returns>
        public TreeNode<T> this[int i] => _children[i];

        /// <summary>
        /// Gets the parent node of this node.
        /// </summary>
        public TreeNode<T> Parent { get; private set; }

        /// <summary>
        /// Gets the value stored in the node.
        /// </summary>
        public T Value => _value;

        /// <summary>
        /// Gets a read-only collection of the node's children.
        /// </summary>
        public ReadOnlyCollection<TreeNode<T>> Children => _children.AsReadOnly();

        /// <summary>
        /// Adds a new child node with the specified value to this node.
        /// </summary>
        /// <param name="value">The value of the new child node.</param>
        /// <returns>The newly created child node.</returns>
        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value) { Parent = this };
            if (value is INodeValue<T> nodeValue)
            {
                // If the value implements INodeValue, set its node reference
                nodeValue.SetNode(node);
            }

            _children.Add(node);
            return node;
        }

        /// <summary>
        /// Adds new child nodes with the specified values to this node.
        /// </summary>
        /// <param name="values">The values for the new child nodes.</param>
        /// <returns>An array of the newly created child nodes.</returns>
        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        /// <summary>
        /// Removes the specified child node from this node.
        /// </summary>
        /// <param name="node">The child node to remove.</param>
        /// <returns>true if the node was successfully removed; otherwise, false.</returns>
        public bool RemoveChild(TreeNode<T> node)
        {
            if (node == null)
            {
                Debug.LogException(new ArgumentException(nameof(node)));
                return default;
            }

            return _children.Remove(node);
        }

        /// <summary>
        /// Performs the specified action on the value of this node and recursively on the values of all descendant nodes.
        /// </summary>
        /// <param name="action">The action to perform on each value.</param>
        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }

        /// <summary>
        /// Returns an enumerable collection of the values from this node and all descendant nodes in depth-first order.
        /// </summary>
        /// <returns>An enumerable collection of values.</returns>
        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(_children.SelectMany(x => x.Flatten()));
        }
    }
}