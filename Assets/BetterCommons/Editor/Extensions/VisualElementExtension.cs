using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Commons.EditorAddons.Extensions
{
    public static class VisualElementExtension
    {
        public static void AddClickedEvent(this VisualElement self, SerializedProperty property, EventCallback<ClickEvent, SerializedProperty> action)
        {
            self.RegisterCallback(action, property);
        }

        public static void AddIconClickedEvent(this VisualElement self, SerializedProperty property, EventCallback<ClickEvent, SerializedProperty> action)
        {
            var image = self.Q<Image>();
            AddClickedEvent(image, property, action);
        }

        public static void AddClickableIcon(this VisualElement self, IconType iconType, SerializedProperty property,
            EventCallback<ClickEvent, (SerializedProperty, Image)> action)
        {
            var image = AddIcon(self, iconType);
            image.RegisterCallback(action, (property, image));
        }

        public static Image AddIcon(this VisualElement self, IconType iconType)
        {
            var icon = iconType.GetIcon();
            var image = new Image
            {
                image = icon
            };
            self.Insert(0, image);
            return image;
        }
    }
}