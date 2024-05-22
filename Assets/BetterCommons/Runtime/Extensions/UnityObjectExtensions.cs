using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;
using Object = UnityEngine.Object;

namespace Better.Commons.Runtime.Extensions
{
    public static class UnityObjectExtensions
    {
        public static bool IsNullOrDestroyed(this Object self)
        {
            if (ReferenceEquals(self, null))
            {
                return true;
            }

            return self == null;
        }

        /// <summary>
        /// Gets object name and converts into CamelCase
        /// </summary>
        /// <param name="self"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string GetPrettyObjectName(this Object self, params string[] remove)
        {
            if (self.IsNullOrDestroyed())
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return string.Empty;
            }

            if (remove != null)
            {
                foreach (var value in remove)
                {
                    self.name = self.name.Replace(value, string.Empty);
                }
            }

            return self.name.PrettyCamelCase();
        }

        public static void Destroy(this Object self)
        {
            if (self.IsNullOrDestroyed())
            {
                var message = $"{nameof(self)} already null or destroyed";
                DebugUtility.LogException<ArgumentException>(message);
                return;
            }

            Object.Destroy(self);
        }

        public static void Destroy(this Object self, float delay)
        {
            if (self.IsNullOrDestroyed())
            {
                var message = $"{nameof(self)} already null or destroyed";
                DebugUtility.LogException<ArgumentException>(message);
                return;
            }

            delay = MathF.Max(delay, 0f);
            Object.Destroy(self, delay);
        }

        public static void Destroy(this IEnumerable<Object> self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            var cached = self.ToArray();
            for (var i = 0; i < cached.Length; i++)
            {
                cached[i].Destroy();
            }
        }

        public static void Destroy(this IEnumerable<Object> self, float delay)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }
            
            var cached = self.ToArray();
            for (var i = 0; i < cached.Length; i++)
            {
                cached[i].Destroy(delay);
            }
        }
    }
}