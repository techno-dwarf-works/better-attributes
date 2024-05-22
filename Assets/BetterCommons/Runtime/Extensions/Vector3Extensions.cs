using System.Collections.Generic;
using Better.Commons.Runtime.Utility;
using Unity.Collections;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class Vector3Extensions
    {
        public static bool Approximately(this Vector3 self, Vector3 other)
        {
            return Vector3Utility.Approximately(self, other);
        }

        /// <summary>
        /// Multiplies Vector3 and Quaternion
        /// </summary>
        /// <param name="self"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Vector3 Multiply(this Vector3 self, Quaternion quaternion)
        {
            return Vector3Utility.Multiply(self, quaternion);
        }

        public static Vector3 Average(this NativeArray<Vector3> self)
        {
            return Vector3Utility.Average(self);
        }

        public static Vector3 Average(this IEnumerable<Vector3> self)
        {
            return Vector3Utility.Average(self);
        }

        public static Vector3 Flat(this Vector3 self)
        {
            return Vector3Utility.Flat(self);
        }

        public static Vector3 DirectionTo(this Vector3 self, Vector3 to)
        {
            return Vector3Utility.Direction(self, to);
        }

        public static float DistanceTo(this Vector3 self, Vector3 to)
        {
            return Vector3.Distance(self, to);
        }

        public static float SqrDistanceTo(this Vector3 self, Vector3 to)
        {
            return Vector3Utility.SqrDistanceTo(self, to);
        }

        public static Vector3 Abs(this Vector3 self)
        {
            return Vector3Utility.Abs(self);
        }
    }
}