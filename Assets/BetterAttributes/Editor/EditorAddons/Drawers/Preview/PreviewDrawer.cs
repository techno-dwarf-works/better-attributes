using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.Runtime.Preview;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    [MultiCustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : MultiFieldDrawer<PreviewHandler>
    {
        public PreviewHandlers Collection => _handlers as PreviewHandlers;


        protected override void Deconstruct()
        {
            _handlers.Deconstruct();
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = _fieldInfo.FieldType;
            var attributeType = _attribute.GetType();
            var property = container.Property;
            if (!PreviewUtility.Instance.IsSupported(fieldType))
            {
                VisualElementUtility.NotSupportedBox(property, fieldType, attributeType);
                return;
            }

            if (!container.TryGetPropertyField(out var propertyElement))
            {
                return;
            }

            propertyElement.RegisterCallback<SerializedPropertyChangeEvent, (SerializedProperty, ElementsContainer)>(OnPropertyChanged, (property, container));
            var cache = ValidateCachedProperties(property, PreviewUtility.Instance);

            var previewSize = ((PreviewAttribute)_attribute).PreviewSize;

            Collection.ValidateObject(property, container);

            var image = container.AddIcon(IconType.View);
            image.RegisterCallback<PointerDownEvent, (SerializedProperty, float)>(OnPointerDown, (property, previewSize));
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
            if(Collection.ValidateObject(data.property, data.container))
            {
                Collection.UpdatePropertyPreviewWindow(data.property);
            }
        }

        protected override HandlerCollection<PreviewHandler> GenerateCollection()
        {
            return new PreviewHandlers();
        }

        public PreviewDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }
    }
}