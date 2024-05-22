using System;
using Better.Commons.Runtime.Utility;
using UnityEngine;

namespace Better.Commons.EditorAddons.Drawers.Caching
{
    public class HeightCacheValue : CacheValue<float>
    {
        public bool Forced { get; private set; }

        public HeightCacheValue(bool additional, float height)
        {
            IsValid = additional;
            Value = height;
        }

        public HeightCacheValue()
        {
        }

        public HeightCacheValue Force()
        {
            Forced = true;
            return this;
        }

        public static HeightCacheValue GetAdditive(float height)
        {
            var cache = new HeightCacheValue(true, height);
            return cache;
        }

        public static HeightCacheValue GetFull(float height)
        {
            var cache = new HeightCacheValue(false, height);
            return cache;
        }

        public static HeightCacheValue operator +(HeightCacheValue left, HeightCacheValue right)
        {
            if (left == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(left));
                return new HeightCacheValue(false, 0);
            }

            if (right == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(right));
                return new HeightCacheValue(false, 0);
            }

            if (left.Forced)
            {
                return left;
            }

            if (right.Forced)
            {
                return right;
            }

            if (!left.IsValid && !right.IsValid)
            {
                return GetFull(Mathf.Max(left.Value, right.Value));
            }

            if ((!left.IsValid && right.IsValid) || (left.IsValid && !right.IsValid))
            {
                return GetFull(left.Value + right.Value);
            }

            if (left.IsValid && right.IsValid)
            {
                return GetAdditive(left.Value + right.Value);
            }

            return GetAdditive(0);
        }
    }
}