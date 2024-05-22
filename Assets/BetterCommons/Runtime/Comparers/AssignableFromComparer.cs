using System;
using System.Collections.Generic;

namespace Better.Commons.Runtime.Comparers
{
    public class AssignableFromComparer : BaseComparer<AssignableFromComparer, Type>, IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            var isAssignableFrom = x.IsAssignableFrom(y);
            return isAssignableFrom || x == y;
        }

        public int GetHashCode(Type obj)
        {
            return 0;
        }
    }
}