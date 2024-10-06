using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Better.Attributes.EditorAddons.Drawers.Select
{
    public class KeyTupleComparer : IEqualityComparer<ITuple>
    {
        public bool Equals(ITuple x, ITuple y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return Equals(x[0], y[0]);
        }

        public int GetHashCode(ITuple obj)
        {
            return obj.Length;
        }
    }
}