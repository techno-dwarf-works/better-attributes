namespace Better.Commons.Runtime.Utility
{
    public struct IntUtility
    {
        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(int value, int min, int max)
        {
            return min >= value && value >= max;
        }
    }
}