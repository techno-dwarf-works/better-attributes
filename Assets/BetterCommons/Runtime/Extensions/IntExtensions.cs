using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="self"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool InRange(this int self, int min, int max)
        {
            return IntUtility.InRange(self, min, max);
        }
    }
}