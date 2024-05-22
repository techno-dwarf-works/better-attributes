using System;
using System.Reflection;
using Better.Commons.EditorAddons.DropDown.Base;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.Runtime.DataStructures.Tree;
using UnityEditor;
using UnityEngine;

namespace Better.Commons.EditorAddons.Utility
{
    internal static class DropdownUtility
    {
        private const string SearchFieldName = "SearchField";
        private static Vector2 _iconSize = new Vector2(13f, 13f);
        private static readonly MethodInfo _setMouseTooltipMethod;

        static DropdownUtility()
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            _setMouseTooltipMethod = typeof(GUIStyle).GetMethod("SetMouseTooltip",bindingFlags);
        }
        
        public static bool DrawItem(TreeNode<DropdownBase> item, bool hasChildren, bool enabled)
        {
            var content = item.Value.Content;
            var image1 = content.image;
            if (content.image == null)
                content.image = Texture2D.whiteTexture;
            var rect = GUILayoutUtility.GetRect(content, Styles.ItemStyle, GUILayout.ExpandWidth(true));
            content.image = image1;

            var isClicked = false;
            var isHover = rect.Contains(Event.current.mousePosition);
            if (enabled)
            {
                isClicked = Event.current.type == EventType.MouseDown && isHover;
            }
            
            if (isHover)
            {
                if (!string.IsNullOrEmpty(content.tooltip))
                {
                    _setMouseTooltipMethod?.Invoke(null, new object[] { content.tooltip, rect });
                }
            }

            if (Event.current.type != EventType.Repaint)
            {
                return isClicked;
            }

            var image2 = content.image;

            if (hasChildren)
            {
                Styles.CheckMark.Draw(new Rect(rect)
                {
                    width = _iconSize.x + 1f
                }, GUIContent.none, false, false, isHover, isHover);
                
                rect.x += _iconSize.x + 1f;
                rect.width -= _iconSize.x + 1f;
                content.image = null;
            }
            else if (content.image == null)
            {
                Styles.ItemStyle.Draw(rect, GUIContent.none, false, false, isHover, isHover);
                rect.x += _iconSize.x + 1f;
                rect.width -= _iconSize.x + 1f;
            }

            using (new EditorGUI.DisabledScope(!enabled))
            {
                Styles.ItemStyle.Draw(rect, content, false, false, isHover, isHover);
                content.image = image2;
                if (hasChildren)
                {
                    var num = (float)((Styles.ItemStyle.fixedHeight -
                                       (double)Styles.RightArrow.fixedHeight) / 2.0);
                    var position = new Rect(
                        rect.xMax - Styles.RightArrow.fixedWidth - Styles.RightArrow.margin.right, rect.y + num,
                        Styles.RightArrow.fixedWidth, Styles.RightArrow.fixedHeight);
                    Styles.RightArrow.Draw(position, false, false, false, false);
                }
            }

            return isClicked;
        }

        public static void DrawHeader(GUIContent content, bool hasParent, Action onBackClick)
        {
            var rect = GUILayoutUtility.GetRect(content, Styles.Header, GUILayout.ExpandWidth(true),
                GUILayout.MaxHeight(22f));
            var isHover = rect.Contains(Event.current.mousePosition);
            if (Event.current.type == EventType.Repaint)
                Styles.Header.Draw(rect, content, isHover, false, false, false);
            if (!hasParent)
                return;
            var num = (float)((rect.height - (double)Styles.LeftArrow.fixedWidth) / 2.0);
            var leftArrowPosition = new Rect(rect.x + Styles.LeftArrow.margin.left, rect.y + num,
                Styles.LeftArrow.fixedWidth, Styles.LeftArrow.fixedHeight);
            if (Event.current.type == EventType.Repaint)
                Styles.LeftArrow.Draw(leftArrowPosition, false, false, false, false);
            if (Event.current.type == EventType.MouseDown & isHover)
            {
                onBackClick?.Invoke();
                Event.current.Use();
            }
        }

        public static string DrawSearchField(string searchString, bool isSearchFieldDisabled)
        {
            if (!isSearchFieldDisabled && string.IsNullOrEmpty(GUI.GetNameOfFocusedControl()))
                EditorGUI.FocusTextInControl(SearchFieldName);
            using (new EditorGUI.DisabledScope(isSearchFieldDisabled))
            {
                GUI.SetNextControlName(SearchFieldName);
                var str = DrawSearchFieldControl(searchString);
                return str;
            }
        }

        private static string DrawSearchFieldControl(string searchString)
        {
            var rect = GUILayoutUtility.GetRect(0.0f, Styles.SearchFieldStyle.fixedHeight, Styles.SearchFieldStyle);
            ++rect.xMin;
            --rect.xMax;
            ++rect.yMin;
            var searchRect = Styles.SearchFieldStyle.margin.Add(rect);
            EditorGUI.DrawRect(searchRect, Styles.BackgroundColor);
            return ToolbarSearchField(rect, searchString, Styles.ToolbarSearchField,
                searchString != ""
                    ? Styles.ToolbarSearchFieldCancelButton
                    : Styles.ToolbarSearchFieldCancelButtonEmpty);
        }

        private static string ToolbarSearchField(Rect position, string text, GUIStyle searchFieldStyle,
            GUIStyle cancelButtonStyle)
        {
            var position1 = position;
            var position2 = position;
            position2.x += position.width - 14f;
            position2.width = 14f;
            if (!string.IsNullOrEmpty(text))
                UnityEditor.EditorGUIUtility.AddCursorRect(position2, MouseCursor.Arrow);
            if (Event.current.type == EventType.MouseUp && position2.Contains(Event.current.mousePosition))
            {
                text = string.Empty;
                ClearSearchField();
            }

            text = EditorGUI.TextField(position1, text, searchFieldStyle);
            GUI.Button(position2, GUIContent.none, cancelButtonStyle);
            return text;
        }

        public static void ClearSearchField()
        {
            UnityEditor.EditorGUIUtility.editingTextField = false;
            GUI.changed = true;
        }
    }
}