using System;

namespace Better.Commons.Runtime.DataStructures.Properties
{
    /// <summary>
    /// Represents a read-only wrapper around a ReactiveProperty, allowing subscription to value changes without the ability to modify the value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public sealed class ReadOnlyReactiveProperty<T>
    {
        private readonly ReactiveProperty<T> _source;

        /// <summary>
        /// Gets the current value of the underlying ReactiveProperty.
        /// </summary>
        public T Value => _source.Value;

        /// <summary>
        /// Initializes a new instance of the ReadOnlyReactiveProperty class with a ReactiveProperty as its source.
        /// </summary>
        /// <param name="source">The ReactiveProperty to encapsulate in a read-only manner.</param>
        public ReadOnlyReactiveProperty(ReactiveProperty<T> source)
        {
            if (source == null)
            {
                throw new ArgumentException(nameof(source));
            }

            _source = source;
        }

        /// <summary>
        /// Subscribes a callback action to be invoked when the underlying ReactiveProperty's value changes.
        /// </summary>
        /// <param name="action">The callback action to invoke on value change.</param>
        public void Subscribe(Action<T> action) => _source.Subscribe(action);

        /// <summary>
        /// Subscribes a callback action to be invoked immediately with the current value, and also when the underlying ReactiveProperty's value changes in the future.
        /// </summary>
        /// <param name="action">The callback action to invoke.</param>
        public void SubscribeWithInvoke(Action<T> action) => _source.SubscribeWithInvoke(action);

        /// <summary>
        /// Unsubscribes a previously subscribed callback action from being invoked when the underlying ReactiveProperty's value changes.
        /// </summary>
        /// <param name="action">The callback action to unsubscribe.</param>
        public void Unsubscribe(Action<T> action) => _source.Unsubscribe(action);
    }
}