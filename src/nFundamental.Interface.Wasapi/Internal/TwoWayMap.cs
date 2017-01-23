using System.Collections;
using System.Collections.Generic;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class TwoWayMap<TKey1, TKey2> : IDictionary<TKey1, TKey2>
    {
        private readonly IDictionary<TKey1, TKey2> _forward = new Dictionary<TKey1, TKey2>();
        private readonly IDictionary<TKey2, TKey1> _reverse = new Dictionary<TKey2, TKey1>();

        public TKey2 this[TKey1 key]
        {
            get { return _forward[key]; }
            set
            {
                _forward[key] = value;
                _reverse[value] = key;
            }
        }
       

        public TKey1 this[TKey2 key]
        {
            get {  return _reverse[key];  }

            set
            {
                _reverse[key] = value;
                _forward[value] = key;
            }
        }


        public int Count => _forward.Count;

        public bool IsReadOnly => false;

        public ICollection<TKey1> Keys => _forward.Keys;


        public ICollection<TKey2> Values => _reverse.Keys;

   
        public void Add(KeyValuePair<TKey1, TKey2> item)
        {
            _forward.Add(item.Key,item.Value);
            _reverse.Add(item.Value, item.Key);
        }

        public void Add(KeyValuePair<TKey2, TKey1> item)
        {
            _reverse.Add(item.Key, item.Value);
            _forward.Add(item.Value, item.Key);
        }

        public void Add(TKey2 key, TKey1 value)
        {
            _reverse.Add(key, value);
            _forward.Add(value, key);
        }


        public void Add(TKey1 key, TKey2 value)
        {
            _forward.Add(key, value);
            _reverse.Add(value, key);
        }


        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        public bool ContainsKey(TKey1 key)
        {
            return _forward.ContainsKey(key);
        }

        public bool ContainsKey(TKey2 key)
        {
            return _reverse.ContainsKey(key);
        }
        public bool Contains(KeyValuePair<TKey2, TKey1> item)
        {
            return _reverse.ContainsKey(item.Key) && _forward.ContainsKey(item.Value);
        }

        public bool Contains(KeyValuePair<TKey1, TKey2> item)
        {
            return _forward.ContainsKey(item.Key) && _reverse.ContainsKey(item.Value);
        }

        public void CopyTo(KeyValuePair<TKey2, TKey1>[] array, int arrayIndex)
        {
            _reverse.CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<TKey1, TKey2>[] array, int arrayIndex)
        {
            _forward.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey1, TKey2>> GetEnumerator()
        {
            return _forward.GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey1, TKey2> item)
        {
            return _reverse.Remove(item.Value) | _forward.Remove(item.Key);
        }

        public bool Remove(KeyValuePair<TKey2, TKey1> item)
        {
            return _forward.Remove(item.Value) | _reverse.Remove(item.Key);
        }

        public bool Remove(TKey2 key)
        {
            TKey1 value;
            if (!TryGetValue(key, out value))
                return false;
            return _forward.Remove(value) | _reverse.Remove(key);
        }

        public bool Remove(TKey1 key)
        {
            TKey2 value;
            if (!TryGetValue(key, out value))
                return false;
            return _reverse.Remove(value) | _forward.Remove(key);
        }

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
