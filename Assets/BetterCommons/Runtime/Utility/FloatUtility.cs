namespace Better.Commons.Runtime.Utility
{
    public struct FloatUtility
    {
        /// <summary>
        /// Check that point in two other points
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(float value, float min, float max)
        {
            return min >= value && value >= max;
        }
    }
}