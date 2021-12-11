using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Slovar_
{
    class CustDict<Tkey, Tvalue> : IDictionary<Tkey, Tvalue>
    {
        internal struct Entry
        {
            internal int hashCode;
            internal Tkey key;
            internal Tvalue value;
        }
        private LinkedList<Entry>[] HashTable;

        public CustDict()
        {
            HashTable = new LinkedList<Entry>[10];
        }

        public Tvalue this[Tkey key] {
            get
            {
                int tempHash = key.GetHashCode();
                int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
                foreach (Entry i in HashTable[index])
                {
                    if (i.key.Equals(key))
                    {
                        return i.value;
                    }
                }
                throw new Exception($"Key {key} not found");
            }
            set
            {
                int tempHash = key.GetHashCode();
                int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
                Entry temp = new Entry { hashCode = tempHash, key = key, value = value };
                var current = HashTable[index].First;
                if (HashTable[index].Any(x => x.key.Equals(key)))
                {
                    while (true)
                    {
                        if (current.Value.key.Equals(key))
                        {
                            current.Value = temp;
                            return;
                        }
                    }
                }
                Add(key, value);
            }
        }

        public ICollection<Tkey> Keys
        {
            get
            {
                List<Tkey> keys = new List<Tkey>();
                foreach (var list in HashTable)
                {
                    foreach (var elem in list)
                    {
                        keys.Add(elem.key);
                    }
                }
                return keys;
            }
        }

        public ICollection<Tvalue> Values
        {
            get
            {
                List<Tvalue> values = new List<Tvalue>();
                foreach (var list in HashTable)
                {
                    foreach (var elem in list)
                    {
                        values.Add(elem.value);
                    }
                }
                return values;
            }
        }

        public int Count => HashTable.Where(x => x != null).Select(x => x.Count).Sum();

        public bool IsReadOnly => false;

        public void Add(Tkey key, Tvalue value)
        {
            int tempHash = key.GetHashCode();
            int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
            if (HashTable[index].Any(x => x.key.Equals(key)))
            {
                throw new ArgumentException("AddingDuplicate");
            }
            HashTable[index].AddLast(new Entry { hashCode = tempHash, key = key, value = value });
            int countZero = 0; // HashTable.Where(x => x.Count != 0).Select(x => x).Count;
            foreach(var list in HashTable)
            {
                if (list.Count == 0)
                {
                    ++countZero;
                }
            }
            if (HashTable[index].Count >= 5 || countZero<HashTable.Length/3)
            {
                LinkedList<Entry>[] newHashTable = new LinkedList<Entry>[HashTable.Length * 2];
                foreach(var list in HashTable)
                {
                    foreach(var elem in list)
                    {
                        tempHash = elem.hashCode;
                        index= ((tempHash % newHashTable.Length) + newHashTable.Length) % newHashTable.Length;
                        newHashTable[index].AddLast(elem);
                    }
                }
                HashTable = newHashTable;
            }
            
        }

        public void Add(KeyValuePair<Tkey, Tvalue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            HashTable = new LinkedList<Entry>[10];
        }

        public bool Contains(KeyValuePair<Tkey, Tvalue> item)
        {
            int tempHash = item.Key.GetHashCode();
            int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
            Entry temp = new Entry { hashCode = tempHash, key = item.Key, value = item.Value };
            return HashTable[index].Contains(temp);
        }

        public bool ContainsKey(Tkey key)
        {
            int tempHash = key.GetHashCode();
            int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
            return HashTable[index].Any(x => x.key.Equals(key));
        }

        public void CopyTo(KeyValuePair<Tkey, Tvalue>[] array, int arrayIndex)
        {
            foreach(var list in HashTable)
            {
                foreach (var elem in list)
                {
                    array[arrayIndex] = new KeyValuePair<Tkey, Tvalue>(elem.key, elem.value);
                    arrayIndex++;
                }
            }
        }

        public IEnumerator<KeyValuePair<Tkey, Tvalue>> GetEnumerator()
        {
            foreach(var list in HashTable)
            {
                foreach (var elem in list)
                {
                    yield return new KeyValuePair<Tkey, Tvalue>(elem.key, elem.value);
                }
            }
        }

        public bool Remove(Tkey key)
        {
            int tempHash = key.GetHashCode();
            int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
            if (HashTable[index].Any(x => x.key.Equals(key)))
            {
                Entry temp = new Entry();
                foreach (Entry i in HashTable[index])
                {
                    if (i.key.Equals(key))
                    {
                        temp = i;
                        break;
                    }
                }
                return HashTable[index].Remove(temp);
            }
            return false;
        }

        public bool Remove(KeyValuePair<Tkey, Tvalue> item)
        {
            int tempHash = item.Key.GetHashCode();
            int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
            Entry temp = new Entry { hashCode = tempHash, key = item.Key, value = item.Value };
            if (HashTable[index].Contains(temp))
            {
                return HashTable[index].Remove(temp);
            }
            return false;
        }

        public bool TryGetValue(Tkey key, [MaybeNullWhen(false)] out Tvalue value)
        {
            int tempHash = key.GetHashCode();
            int index = ((tempHash % HashTable.Length) + HashTable.Length) % HashTable.Length;
            foreach (var elem in HashTable[index])
            {
                if (elem.key.Equals(key))
                {
                    value = elem.value;
                    return true;
                }
            }
            value = default(Tvalue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Tkey, Tvalue>>)this).GetEnumerator();
        }
    }
}
