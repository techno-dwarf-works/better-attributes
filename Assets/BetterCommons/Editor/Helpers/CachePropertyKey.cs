using System;
using Better.Commons.Runtime.Extensions;

namespace Better.Commons.EditorAddons.Helpers
{
    public struct CachePropertyKey : IEquatable<CachePropertyKey>
    {
        private readonly Type _type;
        private readonly string _propertyPath;

        public CachePropertyKey(Type type, string propertyPath)
        {
            _type = type;
            _propertyPath = propertyPath;
        }

        public bool Equals(CachePropertyKey other)
        {
            return _type == other._type && _propertyPath.CompareOrdinal(other._propertyPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CachePropertyKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_type != null ? _type.GetHashCode() : 0) * 397) ^ (_propertyPath != null ? _propertyPath.GetHashCode() : 0);
            }
        }
    }
}