using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public struct QuaternionUtility
    {
        public static bool IsNormalized(Quaternion quaternion)
        {
            var magnitudeSquared = quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w;
            return Mathf.Approximately(magnitudeSquared, 1.0f);
        }

        public static bool Approximately(Quaternion current, Quaternion other)
        {
            return Mathf.Approximately(Quaternion.Dot(current, other), 1.0f);
        }

        public static Quaternion Scale(Quaternion quaternion, Vector3 scale)
        {
            quaternion.eulerAngles = Vector3.Scale(quaternion.eulerAngles, scale);
            return quaternion;
        }

        public static Quaternion Scale(Quaternion quaternion, float scale)
        {
            return Scale(quaternion, Vector3.one * scale);
        }
    }
}