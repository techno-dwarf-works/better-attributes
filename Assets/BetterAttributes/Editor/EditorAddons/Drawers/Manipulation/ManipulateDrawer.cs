using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [MultiCustomPropertyDrawer(typeof(ManipulateAttribute))]
    public class ManipulateDrawer : MultiFieldDrawer<ManipulateWrapper>
    {
        private ManipulateWrapper GetWrapper(SerializedProperty property)
        {
            var cache = ValidateCachedProperties(property, ManipulateUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetProperty(property, (ManipulateAttribute)_attribute);
            }

            return cache.Value.Wrapper;
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            _handlers ??= GenerateCollection();
            var wrapper = GetWrapper(container.Property);
            wrapper.PopulateContainer(container);

            container.OnSerializedObjectChanged += OnSerializedObjectChanged;
        }

        private void OnSerializedObjectChanged(ElementsContainer container)
        {
            var wrapper = GetWrapper(container.Property);
            wrapper.UpdateState(container);
        }

        protected override HandlerCollection<ManipulateWrapper> GenerateCollection()
        {
            return new HandlerCollection<ManipulateWrapper>();
        }

        private ManipulateDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }
    }
}