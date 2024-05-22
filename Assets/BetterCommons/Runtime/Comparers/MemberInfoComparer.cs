using System.Collections.Generic;
using System.Reflection;

namespace Better.Commons.Runtime.Comparers
{
    internal class MemberInfoComparer : BaseComparer<MemberInfoComparer, MemberInfo>, IEqualityComparer<MemberInfo>
    {
        public bool Equals(MemberInfo x, MemberInfo y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return x.Name == y.Name && x.DeclaringType == y.DeclaringType;
        }

        public int GetHashCode(MemberInfo obj)
        {
            unchecked
            {
                var hashCode = obj.Name.GetHashCode();
                if (obj.DeclaringType != null) hashCode = (hashCode * 397) ^ obj.DeclaringType.GetHashCode();
                return hashCode;
            }
        }
    }
}