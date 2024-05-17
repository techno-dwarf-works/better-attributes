using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Manipulation.Wrappers;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime.Manipulation;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Manipulation
{
    [MultiCustomPropertyDrawer(typeof(ManipulateAttribute))]
    public class ManipulateDrawer : MultiFieldDrawer<ManipulateWrapper>
    {
        private ManipulateDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        private ManipulateWrapper GetWrapper(SerializedProperty property)
        {
            var cache = ValidateCachedProperties(property, ManipulateUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetProperty(property, (ManipulateAttribute)_attribute);
            }

            return cache.Value.Wrapper;
        }

        protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            _handlers ??= GenerateCollection();
            var wrapper = GetWrapper(property); 
            wrapper.PreDraw(ref position);
            
            return true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        protected override HeightCacheValue GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var wrapper = GetWrapper(property);
            return wrapper.GetHeight();
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
            var wrapper = GetWrapper(property);
            wrapper.PostDraw();
        }

        protected override HandlerCollection<ManipulateWrapper> GenerateCollection()
        {
            return new HandlerCollection<ManipulateWrapper>();
        }
    }
}