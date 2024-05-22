using System.Collections.Generic;
using System.Linq;
using Better.Commons.EditorAddons.DropDown.Base;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.DataStructures.Tree;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Better.Commons.EditorAddons.DropDown
{
    public class DropdownWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private string _searchText;
        private Vector2 _display;
        private int _maxLines = 10;

        private TreeNode<DropdownBase> _currentNode;
        private Stack<TreeNode<DropdownBase>> _previousItems = new Stack<TreeNode<DropdownBase>>();
        private GUIContent _header;

        [DidReloadScripts]
        private static void OnDidReloadScripts()
        {
            CloseInstance();
        }

        public static void CloseInstance()
        {
            if (HasOpenInstances<DropdownWindow>())
            {
                var window = GetWindow<DropdownWindow>();
                window.ResetPopup();
                window.Close();
            }
        }

        public static DropdownWindow ShowWindow(Rect displayPosition, GUIContent header)
        {
            var window = HasOpenInstances<DropdownWindow>()
                ? GetWindow<DropdownWindow>()
                : CreateInstance<DropdownWindow>();
            window.ResetPopup();
            window.ShowPopup();
            window.SetPosition(displayPosition);
            window._header = header;
            return window;
        }

        private void SetPosition(Rect displayPosition)
        {
            position = displayPosition;
            _display = displayPosition.position;
        }

        private void ResetPopup()
        {
            _currentNode = null;
            _previousItems.Clear();
            _scrollPosition = Vector2.zero;
            _searchText = string.Empty;
        }

        private void OnLostFocus()
        {
            CloseInstance();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            _searchText = DropdownUtility.DrawSearchField(_searchText, false);
            var buffer = SearchResult(_currentNode, _searchText);

            var hasParent = _currentNode.Parent != null;
            DropdownUtility.DrawHeader(hasParent ? _currentNode.Value.Content : _header,
                hasParent, OnBackClicked);
            var close = AnalyseUserInput(buffer);
            if (!close)
            {
                close = DrawItems(buffer, Styles.Button);
            }

            EditorGUILayout.EndVertical();

            if (!close) return;
            ResetPopup();
            Close();
        }

        private void Update()
        {
            Repaint();
        }

        private void OnBackClicked()
        {
            _currentNode = _previousItems.Pop();
        }

        public void SetItems(DropdownCollection collection)
        {
            ResetPopup();
            SetCurrentDrawItems(collection);
        }

        internal void SetCurrentDrawItems(TreeNode<DropdownBase> node)
        {
            if (_currentNode != null)
            {
                _previousItems.Push(_currentNode);
            }

            _currentNode = node;
        }

        private bool DrawItems(List<TreeNode<DropdownBase>> buffer, GUIStyle buttonStyle)
        {
            _scrollPosition =
                EditorGUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);

            EditorGUILayout.BeginVertical(GUIStyle.none);
            var shouldClose = false;

            var size = Vector2.zero;
            foreach (var item in buffer)
            {
                var bufferContent = item.Value.Content;
                size = Vector2.Max(buttonStyle.CalcSize(bufferContent), size);
                if (DropdownUtility.DrawItem(item, item.Children.Count > 0, true))
                    shouldClose = item.Value.Invoke(this);
            }

            ResolveSize(size);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            return shouldClose;
        }

        private List<TreeNode<DropdownBase>> SearchResult(TreeNode<DropdownBase> rootNode, string searchText)
        {
            var buffer = string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText)
                ? rootNode.Children.ToList()
                : rootNode.Children.Where(node => RecursiveSearch(node, searchText)).ToList();

            return buffer;
        }

        private bool RecursiveSearch(TreeNode<DropdownBase> node, string searchText)
        {
            var isFound = node.Value.Contains(searchText);
            if (node.Children.Count > 0)
            {
                isFound = isFound || node.Children.Any(x => RecursiveSearch(x, searchText));
            }

            return isFound;
        }

        private bool AnalyseUserInput(List<TreeNode<DropdownBase>> buffer)
        {
            if (Event.current.type != EventType.KeyDown)
            {
                return false;
            }

            var currentKeyCode = Event.current.keyCode;
            switch (currentKeyCode)
            {
                case KeyCode.Return:
                    return CaseReturn(buffer);
                case KeyCode.Escape:
                    return CaseEscape();
            }

            return false;
        }

        private bool CaseReturn(List<TreeNode<DropdownBase>> buffer)
        {
            var first = buffer.FirstOrDefault();
            return first != null && first.Value.Invoke(this);
        }

        private bool CaseEscape()
        {
            if (!string.IsNullOrEmpty(_searchText))
            {
                if (_previousItems.Count <= 0) return true;
                OnBackClicked();
                return _previousItems.Count <= 0;
            }

            _searchText = string.Empty;
            DropdownUtility.ClearSearchField();

            return false;
        }

        private void ResolveSize(Vector2 size)
        {
            var width = Mathf.Max(position.width, size.x + 50f);
            var height = UnityEditor.EditorGUIUtility.singleLineHeight * _maxLines + VisualElementUtility.SpaceHeight;
            var copy = position;
            copy.position = _display;
            copy.width = width;
            copy.height = height;
            position = Reposition(copy);
        }

        private Rect Reposition(Rect copy)
        {
            var mainWindowSize = UnityEditor.EditorGUIUtility.GetMainWindowPosition();
            if (copy.x < mainWindowSize.x)
            {
                var newX = mainWindowSize.min.x;
                copy.position = new Vector2(newX, copy.position.y);
            }

            if (copy.y < mainWindowSize.y)
            {
                var newY = mainWindowSize.min.y;
                copy.position = new Vector2(position.x, newY);
            }

            var height = mainWindowSize.y + mainWindowSize.height;
            if (copy.y + copy.height > height)
            {
                var newY = height - copy.height;
                copy.position = new Vector2(position.x, newY);
            }

            var width = mainWindowSize.x + mainWindowSize.width;
            if (copy.x + copy.width > width)
            {
                var newX = width - copy.width;
                copy.position = new Vector2(newX, copy.position.y);
            }

            return copy;
        }
    }
}