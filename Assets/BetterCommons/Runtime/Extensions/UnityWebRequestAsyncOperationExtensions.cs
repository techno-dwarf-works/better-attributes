using System;
using Better.Commons.Runtime.Helpers.NotifyCompletions;
using UnityEngine.Networking;

namespace Better.Commons.Runtime.Extensions
{
    public static class UnityWebRequestAsyncOperationExtensions
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new UnityWebRequestAwaiter(self);
        }
    }
}