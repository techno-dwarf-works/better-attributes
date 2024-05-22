using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public struct Vector3Utility
    {
        public static bool Approximately(Vector3 current, Vector3 other)
        {
            return Mathf.Approximately(current.x, other.x) &&
                   Mathf.Approximately(current.y, other.y) &&
                   Mathf.Approximately(current.z, other.z);
        }

        /// <summary>
        /// Take projection of point on Plane
        /// </summary>
        /// <param name="planeOffset"></param>
        /// <param name="planeNormal"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Vector3 ClosestPointOnPlane(Vector3 planeOffset, Vector3 planeNormal, Vector3 point)
        {
            return point + DistanceFromPlane(planeOffset, planeNormal, point) * planeNormal;
        }

        /// <summary>
        /// Take Distance from plane to point
        /// </summary>
        /// <param name="planeOffset"></param>
        /// <param name="planeNormal"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static float DistanceFromPlane(Vector3 planeOffset, Vector3 planeNormal, Vector3 point)
        {
            return Vector3.Dot(planeOffset - point, planeNormal);
        }

        public static Vector3 Parabola(Vector3 start, Vector3 end, Vector3 height, float step)
        {
            var middlePoint = MiddlePoint(start, end, -height);
            var point1 = Vector3.Lerp(start, middlePoint, step);
            var point2 = Vector3.Lerp(middlePoint, end, step);
            return Vector3.Lerp(point1, point2, step);
        }

        /// <summary>
        /// Multiplies Vector3 and Quaternion
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="quaternion"></param>
        /// <returns></returns>
        public static Vector3 Multiply(Vector3 vector, Quaternion quaternion)
        {
            // Vector
            var x = vector.x;
            var y = vector.y;
            var z = vector.z;
            // Quaternion
            var qx = quaternion.x;
            var qy = quaternion.y;
            var qz = quaternion.z;
            var qw = quaternion.w;
            // Quaternion * Vector
            var ix = qw * x + qy * z - qz * y;
            var iy = qw * y + qz * x - qx * z;
            var iz = qw * z + qx * y - qy * x;
            var iw = -qx * x - qy * y - qz * z;
            var result = Vector3.zero;
            // Final Quaternion * Vector = Result
            result.x = ix * qw + iw * -qx + iy * -qz - iz * -qy;
            result.y = iy * qw + iw * -qy + iz * -qx - ix * -qz;
            result.z = iz * qw + iw * -qz + ix * -qy - iy * -qx;
            return result;
        }

        public static Vector3 Average(NativeArray<Vector3> vectors)
        {
            var sum = Vector3.zero;
            for (var index = 0; index < vectors.Length; index++)
            {
                var vector = vectors[index];
                sum += vector;
            }

            return sum / vectors.Length;
        }

        public static Vector3 Average(IEnumerable<Vector3> vectors)
        {
            var vectorList = vectors.ToList();
            var sum = Vector3.zero;
            foreach (var vector in vectorList)
            {
                sum += vector;
            }

            return sum / vectorList.Count;
        }

        public static Vector3 Average(params Vector3[] vectors)
        {
            var vectorList = vectors.ToList();
            return Average(vectorList);
        }

        public static Vector3 MiddlePoint(Vector3 start, Vector3 end)
        {
            var t = start + end;
            return t / 2;
        }

        public static Vector3 MiddlePoint(Vector3 start, Vector3 end, Vector3 offset)
        {
            var middlePoint = MiddlePoint(start, end);
            return middlePoint + offset;
        }

        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            if (a == b)
            {
                return default;
            }

            var ab = b - a;
            var av = value - a;

            var result = Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
            return Mathf.Clamp01(result);
        }

        public static Vector3 AxesInverseLerp(Vector3 a, Vector3 b, Vector3 value)
        {
            return new Vector3(
                Mathf.InverseLerp(a.x, b.x, value.x),
                Mathf.InverseLerp(a.y, b.y, value.y),
                Mathf.InverseLerp(a.z, b.z, value.z)
            );
        }
        
        public static Vector3 Flat(Vector3 source)
        {
            source.y = default;
            return source;
        }

        public static Vector3 Direction(Vector3 from, Vector3 to)
        {
            var difference = to - from;
            return difference.normalized;
        }

        public static float SqrDistanceTo(Vector3 from, Vector3 to)
        {
            var difference = to - from;
            return difference.sqrMagnitude;
        }

        public static Vector3 Abs(Vector3 self)
        {
            self.x = Mathf.Abs(self.x);
            self.y = Mathf.Abs(self.y);
            self.z = Mathf.Abs(self.z);
            return self;
        }
    }
}