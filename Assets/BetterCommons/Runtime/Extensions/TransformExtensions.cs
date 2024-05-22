using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class TransformExtensions
    {
        public static Transform CreateChild(this Transform self, string name)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            var created = new GameObject(name);
            created.transform.parent = self;
            created.transform.LocalReset();
            return created.transform;
        }

        public static Transform CreateChild(this Transform self)
        {
            var name = "GameObject (new)";
            return self.CreateChild(name);
        }

        /// <summary>
        /// Copies rotation and position from another transform
        /// </summary>
        public static void Copy(this Transform self, Transform root)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (root == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(root));
                return;
            }

            self.position = root.position;
            self.rotation = root.rotation;
        }

        /// <summary>
        /// Copies localPosition, localRotation and optional - localScale from another transform
        /// </summary>
        public static void LocalCopy(this Transform self, Transform root, bool scaleCopy = true)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (root == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(root));
                return;
            }

            self.localPosition = root.localPosition;
            self.localRotation = root.localRotation;

            if (scaleCopy)
            {
                self.localScale = root.localScale;
            }
        }

        public static void CopyPosition(this Transform self, Transform root)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (root == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(root));
                return;
            }

            self.position = root.position;
        }

        public static void CopyLocalPosition(this Transform self, Transform root)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (root == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(root));
                return;
            }

            self.localPosition = root.localPosition;
        }

        public static void CopyRotation(this Transform self, Transform root)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (root == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(root));
                return;
            }

            self.rotation = root.rotation;
        }

        public static void CopyLocalRotation(this Transform self, Transform root)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (root == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(root));
                return;
            }

            self.localRotation = root.localRotation;
        }

        /// <summary>
        /// Resets rotation (identity) and position (0, 0, 0) relative to world space
        /// </summary>
        public static void Reset(this Transform self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.position = Vector3.zero;
            self.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Resets rotation (identity), position (0, 0, 0)
        /// and optional - scale (1, 1, 1), relative to local space
        /// </summary>
        public static void LocalReset(this Transform self, bool scaleReset = true)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.localPosition = Vector3.zero;
            self.localRotation = Quaternion.identity;

            if (scaleReset)
            {
                self.localScale = Vector3.one;
            }
        }

        public static IEnumerable<Transform> GetChildren(this Transform self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<Transform>();
            }

            var result = new List<Transform>();
            for (var i = 0; i < self.childCount; i++)
            {
                result.Add(self.GetChild(i));
            }

            return result;
        }

        public static void DestroyChildren(this Transform self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.GetChildren()
                .GetGameObjects()
                .Destroy();
        }

        public static void DestroyChildren(this Transform self, float delay)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.GetChildren()
                .GetGameObjects()
                .Destroy(delay);
        }
    }
}