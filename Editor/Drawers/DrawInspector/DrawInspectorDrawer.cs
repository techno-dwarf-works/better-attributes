using Better.Attributes.Runtime.DrawInspector;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.DrawInspector
{
    [CustomPropertyDrawer(typeof(DrawInspectorAttribute), true)]
    public class DrawInspectorDrawer : BasePropertyDrawer<DrawInspectorHandler, DrawInspectorAttribute>
    {
        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = GetFieldOrElementType();
            var property = container.SerializedProperty;
            if (!TypeHandlersBinder.IsSupported(fieldType))
            {
                container.AddNotSupportedBox(fieldType, Attribute.GetType());
                return;
            }

            container.SerializedPropertyChanged += OnPropertyChanged;
            var handler = GetHandler(property);

            var inspectorElement = handler.GetInspectorContainer(property);
            container.CreateElementFrom(inspectorElement);
            handler.SetupInspector();

            var isOpen = handler.IsOpen();
            handler.SetOpen(isOpen);
            var iconType = isOpen ? IconType.Minus : IconType.PlusMore;
            container.AddClickableIcon(iconType, property, OnIconClickEvent);
        }

        private void OnPropertyChanged(ElementsContainer elementsContainer)
        {
            var handler = GetHandler(elementsContainer.SerializedProperty);
            handler.SetupInspector();
        }

        private void OnIconClickEvent(ClickEvent clickEvent, (SerializedProperty property, Image icon) data)
        {
            UpdateState(data.property, data.icon);
        }

        private void UpdateState(SerializedProperty property, Image icon)
        {
            var handler = GetHandler(property);
            if (!handler.CanOpen()) return;
            var isOpen = !handler.IsOpen();

            var iconType = isOpen ? IconType.Minus : IconType.PlusMore;
            icon.image = iconType.GetIcon();
            handler.SetOpen(isOpen);
        }
    }
}