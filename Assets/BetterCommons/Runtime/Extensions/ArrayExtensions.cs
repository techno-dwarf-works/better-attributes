using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class ArrayExtensions
    {
        public static IEnumerable<TElement> ToEnumerable<TElement>(this Array self)
        {
            if (self == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(self));
                return Enumerable.Empty<TElement>();
            }

            var list = new List<TElement>(self.Length);
            for (int i = 0; i < self.Length; i++)
            {
                if (self.GetValue(i) is TElement element)
                {
                    list.Add(element);
                }
            }

            return list;
        }
    }
}