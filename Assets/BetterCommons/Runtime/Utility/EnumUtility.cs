using System;
using System.Collections.Generic;
using System.Linq;
using Better.Commons.Runtime.Extensions;

namespace Better.Commons.Runtime.Utility
{
    public static class EnumUtility
    {
        public const int DefaultIntFlag = 0;

        public static IEnumerable<Enum> GetAllValues(Type enumType)
        {
            if (enumType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(enumType));
                return Enumerable.Empty<Enum>();
            }

            if (!enumType.IsEnum())
            {
                var message = $"{nameof(enumType)} must be provided {typeof(Enum)}";
                DebugUtility.LogException<ArgumentException>(message);
                return Enumerable.Empty<Enum>();
            }

            var values = Enum.GetValues(enumType);
            return values.ToEnumerable<Enum>();
        }

        public static IEnumerable<TEnum> GetAllValues<TEnum>()
            where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            return GetAllValues(enumType).Cast<TEnum>();
        }

        public static Enum EverythingFlag(Type enumType)
        {
            if (enumType == null)
            {
                DebugUtility.LogException<ArgumentNullException>(nameof(enumType));
                return default;
            }

            if (!enumType.IsEnum())
            {
                var message = $"{nameof(enumType)} must be provided {typeof(Enum)}";
                DebugUtility.LogException<ArgumentException>(message);
                return default;
            }

            long newValue = 0;
            var values = GetAllValues(enumType);
            foreach (var value in values)
            {
                long v = (long)Convert.ChangeType(value, TypeCode.Int64);
                if (v == 1 || v % 2 == 0)
                {
                    newValue |= v;
                }
            }

            return (Enum)Enum.ToObject(enumType, newValue);
        }

        public static TEnum EverythingFlag<TEnum>()
            where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            return (TEnum)EverythingFlag(enumType);
        }

        public static Enum Add(Enum a, Enum b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            if (a.GetType() != b.GetType())
            {
                throw new Exception($"Type of {a.ToString()} and {b.ToString()} is different");
            }

            return (Enum)Enum.ToObject(a.GetType(), a.ToFlagInt() | b.ToFlagInt());
        }

        public static Enum Remove(Enum a, Enum b)
        {
            if (a == null)
            {
                throw new ArgumentNullException(nameof(a));
            }

            if (b == null)
            {
                throw new ArgumentNullException(nameof(b));
            }
            
            if (a.GetType() != b.GetType())
            {
                throw new Exception($"Type of {a.ToString()} and {b.ToString()} is different");
            }

            return (Enum)Enum.ToObject(a.GetType(), a.ToFlagInt() & ~b.ToFlagInt());
        }

        public static Enum ToEnum(Type enumType, int value)
        {
            return (Enum)Enum.ToObject(enumType, value);
        }

        public static bool HasValue(Enum currentValue, Enum value, bool isFlag)
        {
            if (isFlag)
            {
                return currentValue.HasFlag(value);
            }

            return Equals(currentValue, value);
        }
    }
}