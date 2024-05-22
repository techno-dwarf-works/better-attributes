using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class ComponentExtensions
    {
        public static void DestroyGameObject(this Component self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.gameObject.Destroy();
        }

        public static void DestroyGameObject(this Component self, float delay)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            self.gameObject.Destroy(delay);
        }

        public static void DestroyGameObject(this IEnumerable<Component> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            var cached = self.ToArray();
            for (var i = 0; i < cached.Length; i++)
            {
                cached[i].DestroyGameObject();
            }
        }

        public static void DestroyGameObject(this IEnumerable<Component> self, float delay)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            var cached = self.ToArray();
            for (var i = 0; i < cached.Length; i++)
            {
                cached[i].DestroyGameObject(delay);
            }
        }

        public static IEnumerable<GameObject> GetGameObjects(this IEnumerable<Component> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<GameObject>();
            }

            return self.Select(c => c.gameObject);
        }

        public static IEnumerable<Transform> GetTransforms(this IEnumerable<Component> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<Transform>();
            }

            return self.Select(c => c.transform);
        }

        public static T GetOrAddComponent<T>(this Component self)
            where T : Component
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            return self.gameObject.GetOrAddComponent<T>();
        }

        public static bool TryGetComponentInParent<T>(this Component self, out T component)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                component = default;
                return false;
            }

            return self.gameObject.TryGetComponentInParent(out component);
        }

        public static bool TryGetComponentInChildren<T>(this Component self, out T component)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                component = default;
                return false;
            }

            return self.gameObject.TryGetComponentInChildren(out component);
        }
    }
}