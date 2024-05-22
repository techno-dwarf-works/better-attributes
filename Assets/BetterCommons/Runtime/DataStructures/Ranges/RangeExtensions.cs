using System;
using UnityEngine;

namespace Better.Commons.Runtime.DataStructures.Ranges
{
    /// <summary>
    /// Provides extension methods for Range of different types to perform common operations.
    /// </summary>
    public static class RangeExtensions
    {
        /// <summary>
        /// Clamps an integer value to the range defined by the Range object.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static int Clamp(this Range<int> self, int value)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return value;
            }
            
            return Mathf.Clamp(value, self.Min, self.Max);
        }

        /// <summary>
        /// Clamps a float value to the range defined by the Range object.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(this Range<float> self, float value)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return value;
            }
            
            return Mathf.Clamp(value, self.Min, self.Max);
        }

        /// <summary>
        /// Returns a random float value within the range defined by the Range object.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <returns>A random float value within the range.</returns>
        public static float Random(this Range<float> self)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
            return UnityEngine.Random.Range(self.Min, self.Max);
        }

        /// <summary>
        /// Returns a random integer value within the range defined by the Range object.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <returns>A random integer value within the range.</returns>
        public static int Random(this Range<int> self)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
            return UnityEngine.Random.Range(self.Min, self.Max + 1); // +1 to include the maximum value in the range.
        }

        /// <summary>
        /// Linearly interpolates between the minimum and maximum values of the range based on the given interpolation parameter.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="t">The interpolation parameter.</param>
        /// <returns>The interpolated float value.</returns>
        public static float Lerp(this Range<float> self, float t)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
            return Mathf.Lerp(self.Min, self.Max, t);
        }

        /// <summary>
        /// Linearly interpolates between the minimum and maximum values of the range based on the given interpolation parameter and rounds the result to the nearest integer.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="t">The interpolation parameter.</param>
        /// <returns>The interpolated and rounded integer value.</returns>
        public static int Lerp(this Range<int> self, float t)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
            return Mathf.RoundToInt(Mathf.Lerp(self.Min, self.Max, t));
        }

        /// <summary>
        /// Checks if a float value is contained within the range defined by the Range object.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is within the range; otherwise, false.</returns>
        public static bool Contains(this Range<float> self, float value)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
            return value >= self.Min && value <= self.Max;
        }

        /// <summary>
        /// Checks if an integer value is contained within the range defined by the Range object.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is within the range; otherwise, false.</returns>
        public static bool Contains(this Range<int> self, int value)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
            return value >= self.Min && value <= self.Max;
        }

        /// <summary>
        /// Clamps a double value to the range defined by the Range object. Uses Math.Clamp if available.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static double Clamp(this Range<double> self, double value)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
#if UNITY_2021_3_OR_NEWER
            return Math.Clamp(value, self.Min, self.Max);
#else
            if (value < range.Min)
                value = range.Min;
            else if (value > range.Max)
                value = range.Max;
            return value;
#endif
        }

        /// <summary>
        /// Clamps a byte value to the range defined by the Range object. Uses Math.Clamp if available.
        /// </summary>
        /// <param name="self">The Range object.</param>
        /// <param name="value">The value to clamp.</param>
        /// <returns>The clamped value.</returns>
        public static byte Clamp(this Range<byte> self, byte value)
        {
            if (self == null)
            {
                Debug.LogException(new ArgumentException(nameof(self)));
                return default;
            }
            
#if UNITY_2021_3_OR_NEWER
            return Math.Clamp(value, self.Min, self.Max);
#else
            if (value < range.Min)
                value = range.Min;
            else if (value > range.Max)
                value = range.Max;
            return value;
#endif
        }
    }
}
