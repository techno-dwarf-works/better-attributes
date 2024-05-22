using System;
using System.Collections.Generic;

namespace Better.Commons.Runtime.Comparers
{
    public class IsSubClassComparer : BaseComparer<IsSubClassComparer, Type>, IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            var isAssignableFrom = y.IsSubclassOf(x);
            return isAssignableFrom || x == y;
        }

        public int GetHashCode(Type obj)
        {
            return 0;
        }
    }
}