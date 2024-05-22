using Better.Commons.Runtime.Utility;

namespace Better.Commons.Runtime.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNullable<T>(this T self)
        {
            return TypeUtility.IsNullable<T>();
        }
    }
}