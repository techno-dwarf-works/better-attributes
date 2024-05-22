using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="self"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool InRange(this float self, float min, float max)
        {
            return FloatUtility.InRange(self, min, max);
        }
    }
}