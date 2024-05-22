using System;
using Better.Commons.Runtime.Helpers.NotifyCompletions;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class ResourceRequestExtensions
    {
        public static ResourceRequestAwaiter GetAwaiter(this ResourceRequest self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new ResourceRequestAwaiter(self);
        }
    }
}