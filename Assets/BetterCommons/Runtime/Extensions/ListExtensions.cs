using System;
using System.Collections.Generic;
using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class ListExtensions
    {
        public static bool RemoveRange<T>(this List<T> self, IEnumerable<T> range)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            if (range == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(range));
                return false;
            }

            var anyRemoved = false;
            foreach (var item in range)
            {
                if (self.Remove(item))
                {
                    anyRemoved = true;
                }
            }

            return anyRemoved;
        }

        public static void AddRangeDistinct<T>(this List<T> self, IEnumerable<T> range)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return;
            }

            if (range == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(range));
                return;
            }

            foreach (var item in range)
            {
                if (self.Contains(item)) continue;
                self.Add(item);
            }
        }

        public static bool TrimToCount<T>(this List<T> self, int count)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return false;
            }

            count = Math.Max(count, 0);

            if (self.Count > count)
            {
                var difference = self.Count - count;
                self.RemoveRange(count, difference);
                return true;
            }

            return false;
        }
    }
}