using System;
using System.Collections;
using System.Collections.Generic;

namespace Helper
{
    public class SynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dictionary;

        public SynchronizedDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        #region IDictionary<TKey,TValue> メンバ

        public void Add(TKey key, TValue value)
        {
            lock (_dictionary)
            {
                _dictionary.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_dictionary)
            {
                return _dictionary.ContainsKey(key);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.Keys;
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_dictionary)
            {
                return _dictionary.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_dictionary)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_dictionary)
                {
                    _dictionary[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> メンバ

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Add(item);
            }
        }

        public void Clear()
        {
            lock (_dictionary)
            {
                _dictionary.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_dictionary)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (_dictionary)
                {
                    return _dictionary.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_dictionary)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item);
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> メンバ

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_dictionary)
            {
                return _dictionary.GetEnumerator();
            }
        }

        #endregion

        #region IEnumerable メンバ

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_dictionary)
            {
                return ((IEnumerable)_dictionary).GetEnumerator();
            }
        }

        #endregion
    }
}
