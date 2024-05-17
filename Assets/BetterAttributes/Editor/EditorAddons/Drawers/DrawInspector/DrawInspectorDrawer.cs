using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.EditorAddons.Drawers.WrapperCollections;
using Better.Attributes.Runtime.DrawInspector;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Drawers.Attributes;
using Better.Commons.Runtime.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Better.Attributes.EditorAddons.Drawers.DrawInspector
{
    [MultiCustomPropertyDrawer(typeof(DrawInspectorAttribute))]
    public class DrawInspectorDrawer : MultiFieldDrawer<DrawInspectorHandler>
    {
        private DrawInspectorCollection Collection => _handlers as DrawInspectorCollection;

        protected override void Deconstruct()
        {
            _handlers?.Deconstruct();
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var fieldType = GetFieldOrElementType();
            var property = container.Property;
            if (!DrawInspectorUtility.Instance.IsSupported(fieldType))
            {
                container.AddNotSupportedBox(fieldType, _attribute.GetType());
                return;
            }

            if (!container.TryGetByTag(property, out var propertyElement))
            {
                return;
            }

            if (!propertyElement.Elements.TryFind(out PropertyField propertyField))
            {
                return;
            }

            propertyField.RegisterCallback<SerializedPropertyChangeEvent, SerializedProperty>(OnPropertyChanged, property);
            var cache = ValidateCachedProperties(property, DrawInspectorUtility.Instance);

            var inspectorElement = Collection.GetInspectorContainer(property);
            container.CreateElementFrom(inspectorElement);

            Collection.SetupInspector(property);

            var isOpen = Collection.IsOpen(property);
            Collection.SetOpen(property, isOpen);
            var iconType = isOpen ? IconType.Minus : IconType.PlusMore;
            VisualElementUtility.AddClickableIcon(propertyField, iconType, property, OnIconClickEvent);
        }

        private void OnPropertyChanged(SerializedPropertyChangeEvent changeEvent, SerializedProperty property)
        {
            Collection.SetupInspector(property);
        }

        private void OnIconClickEvent(ClickEvent clickEvent, (SerializedProperty property, Image icon) data)
        {
            var isOpen = !Collection.IsOpen(data.property);

            var iconType = isOpen ? IconType.Minus : IconType.PlusMore;
            data.icon.image = iconType.GetIcon();
            Collection.SetOpen(data.property, isOpen);
        }

        protected override HandlerCollection<DrawInspectorHandler> GenerateCollection()
        {
            return new DrawInspectorCollection();
        }

        public DrawInspectorDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }
    }
}