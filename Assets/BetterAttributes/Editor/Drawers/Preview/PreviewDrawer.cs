using System;
using Better.Attributes.Runtime.Preview;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : BasePropertyDrawer<PreviewHandler, PreviewAttribute>
    {
        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = FieldInfo.FieldType;
            var attributeType = Attribute.GetType();
            var property = container.SerializedProperty;
            if (!TypeHandlersBinder.IsSupported(fieldType))
            {
                container.AddNotSupportedBox(fieldType, attributeType);
                return;
            }

            container.SerializedPropertyChanged += OnPropertyChanged;
            var handler = GetHandler(property);

            var previewSize = Attribute.PreviewSize;

            handler.ProcessElementsContainer(container);

            var image = container.AddIcon(IconType.View);
            image.RegisterCallback<PointerDownEvent, ValueTuple<SerializedProperty, float>>(OnPointerDown, (property, previewSize));
            image.RegisterCallback<PointerUpEvent, SerializedProperty>(OnPointerUp, property);
            image.RegisterCallback<PointerLeaveEvent, SerializedProperty>(OnPointerLeave, property);
            image.RegisterCallback<PointerMoveEvent, SerializedProperty>(OnPointerMove, property);
        }

        private void OnPointerMove(PointerMoveEvent moveEvent, SerializedProperty property)
        {
            var cache = GetHandler(property);
            cache.UpdatePreviewWindow(moveEvent.position);
        }

        private void OnPointerLeave(PointerLeaveEvent leaveEvent, SerializedProperty property)
        {
            var cache = GetHandler(property);
            cache.ClosePreviewWindow();
        }

        private void OnPointerUp(PointerUpEvent upEvent, SerializedProperty property)
        {
            var cache = GetHandler(property);
            cache.ClosePreviewWindow();
        }

        private void OnPointerDown(PointerDownEvent downEvent, (SerializedProperty property, float previewSize) data)
        {
            var cache = GetHandler(data.property);
            cache.OpenPreviewWindow(data.property, downEvent.position, data.previewSize);
        }

        private void OnPropertyChanged(ElementsContainer container)
        {
            var cache = GetHandler(container.SerializedProperty);
            cache.UpdatePropertyPreviewWindow(container);
        }
    }
}