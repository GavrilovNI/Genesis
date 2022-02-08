using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Utils
{
    public class BidirectionalDictrionary<TKey, TValue> where TKey : notnull where TValue : notnull
    {
        private Dictionary<TKey, TValue> _forward;
        private Dictionary<TValue, TKey> _reverse;

        public int Count => _forward.Count;

        public BidirectionalDictrionary()
        {
            _forward = new Dictionary<TKey, TValue>();
            _reverse = new Dictionary<TValue, TKey>();
        }
        public BidirectionalDictrionary(BidirectionalDictrionary<TKey, TValue> other)
        {
            _forward = other._forward.ToDictionary(entry => entry.Key, entry => entry.Value);
            _reverse = other._reverse.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        private BidirectionalDictrionary(Dictionary<TKey, TValue> forward, Dictionary<TValue, TKey> reverse)
        {
            _forward = forward;
            _reverse = reverse;
        }

        public TValue this[TKey index]
        {
            get
            {
                return _forward[index];
            }
            set
            {
                if(_forward.TryGetValue(index, out TValue? oldValue))
                {
                    _forward.Remove(index);
                    _reverse.Remove(oldValue);
                }
                if (_reverse.TryGetValue(value, out TKey? oldKey))
                {
                    _forward.Remove(oldKey);
                    _reverse.Remove(value);
                }

                _forward[index] = value;
                _reverse[value] = index;
            }
        }

        public void Add(TKey t1, TValue t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public void Clear()
        {
            _forward.Clear();
            _reverse.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            return _forward.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _reverse.ContainsKey(value);
        }

        public bool Remove(TKey key)
        {
            if(_forward.Remove(key, out TValue? value))
            {
                _reverse.Remove(value);
                return true;
            }
            return false;
        }

        public bool Remove(TKey key, out TValue? value)
        {
            if (_forward.Remove(key, out TValue? valueInner))
            {
                _reverse.Remove(valueInner);
                value = valueInner;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if(_forward.TryAdd(key, value))
            {
                _reverse.Add(value, key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue? value)
        {
            if(_forward.TryGetValue(key, out TValue? valueInner))
            {
                value = valueInner;
                return true;
            }
            value = default(TValue);
            return true;
        }

        public BidirectionalDictrionary<TValue, TKey> Reverse => new BidirectionalDictrionary<TValue, TKey>(_reverse, _forward);

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            return _forward.GetEnumerator();
        }
    }
}
