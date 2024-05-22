using System;
using Better.Commons.EditorAddons.Drawers.Base;
using Better.Commons.EditorAddons.Drawers.Utility;
using Better.Commons.Runtime.Utility;
using UnityEditor;

namespace Better.Commons.EditorAddons.Drawers.Caching
{
    public static class ValidateCachedPropertiesUtility
    {
        public static void Validate<TCache, TWrapper, THandler>(HandlerCollection<TWrapper> handlers, TCache cache, SerializedProperty property, Type fieldType,
            Type attributeType, SerializedPropertyUtility<THandler> handler) where TCache : CacheValue<CollectionValue<TWrapper>>
            where TWrapper : SerializedPropertyHandler
            where THandler : new()
        {
            if (cache == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(cache));
                return;
            }
            
            if (handlers == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(handlers));
                cache.Set(false, null);
                return;
            }

            if (property == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(property));
                cache.Set(false, null);
                return;
            }

            if (fieldType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(fieldType));
                cache.Set(false, null);
                return;
            }

            if (attributeType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(attributeType));
                cache.Set(false, null);
                return;
            }

            if (handler == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(handler));
                cache.Set(false, null);
                return;
            }

            if (handlers.TryGetValue(property, out var wrapperCollectionValue))
            {
                handler.ValidateCachedProperties(handlers);
                cache.Set(true, wrapperCollectionValue);
                return;
            }

            var wrapper = handler.GetUtilityWrapper<TWrapper>(fieldType, attributeType);
            if (wrapper == null)
            {
                cache.Set(false, null);
                return;
            }

            var collectionValue = new CollectionValue<TWrapper>(wrapper, fieldType);
            handlers.Add(property, collectionValue);
            cache.Set(false, collectionValue);
        }
    }
}