using UnityEngine;

namespace Better.Commons.Runtime.Extensions
{
    public static class RectExtensions
    {
        public static float GetRatio(this Rect self)
        {
            return self.size.y / self.size.y;
        }
    }
}