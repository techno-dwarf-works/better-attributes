using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    [MultiCustomPropertyDrawer(typeof(MiscAttribute))]
    public class MiscDrawer : MultiFieldDrawer<MiscHandler>
    {
        private MiscDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        protected override HandlerCollection<MiscHandler> GenerateCollection()
        {
            return new HandlerCollection<MiscHandler>();
        }

        protected override void PopulateContainer(ElementsContainer container)
        {
            var property = container.Property;
            var cache = ValidateCachedProperties(property, MiscUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetupContainer(container, _fieldInfo, (MiscAttribute)_attribute);
            }
        }
    }
}