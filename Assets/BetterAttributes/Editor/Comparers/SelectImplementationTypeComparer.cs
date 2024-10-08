﻿using System;
using System.Collections.Generic;
using Better.Commons.Runtime.Comparers;

namespace Better.Attributes.EditorAddons.Comparers
{
    public class SelectImplementationTypeComparer : BaseComparer<SelectImplementationTypeComparer, Type>, IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.IsAssignableFrom(y) || x == y) return true;
            if ((y.IsInterface || y.IsAbstract) && x == typeof(Type)) return true;
            return x == typeof(Type);
        }

        public int GetHashCode(Type obj)
        {
            return 0;
        }
    }
}