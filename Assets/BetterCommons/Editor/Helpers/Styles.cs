using UnityEditor;
using UnityEngine;

namespace Better.Commons.EditorAddons.Helpers
{
    public class Styles
    {
        private GUIStyle _inspectorDefaultMargins;
        private GUIStyle _inspectorFullWidthMargins;
        private GUIStyle _defaultContentMargins;
        private GUIStyle _toolbarSearchField;

        private GUIStyle _toolbarSearchFieldCancelButton;
        private GUIStyle _toolbarSearchFieldCancelButtonEmpty;
        private GUIStyle _itemStyle;
        private GUIStyle _header;
        private GUIStyle _checkMark;
        private GUIStyle _lineSeparator;
        private GUIStyle _rightArrow;
        private GUIStyle _leftArrow;
        private GUIStyle _searchFieldStyle;
        private static Styles _current;
        private GUIStyle _defaultContentPaddings;
        private GUIStyle _background;
        private GUIStyle _button;

        private static Styles Current
        {
            get
            {
                if (_current != null) return _current;
                _current = new Styles();
                _current.InitSharedStyles();

                return _current;
            }
        }


        public static GUIStyle ToolbarSearchFieldCancelButton => Current._toolbarSearchFieldCancelButton;

        public static GUIStyle ToolbarSearchFieldCancelButtonEmpty => Current._toolbarSearchFieldCancelButtonEmpty;
        public static GUIStyle ItemStyle => Current._itemStyle;
        public static GUIStyle Button => Current._button;
        public static GUIStyle Header => Current._header;
        public static GUIStyle CheckMark => Current._checkMark;

        public static GUIContent CheckMarkContent => Current._checkMarkContent;
        public static GUIStyle LineSeparator => Current._lineSeparator;
        public static GUIStyle RightArrow => Current._rightArrow;
        public static GUIStyle LeftArrow => Current._leftArrow;
        public static GUIStyle SearchFieldStyle => Current._searchFieldStyle;

        /// <summary>
        ///   <para>Wrap content in a vertical group with this style to get the default margins used in the Inspector.</para>
        /// </summary>
        public static GUIStyle InspectorDefaultMargins => Current._inspectorDefaultMargins;

        /// <summary>
        ///   <para>Wrap content in a vertical group with this style to get full width margins in the Inspector.</para>
        /// </summary>
        public static GUIStyle InspectorFullWidthMargins => Current._inspectorFullWidthMargins;

        public static GUIStyle DefaultContentMargins => Current._defaultContentMargins;

        /// <summary>
        ///   <para>Toolbar search field.</para>
        /// </summary>
        public static GUIStyle ToolbarSearchField => Current._toolbarSearchField;

        public static GUIStyle Background => Current._background;

        public static Color BackgroundColor => UnityEditor.EditorGUIUtility.isProSkin
            ? Current._proBackgroundColor
            : Current._backgroundColor;

        private Color _proBackgroundColor;
        private Color _backgroundColor;
        private GUIContent _checkMarkContent;

        private void InitSharedStyles()
        {
            _proBackgroundColor = new Color32(56, 56, 56, 255);
            _backgroundColor = new Color32(194, 194, 194, 255);
            _button = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                fixedHeight = UnityEditor.EditorGUIUtility.singleLineHeight
            };
            _inspectorDefaultMargins = new GUIStyle
            {
                padding = new RectOffset(18, 4, 4, 0)
            };
            _inspectorFullWidthMargins = new GUIStyle
            {
                padding = new RectOffset(5, 4, 0, 0)
            };
            _defaultContentMargins = new GUIStyle
            {
                padding = new RectOffset(4, 4, 4, 4)
            };

            _itemStyle = GetStyle("DD ItemStyle");
#if UNITY_2022_3_OR_NEWER
            _toolbarSearchField = GetStyle("ToolbarSearchTextField");
            _toolbarSearchFieldCancelButton = GetStyle("ToolbarSearchCancelButton");
            _toolbarSearchFieldCancelButtonEmpty = GetStyle("ToolbarSearchCancelButtonEmpty");
#else
            _toolbarSearchField = GetStyle("ToolbarSeachTextField");
            _toolbarSearchFieldCancelButton = GetStyle("ToolbarSeachCancelButton");
            _toolbarSearchFieldCancelButtonEmpty = GetStyle("ToolbarSeachCancelButtonEmpty");
#endif

            _header = GetStyle("DD HeaderStyle");
            _checkMark = GetStyle("DD ItemCheckmark");
            _lineSeparator = GetStyle("DefaultLineSeparator");
            _rightArrow = GetStyle("ArrowNavigationRight");
            _leftArrow = new GUIStyle(GetStyle("ArrowNavigationLeft"));
            _checkMarkContent = new GUIContent("✔");
            _searchFieldStyle = new GUIStyle(_toolbarSearchField)
            {
                margin = new RectOffset(5, 4, 4, 5)
            };
            _background = GetStyle("DD Background");
        }

        public GUIStyle GetStyle(string styleName)
        {
            GUIStyle style = GUI.skin.FindStyle(styleName) ??
                             UnityEditor.EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
            if (style == null)
            {
                Debug.LogError("Missing built-in guistyle " + styleName);
                style = GUIStyle.none;
            }

            return style;
        }
    }
}