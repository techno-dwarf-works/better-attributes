using System;
using System.Collections.Generic;

namespace Better.Commons.Runtime.Comparers
{
    public class TypeComparer : BaseComparer<TypeComparer, Type>, IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.IsAssignableFrom(y) || x == y) return true;
            return (y.IsInterface || y.IsAbstract) && x == typeof(Type);
        }

        public int GetHashCode(Type obj)
        {
            return 0;
        }
    }
}