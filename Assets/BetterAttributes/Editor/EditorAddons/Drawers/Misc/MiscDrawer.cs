using System.Reflection;
using Better.Attributes.EditorAddons.Drawers.Misc.Wrappers;
using Better.Attributes.EditorAddons.Drawers.Utility;
using Better.Attributes.Runtime.Misc;
using Better.Commons.EditorAddons.Drawers.Attributes;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Caching;
using Better.Commons.Runtime.Drawers.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Misc
{
    [MultiCustomPropertyDrawer(typeof(MiscAttribute))]
    public class MiscDrawer : MultiFieldDrawer<MiscWrapper>
    {
        private MiscDrawer(FieldInfo fieldInfo, MultiPropertyAttribute attribute) : base(fieldInfo, attribute)
        {
        }

        private MiscWrapper GetWrapper(SerializedProperty property)
        {
            var cache = ValidateCachedProperties(property, MiscUtility.Instance);
            if (!cache.IsValid)
            {
                cache.Value.Wrapper.SetProperty(property, _fieldInfo, (MiscAttribute)_attribute);
            }

            return cache.Value.Wrapper;
        }

        protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            _handlers ??= GenerateCollection();
            var wrapper = GetWrapper(property); 
            wrapper.PreDraw(position, label);
            
            return true;
        }

        protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        protected override void DrawField(Rect position, SerializedProperty property, GUIContent label)
        {
            var wrapper = GetWrapper(property);
            wrapper.DrawField(position, label);
        }

        protected override HeightCacheValue GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var wrapper = GetWrapper(property);
            return wrapper.GetHeight(label);
        }

        protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
            var wrapper = GetWrapper(property);
            wrapper.PostDraw();
        }

        protected override HandlerCollection<MiscWrapper> GenerateCollection()
        {
            return new HandlerCollection<MiscWrapper>();
        }
    }
}