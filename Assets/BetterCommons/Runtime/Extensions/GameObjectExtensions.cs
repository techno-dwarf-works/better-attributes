using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class GameObjectExtensions
    {
        public static void SetActive(this IEnumerable<GameObject> self, bool value)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            var cached = self.ToArray();
            for (var i = 0; i < cached.Length; i++)
            {
                cached[i].SetActive(value);
            }
        }

        public static T GetOrAddComponent<T>(this GameObject self)
            where T : Component
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return default;
            }

            if (self.TryGetComponent(out T component))
            {
                return component;
            }

            return self.AddComponent<T>();
        }

        public static bool TryGetComponentInParent<T>(this GameObject self, out T component)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                component = default;
                return false;
            }

            component = self.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this GameObject self, out T component)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                component = default;
                return false;
            }

            component = self.GetComponentInChildren<T>();
            return component != null;
        }

        public static void RecursiveSetLayer(this GameObject self, int layer)
        {
            self.layer = layer;

            foreach (Transform child in self.transform)
            {
                RecursiveSetLayer(child.gameObject, layer);
            }
        }

        public static void RecursiveSetLayer(this GameObject self, LayerMask layerMask)
        {
            self.RecursiveSetLayer(layerMask.value);
        }
    }
}