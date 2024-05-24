using System;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.Runtime.Preview;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : BasePropertyDrawer<PreviewHandler>
    {
        public PreviewHandlers Collection => _handlers as PreviewHandlers;

        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = _fieldInfo.FieldType;
            var attributeType = _attribute.GetType();
            var property = container.Property;
            if (!__PreviewUtility.Instance.IsSupported(fieldType))
            {
                container.AddNotSupportedBox(fieldType, attributeType);
                return;
            }

            if (!container.TryGetPropertyField(out var propertyElement))
            {
                return;
            }

            propertyElement.RegisterCallback<SerializedPropertyChangeEvent, (SerializedProperty, ElementsContainer)>(OnPropertyChanged, (property, container));
            var cache = ValidateCachedProperties(property, __PreviewUtility.Instance);

            var previewSize = ((PreviewAttribute)_attribute).PreviewSize;

            Collection.__ValidateObject(property, container);

            var image = container.AddIcon(IconType.View);
            image.RegisterCallback<PointerDownEvent, ValueTuple<SerializedProperty, float>>(OnPointerDown, (property, previewSize));
            image.RegisterCallback<PointerUpEvent, SerializedProperty>(OnPointerUp, property);
            image.RegisterCallback<PointerLeaveEvent, SerializedProperty>(OnPointerLeave, property);
            image.RegisterCallback<PointerMoveEvent, SerializedProperty>(OnPointerMove, property);
        }

        private void OnPointerMove(PointerMoveEvent moveEvent, SerializedProperty property)
        {
            Collection.UpdatePreviewWindow(property, moveEvent.position);
        }

        private void OnPointerLeave(PointerLeaveEvent leaveEvent, SerializedProperty property)
        {
            Collection.ClosePreviewWindow(property);
        }

        private void OnPointerUp(PointerUpEvent upEvent, SerializedProperty property)
        {
            Collection.ClosePreviewWindow(property);
        }

        private void OnPointerDown(PointerDownEvent downEvent, (SerializedProperty property, float previewSize) data)
        {
            Collection.OpenPreviewWindow(downEvent.position, data.property, data.previewSize);
        }

        private void OnPropertyChanged(SerializedPropertyChangeEvent changeEvent, (SerializedProperty property, ElementsContainer container) data)
        {
            if(Collection.__ValidateObject(data.property, data.container))
            {
                Collection.UpdatePropertyPreviewWindow(data.property);
            }
        }

        protected override HandlerCollection<PreviewHandler> GenerateCollection()
        {
            return new PreviewHandlers();
        }
    }
}