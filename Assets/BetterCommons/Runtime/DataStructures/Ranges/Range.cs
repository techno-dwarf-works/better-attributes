using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Better.Commons.Runtime.DataStructures.Ranges
{
    /// <summary>
    /// Represents a range with minimum and maximum values of a generic type.
    /// </summary>
    /// <typeparam name="T">The type of the values defining the range.</typeparam>
    [Serializable]
    public class Range<T> : IEquatable<Range<T>>
    {
        [FormerlySerializedAs("min")] [SerializeField] private T _min;
        [FormerlySerializedAs("max")] [SerializeField] private T _max;

        /// <summary>
        /// Initializes a new instance of the Range class with default minimum and maximum values.
        /// </summary>
        public Range()
        {
            _min = default;
            _max = default;
        }

        /// <summary>
        /// Initializes a new instance of the Range class by copying another range.
        /// </summary>
        /// <param name="range">The range to copy.</param>
        public Range(Range<T> range)
        {
            _min = range.Min;
            _max = range.Max;
        }

        /// <summary>
        /// Initializes a new instance of the Range class with specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        public Range(T min, T max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Gets the minimum value of the range.
        /// </summary>
        public T Min => _min;

        /// <summary>
        /// Gets the maximum value of the range.
        /// </summary>
        public T Max => _max;

        /// <summary>
        /// Determines whether the specified Range is equal to the current Range.
        /// </summary>
        /// <param name="other">The Range to compare with the current Range.</param>
        /// <returns>true if the specified Range is equal to the current Range; otherwise, false.</returns>
        public bool Equals(Range<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(_min, other._min) && EqualityComparer<T>.Default.Equals(_max, other._max);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current Range.
        /// </summary>
        /// <param name="obj">The object to compare with the current Range.</param>
        /// <returns>true if the specified object is equal to the current Range; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Range<T>)obj);
        }

        /// <summary>
        /// Creates a new instance of the Range that is a copy of the current Range.
        /// </summary>
        /// <returns>A new Range instance that is a copy of this Range.</returns>
        public Range<T> Copy()
        {
            return new Range<T>(_min, _max);
        }

        /// <summary>
        /// Creates a new instance of the Range with the same minimum value as this instance and a new maximum value.
        /// </summary>
        /// <param name="maxValue">The new maximum value for the range.</param>
        /// <returns>A new Range instance with the updated maximum value while retaining the original minimum value.</returns>
        public Range<T> CopyWithMax(T maxValue)
        {
            return new Range<T>(_min, maxValue);
        }

        /// <summary>
        /// Creates a new instance of the Range with the same maximum value as this instance and a new minimum value.
        /// </summary>
        /// <param name="minValue">The new minimum value for the range.</param>
        /// <returns>A new Range instance with the updated minimum value while retaining the original maximum value.</returns>
        public Range<T> CopyWithMin(T minValue)
        {
            return new Range<T>(minValue, _max);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current Range.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(_min) * 397) ^ EqualityComparer<T>.Default.GetHashCode(_max);
            }
        }
    }
}
