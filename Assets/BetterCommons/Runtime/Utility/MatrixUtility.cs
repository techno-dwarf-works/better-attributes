using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public struct MatrixUtility
    {
        /// <summary>
        ///   <para>Transforms position from world space to local space.</para>
        /// </summary>
        public static Vector3 InverseTransformPoint(Vector3 point, Vector3 position, Quaternion rotation,
            Vector3 lossyScale)
        {
            var source = new Vector3(1 / lossyScale.x, 1 / lossyScale.y, 1 / lossyScale.z);
            var scaleFactor = Quaternion.Inverse(rotation) * (point - position);
            return Vector3.Scale(source, scaleFactor);
        }

        /// <summary>
        ///   <para>Transforms vector from world space to local space.</para>
        /// </summary>
        public static Vector3 InverseTransformVector(Vector3 position, Quaternion rotation, Vector3 localScale,
            Vector3 vector3)
        {
            var initialMatrix = Matrix4x4.TRS(position, rotation, localScale).inverse;
            var scaleMatrix = Matrix4x4.Scale(vector3);
            var transformed = initialMatrix * scaleMatrix;
            return transformed.lossyScale;
        }

        /// <summary>
        ///   <para>Transforms position from local space to world space.</para>
        /// </summary>
        public static Vector3 TransformPoint(Vector3 point, Vector3 position, Quaternion rotation, Vector3 lossyScale)
        {
            return position + rotation * Vector3.Scale(lossyScale, point);
        }
    }
}