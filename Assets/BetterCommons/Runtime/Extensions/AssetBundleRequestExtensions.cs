using System;
using Better.Commons.Runtime.Helpers.NotifyCompletions;
using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class AssetBundleRequestExtensions
    {
        public static AssetBundleRequestAwaiter GetAwaiter(this AssetBundleRequest self)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return new AssetBundleRequestAwaiter(self);
        }
    }
}