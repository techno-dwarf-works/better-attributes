using UnityEngine;

namespace Better.Commons.Runtime.Utility
{
    public struct Vector2Utility
    {
        public static bool Approximately(Vector2 current, Vector2 other)
        {
            return Mathf.Approximately(current.x, other.x) &&
                   Mathf.Approximately(current.y, other.y);
        }

        public static Vector2 MiddlePoint(Vector2 start, Vector2 end)
        {
            var t = start + end;
            return t / 2;
        }

        public static Vector2 MiddlePoint(Vector2 start, Vector2 end, Vector2 offset)
        {
            var middlePoint = MiddlePoint(start, end);
            return middlePoint + offset;
        }

        public static Vector2 AxesInverseLerp(Vector2 a, Vector2 b, Vector2 value)
        {
            return new Vector2(
                Mathf.InverseLerp(a.x, b.x, value.x),
                Mathf.InverseLerp(a.y, b.y, value.y)
            );
        }

        public static float InverseLerp(Vector2 a, Vector2 b, Vector2 value)
        {
            if (a == b)
            {
                return default;
            }

            var ab = b - a;
            var av = value - a;

            var result = Vector2.Dot(av, ab) / Vector2.Dot(ab, ab);
            return Mathf.Clamp01(result);
        }

        public static Vector2 Direction(Vector2 from, Vector2 to)
        {
            var difference = to - from;
            return difference.normalized;
        }

        public static float SqrDistanceTo(Vector2 from, Vector2 to)
        {
            var difference = to - from;
            return difference.sqrMagnitude;
        }

        public static Vector2 Abs(Vector2 source)
        {
            source.x = Mathf.Abs(source.x);
            source.y = Mathf.Abs(source.y);
            return source;
        }
    }
}