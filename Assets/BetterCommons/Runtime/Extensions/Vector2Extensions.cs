using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class Vector2Extensions
    {
        public static bool Approximately(this Vector2 self, Vector2 other)
        {
            return Vector2Utility.Approximately(self, other);
        }

        public static Vector2 DirectionTo(this Vector2 self, Vector2 to)
        {
            return Vector2Utility.Direction(self, to);
        }

        public static float DistanceTo(this Vector2 self, Vector2 to)
        {
            return Vector2.Distance(self, to);
        }

        public static float SqrDistanceTo(this Vector2 self, Vector2 to)
        {
            return Vector2Utility.SqrDistanceTo(self, to);
        }

        public static Vector2 Abs(this Vector2 self)
        {
            return Vector2Utility.Abs(self);
        }
    }
}