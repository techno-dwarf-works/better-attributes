using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class BoundsExtensions
    {
        public static bool Approximately(this Bounds self, Bounds other)
        {
            return BoundsUtility.Approximately(self, other);
        }
    }
}