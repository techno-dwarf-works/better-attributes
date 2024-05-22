using System;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Commons.EditorAddons.Utility
{
    public static class ElementsContainerExtensions
    {
        //TODO: Add validation
        public static void AddClickableIcon(this ElementsContainer self, IconType iconType, SerializedProperty property,
            EventCallback<ClickEvent, (SerializedProperty, Image)> action)
        {
            var image = self.AddIcon(iconType);
            image.RegisterCallback(action, (property, image));
        }

        public static Image AddIcon(this ElementsContainer self, IconType iconType)
        {
            var icon = iconType.GetIcon();
            var image = new Image
            {
                image = icon
            };
            image.AddToClassList(StyleDefinition.SingleLineHeight);
            var element = self.GetByTag(self.Property);
            element.RootStyle.FlexDirection(new StyleEnum<FlexDirection>(FlexDirection.Row));
            element.Elements.Insert(0, image);
            return image;
        }
        
        public static bool TryGetPropertyField(this ElementsContainer self, out PropertyField propertyField)
        {
            if (!self.TryGetByTag(self.Property, out var propertyElement))
            {
                propertyField = null;
                return false;
            }

            if (!propertyElement.Elements.TryFind(out propertyField))
            {
                propertyField = null;
                return false;
            }

            return true;
        }

        public static void AddNotSupportedBox(this ElementsContainer self, Type fieldType, Type attributeType)
        {
            var helpBox = VisualElementUtility.NotSupportedBox(self.Property, fieldType, attributeType);
            if (self.TryGetByTag(VisualElementUtility.NotSupportedTag, out var element))
            {
                element.Elements.Add(helpBox);
            }
            else
            {
                element = self.CreateElementFrom(helpBox);
                element.AddTag(VisualElementUtility.NotSupportedTag);
            }
        }

        public static FieldVisualElement GetOrAddHelpBox(this ElementsContainer self, string message, object tag, HelpBoxMessageType messageType)
        {
            if (!self.TryGetByTag(tag, out var element))
            {
                var helpBox = VisualElementUtility.HelpBox(message, messageType);
                element = self.CreateElementFrom(helpBox);
                element.AddTag(tag);
            }

            return element;
        }
    }
}