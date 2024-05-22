using System;
using UnityEngine;

namespace Better.Commons.Runtime.DataStructures.Properties
{
    /// <summary>
    /// A reactive property that notifies subscribers about changes to its value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public class ReactiveProperty<T>
    {
        private event Action<T> ValueChangedEvent;
        private ReadOnlyReactiveProperty<T> _cachedReadOnly;

        [SerializeField] protected T _value;

        /// <summary>
        /// Gets or sets the value of the property. Setting the value notifies all subscribers about the change.
        /// </summary>
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                SetDirty();
            }
        }

        /// <summary>
        /// Initializes a new instance of the ReactiveProperty class with an optional default value.
        /// </summary>
        /// <param name="defaultValue">The initial value of the property.</param>
        public ReactiveProperty(T defaultValue = default)
        {
            _value = defaultValue;
        }

        /// <summary>
        /// Notifies subscribers about a change to the property's value.
        /// </summary>
        public virtual void SetDirty()
        {
            ValueChangedEvent?.Invoke(Value);
        }

        /// <summary>
        /// Subscribes a callback action to be invoked when the property's value changes.
        /// </summary>
        /// <param name="action">The callback action to invoke on value change.</param>
        public virtual void Subscribe(Action<T> action)
        {
            ValueChangedEvent += action;
        }

        /// <summary>
        /// Subscribes a callback action to be invoked immediately and also when the property's value changes in the future.
        /// </summary>
        /// <param name="action">The callback action to invoke.</param>
        public virtual void SubscribeWithInvoke(Action<T> action)
        {
            Subscribe(action);
            action?.Invoke(Value);
        }

        /// <summary>
        /// Unsubscribes a previously subscribed callback action from being invoked when the property's value changes.
        /// </summary>
        /// <param name="action">The callback action to unsubscribe.</param>
        public virtual void Unsubscribe(Action<T> action)
        {
            ValueChangedEvent -= action;
        }

        /// <summary>
        /// Gets a read-only version of the ReactiveProperty, which can be exposed publicly to prevent external modifications.
        /// </summary>
        /// <returns>A ReadOnlyReactiveProperty encapsulating the read-only view of this ReactiveProperty.</returns>
        public ReadOnlyReactiveProperty<T> AsReadOnly()
        {
            _cachedReadOnly ??= new ReadOnlyReactiveProperty<T>(this);
            return _cachedReadOnly;
        }
    }
}
