using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Base;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [CustomPropertyDrawer(typeof(ManipulateAttribute), true)]
    public class ManipulateDrawer : BasePropertyDrawer<ManipulateWrapper>
    {
        private ManipulateWrapper GetWrapper(SerializedProperty property)
        {
            var cache = ValidateCachedProperties(property, __ManipulateUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetProperty(property, (ManipulateAttribute)_attribute);
            }

            return cache.Value.Wrapper;
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
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
    }
}