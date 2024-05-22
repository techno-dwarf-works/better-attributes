using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class TimeUtility
    {
        public static int SecondsToMilliseconds(float seconds)
        {
            return Mathf.RoundToInt(seconds * 1000);
        }
    }
}