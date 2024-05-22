using Better.Commons.Runtime.Extensions;

namespace Better.Commons.Runtime.Utility
{
    public static class TypeUtility
    {
        public static bool IsNullable<T>()
        {
            var type = typeof(T);
            return type.IsNullable();
        }
        
        public static bool IsAnonymous<T>()
        {
            return typeof(T).IsAnonymous();
        }
    }
}