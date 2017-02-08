using System.Collections;
using System.Collections.Generic;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class TwoWayMap<TKey1, TKey2> : IDictionary<TKey1, TKey2>
    {
        private readonly IDictionary<TKey1, TKey2> _forward = new Dictionary<TKey1, TKey2>();
        private readonly IDictionary<TKey2, TKey1> _reverse = new Dictionary<TKey2, TKey1>();

        /// <summary>
        /// Gets or sets the <see cref="TKey2"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TKey2"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TKey2 this[TKey1 key]
        {
            get { return _forward[key]; }
            set
            {
                _forward[key] = value;
                _reverse[value] = key;
            }
        }


        /// <summary>
        /// Gets or sets the <see cref="TKey1"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TKey1"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TKey1 this[TKey2 key]
        {
            get {  return _reverse[key];  }

            set
            {
                _reverse[key] = value;
                _forward[value] = key;
            }
        }


        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count => _forward.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public ICollection<TKey1> Keys => _forward.Keys;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        public ICollection<TKey2> Values => _reverse.Keys;


        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<TKey1, TKey2> item)
        {
            _forward.Add(item.Key,item.Value);
            _reverse.Add(item.Value, item.Key);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<TKey2, TKey1> item)
        {
            _reverse.Add(item.Key, item.Value);
            _forward.Add(item.Value, item.Key);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey2 key, TKey1 value)
        {
            _reverse.Add(key, value);
            _forward.Add(value, key);
        }


        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(TKey1 key, TKey2 value)
        {
            _forward.Add(key, value);
            _reverse.Add(value, key);
        }


        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey1 key)
        {
            return _forward.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey2 key)
        {
            return _reverse.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyValuePair<TKey2, TKey1> item)
        {
            return _reverse.ContainsKey(item.Key) && _forward.ContainsKey(item.Value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey1, TKey2> item)
        {
            return _forward.ContainsKey(item.Key) && _reverse.ContainsKey(item.Value);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey2, TKey1>[] array, int arrayIndex)
        {
            _reverse.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<TKey1, TKey2>[] array, int arrayIndex)
        {
            _forward.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey1, TKey2>> GetEnumerator()
        {
            return _forward.GetEnumerator();
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(KeyValuePair<TKey1, TKey2> item)
        {
            return _reverse.Remove(item.Value) | _forward.Remove(item.Key);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey2, TKey1> item)
        {
            return _forward.Remove(item.Value) | _reverse.Remove(item.Key);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(TKey2 key)
        {
            TKey1 value;
            if (!TryGetValue(key, out value))
                return false;
            return _forward.Remove(value) | _reverse.Remove(key);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        public bool Remove(TKey1 key)
        {
            TKey2 value;
            if (!TryGetValue(key, out value))
                return false;
            return _reverse.Remove(value) | _forward.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey1 key, out TKey2 value)
        {
            return _forward.TryGetValue(key, out value);
        }

        public bool TryGetValue(TKey2 key, out TKey1 value)
        {
            return _reverse.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
