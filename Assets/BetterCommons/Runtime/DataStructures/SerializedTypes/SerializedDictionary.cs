using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Better.Commons.Runtime.DataStructures.SerializedTypes
{
    /// <summary>
    /// A serializable dictionary that can be used with Unity's serialization system.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [Serializable]
    public class SerializedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver, IReadOnlyDictionary<TKey, TValue>
    {
        [SerializeField] private List<TKey> _keys; // Serialized list of keys
        [SerializeField] private List<TValue> _values; // Serialized list of values

        private Dictionary<TKey, TValue> _dictionary = new(); // The underlying dictionary

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        /// <summary>
        /// Readonly collection of dictionary keys.
        /// </summary>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        /// <summary>
        /// Readonly collection of dictionary values.
        /// </summary>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        /// <summary>
        /// Gets an ICollection containing the keys of the Dictionary.
        /// </summary>
        public ICollection<TKey> Keys => _dictionary.Keys;

        /// <summary>
        /// Gets an ICollection containing the values of the Dictionary.
        /// </summary>
        public ICollection<TValue> Values => _dictionary.Values;

        /// <summary>
        /// Gets the number of key/value pairs contained in the Dictionary.
        /// </summary>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Gets a value indicating whether the Dictionary is read-only.
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).IsReadOnly;

        /// <summary>
        /// Gets the key associated with the specified value.
        /// </summary>
        /// <param name="value">The value to find the key of.</param>
        /// <returns>The key associated with the specified value.</returns>
        public TKey this[TValue value]
        {
            get
            {
                _keys = new List<TKey>(_dictionary.Keys);
                _values = new List<TValue>(_dictionary.Values);
                var index = _values.FindIndex(x => EqualityComparer<TValue>.Default.Equals(x, value));

                if (index < 0)
                {
                    throw new KeyNotFoundException();
                }

                return _keys[index];
            }
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        /// <summary>
        /// Determines whether the Dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the Dictionary.</param>
        /// <returns>true if the Dictionary contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the Dictionary.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false.</returns>
        public bool Remove(TKey key)
        {
            return _dictionary.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the object that implements IDictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Removes all keys and values from the Dictionary.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Adds a key/value pair to the ICollection.
        /// </summary>
        /// <param name="item">The key/value pair to add to the ICollection.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        /// <summary>
        /// Determines whether the ICollection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the ICollection.</param>
        /// <returns>true if item is found in the ICollection; otherwise, false.</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ICollection.
        /// </summary>
        /// <param name="item">The object to remove from the ICollection.</param>
        /// <returns>true if item was successfully removed from the ICollection; otherwise, false.</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return (_dictionary as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// Called before Unity serializes this object.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _keys = new List<TKey>(_dictionary.Keys);
            _values = new List<TValue>(_dictionary.Values);
        }

        /// <summary>
        /// Called after Unity deserializes this object.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Debug.Assert(_keys.Count == _values.Count);
            Clear();

            for (var i = 0; i < _keys.Count; ++i)
            {
                Add(_keys[i], _values[i]);
            }
        }
    }
}