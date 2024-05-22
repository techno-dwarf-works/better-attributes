using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class ScreenUtility
    {
        public static Rect GetScreenBounds()
        {
            var rect = new Rect(0f, 0f, Screen.width, Screen.height);
            return rect;
        }
    }
}