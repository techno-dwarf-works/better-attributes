using Better.Commons.Runtime.Extensions;
using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class BoundsUtility
    {
        public static bool Approximately(Bounds current, Bounds other)
        {
            return current.center.Approximately(other.center) &&
                   current.size.Approximately(other.size);
        }
    }
}