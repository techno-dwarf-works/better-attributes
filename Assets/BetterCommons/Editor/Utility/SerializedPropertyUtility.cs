using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Better.Commons.EditorAddons.Helpers;
using Better.Commons.Runtime.Extensions;
using Better.Commons.Runtime.Utility;
using Better.Internal.Core.Runtime;
using UnityEditor.Callbacks;

namespace Better.Commons.EditorAddons.Utility
{
    public static class SerializedPropertyUtility
    {
        public static readonly Regex ArrayDataWithIndexRegex = new Regex(@"\.Array\.data\[[0-9]+\]", RegexOptions.Compiled);
        public static readonly Regex ArrayDataWithIndexRegexAny = new Regex(@"\.Array\.data\[[0-9]+\]$", RegexOptions.Compiled);

        public static readonly Regex ArrayElementRegex = new Regex(@"\GArray\.data\[(\d+)\]", RegexOptions.Compiled);
        public static readonly Regex ArrayIndexRegex = new Regex(@"\[([^\[\]]*)\]", RegexOptions.Compiled);

        public static readonly Regex ArrayRegex = new Regex(@"\.Array\.data", RegexOptions.Compiled);

        public const int IteratorNotAtEnd = 2;
        public const string ArrayDataName = ".Array.data[";
        private const string ArrayElementDotName = "." + ArrayElementName;
        private const string ArrayElementName = "___ArrayElement___";

        private static readonly Dictionary<CachePropertyKey, CachedFieldInfo> FieldInfoFromPropertyPathCache = new Dictionary<CachePropertyKey, CachedFieldInfo>();

        [DidReloadScripts]
        private static void Reload()
        {
            FieldInfoFromPropertyPathCache.Clear();
        }

        public static string GetPropertyParentList(string propertyPath)
        {
            if (propertyPath.IsNullOrEmpty())
            {
                DebugUtility.LogException<ArgumentException>(nameof(propertyPath));
                return string.Empty;
            }

            int length = propertyPath.LastIndexOf(ArrayDataName, StringComparison.Ordinal);
            return length < 0 ? null : propertyPath.Substring(0, length);
        }

        public static bool GetTypeFromManagedReferenceFullTypeName(string managedReferenceFullTypeName, out Type managedReferenceInstanceType)
        {
            if (managedReferenceFullTypeName.IsNullOrEmpty())
            {
                managedReferenceInstanceType = null;
                return false;
            }

            var parts = managedReferenceFullTypeName.Split(' ');
            if (parts.Length == 2)
            {
                var assemblyPart = parts[0];
                var classNamePart = parts[1];
                managedReferenceInstanceType = Type.GetType($"{classNamePart}, {assemblyPart}");
            }

            managedReferenceInstanceType = null;
            return managedReferenceInstanceType != null;
        }

        public static CachedFieldInfo GetFieldInfoFromPropertyPath(Type type, string propertyPath)
        {
            var arrayElement = ArrayDataWithIndexRegexAny.IsMatch(propertyPath);
            propertyPath = ArrayDataWithIndexRegex.Replace(propertyPath, ArrayElementDotName);
            var cache = new CachePropertyKey(type, propertyPath);

            if (FieldInfoFromPropertyPathCache.TryGetValue(cache, out var fieldInfoCache))
            {
                return fieldInfoCache;
            }
            
            if (FieldInfoFromPropertyPath(propertyPath, ref type, out var fieldInfo))
            {
                return null;
            }

            if (arrayElement && type != null && type.IsArrayOrList())
            {
                type = type.GetCollectionElementType();
            }

            fieldInfoCache = new CachedFieldInfo(fieldInfo, type);
            FieldInfoFromPropertyPathCache.Add(cache, fieldInfoCache);
            return fieldInfoCache;
        }

        private static bool FieldInfoFromPropertyPath(string propertyPath, ref Type type, out FieldInfo fieldInfo)
        {
            var originalType = type;
            fieldInfo = null;
            var parts = propertyPath.Split('.');
            for (var i = 0; i < parts.Length; i++)
            {
                var member = parts[i];
                FieldInfo foundField = null;
                for (var currentType = type; foundField == null && currentType != null; currentType = currentType.BaseType)
                {
                    foundField = currentType.GetField(member, Defines.ConstructorFlags);
                }

                if (foundField == null)
                {
                    var cacheKey = new CachePropertyKey(originalType, propertyPath);
                    FieldInfoFromPropertyPathCache.Add(cacheKey, null);
                    return true;
                }

                fieldInfo = foundField;
                type = fieldInfo.FieldType;
                
                if (i >= parts.Length - 1 || parts[i + 1] != ArrayElementName || !type.IsArrayOrList()) continue;
                i++;
                type = type.GetCollectionElementType();
            }

            return false;
        }
    }
}