using System;
using Better.Commons.Runtime.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Commons.EditorAddons.Utility
{
    public static class VisualElementUtility
    {
        private static float _spaceHeight = 6f;
        public static float SpaceHeight => _spaceHeight;

        public const int MouseButtonLeft = 0;
        public const int MouseButtonRight = 1;
        public const int MouseButtonMiddle = 2;

        private static readonly HelpBox EmptyHelpBox = new HelpBox();

        public const string NotSupportedTag = nameof(NotSupportedTag);

        public static string NotSupportedMessage(string fieldName, Type fieldType, Type attributeType)
        {
            return
                $"Field {FormatBold(fieldName)} of type {FormatBold(fieldType.Name)} not supported for {FormatBold(attributeType.Name)}";
        }

        /// <summary>
        /// Not supported Inspector HelpBox with RTF text
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="attributeType"></param>
        /// <param name="property"></param>
        public static HelpBox NotSupportedBox(SerializedProperty property, Type fieldType, Type attributeType)
        {
            if (property == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(property));
                return EmptyHelpBox;
            }

            if (fieldType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(fieldType));
                return EmptyHelpBox;
            }

            if (attributeType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(attributeType));
                return EmptyHelpBox;
            }

            var message = NotSupportedMessage(property.name, fieldType, attributeType);
            return HelpBox(message, HelpBoxMessageType.Error);
        }

        public static string FormatBold(string text)
        {
            return $"<b>{text}</b>";
        }

        public static string FormatItalic(string text)
        {
            return $"<i>{text}</i>";
        }

        public static string FormatBoldItalic(string text)
        {
            return $"<b><i>{text}</i></b>";
        }

        public static string BeautifyFormat(string text)
        {
            return $"\"<b><i>{text}</i></b>\"";
        }

        /// <summary>
        /// Override for default Inspector HelpBox with style
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public static HelpBox HelpBox(string message, HelpBoxMessageType type)
        {
            if (message == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(message));
                return EmptyHelpBox;
            }

            return new HelpBox(message, type);
        }

        /// <summary>
        /// Override for default Inspector HelpBox with style
        /// </summary>
        /// <param name="message"></param>
        public static HelpBox HelpBox(string message)
        {
            if (message == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(message));
                return EmptyHelpBox;
            }

            return HelpBox(message, HelpBoxMessageType.None);
        }

        public static int SelectionGrid(int selected, string[] texts, int xCount, GUIStyle style,
            params GUILayoutOption[] options)
        {
            var bufferSelected = selected;
            GUILayout.BeginVertical();
            var count = 0;
            var isHorizontal = false;

            for (var index = 0; index < texts.Length; index++)
            {
                var text = texts[index];

                if (count == 0)
                {
                    GUILayout.BeginHorizontal();
                    isHorizontal = true;
                }

                count++;
                if (GUILayout.Toggle(bufferSelected == index, text, new GUIStyle(style), options))
                    bufferSelected = index;

                if (count == xCount)
                {
                    GUILayout.EndHorizontal();
                    count = 0;
                    isHorizontal = false;
                }
            }

            if (isHorizontal) GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            return bufferSelected;
        }

        public static bool IsLeftButton(ClickEvent clickEvent)
        {
            return IsMouseButton(clickEvent, MouseButtonLeft);
        }

        public static bool IsRightButton(ClickEvent clickEvent)
        {
            return IsMouseButton(clickEvent, MouseButtonRight);
        }

        public static bool IsMiddleButton(ClickEvent clickEvent)
        {
            return IsMouseButton(clickEvent, MouseButtonMiddle);
        }

        public static bool IsMouseButton(ClickEvent clickEvent, int mouseButton)
        {
            return clickEvent.button == mouseButton;
        }

        public static VisualElement CreateHorizontalGroup()
        {
            var element = new VisualElement();
            element.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            return element;
        }

        public static VisualElement CreateVerticalGroup()
        {
            var element = new VisualElement();
            element.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            return element;
        }
    }
}