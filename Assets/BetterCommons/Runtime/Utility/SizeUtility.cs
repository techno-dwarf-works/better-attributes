using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public static class SizeUtility
    {
        private const float ConvertRation = 1024f;

        public enum Units : long
        {
            Byte = 0,
            Kb = 1,
            Mb = 2,
            Gb = 3,
            Tb = 4,
            Pb = 5,
            Eb = 6,
            Zb = 7,
            Yb = 8
        }

        public static double ToSize(ulong value, Units unit)
        {
            return value / Mathf.Pow(ConvertRation, (long)unit);
        }

        public static double ToSize(long value, Units unit)
        {
            return value / Mathf.Pow(ConvertRation, (long)unit);
        }

        public static double ToSize(double value, Units unit)
        {
            return value / Mathf.Pow(ConvertRation, (long)unit);
        }
    }
}