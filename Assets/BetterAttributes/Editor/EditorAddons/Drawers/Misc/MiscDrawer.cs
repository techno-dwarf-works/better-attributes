using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Base;
using UnityEditor;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    [CustomPropertyDrawer(typeof(MiscAttribute), true)]
    public class MiscDrawer : BasePropertyDrawer<MiscHandler>
    {
        protected override HandlerCollection<MiscHandler> GenerateCollection()
        {
            return new HandlerCollection<MiscHandler>();
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var property = container.Property;
            var cache = ValidateCachedProperties(property, __MiscUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetupContainer(container, _fieldInfo, (MiscAttribute)_attribute);
            }
        }
    }
}