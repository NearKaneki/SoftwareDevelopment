using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slovar_
{
    class CustomDictionary<Tkey, Tvalue> : IDictionary<Tkey, Tvalue>
    {
        internal struct Entry
        {
            internal int hashCode;
            internal Tkey key;
            internal Tvalue value;
        }
        private List<List<Entry>> HashTable;
        private List<Tkey> KeyCollection;
        private List<Tvalue> ValueCollection;
        private int count;

        public CustomDictionary()
        {
            HashTable = new List<List<Entry>>(10);
            for(int i = 0; i < 10; ++i)
            {
                HashTable.Add(new List<Entry>());
            }
            KeyCollection = new List<Tkey>();
            ValueCollection = new List<Tvalue>();
            count = 0;
        }
        public Tvalue this[Tkey key]
        {
            get
            {
                int tempHash = key.GetHashCode();
                int index = tempHash % HashTable.Capacity;
                if (HashTable[index].Count != 0)
                {
                    foreach (Entry i in HashTable[index])
                    {
                        if (i.hashCode == tempHash)
                        {
                            return i.value;
                        }
                    }
                }
                throw new Exception("dont have");
            }
            set
            {
                int tempHash = key.GetHashCode();
                int index = tempHash % HashTable.Capacity;
                if (HashTable[index].Count != 0)
                {
                    for(int i=0;i< HashTable[index].Count; ++i)
                    {
                        if (HashTable[index][i].key.Equals(key))
                        {
                            HashTable[index][i] = new Entry { hashCode = tempHash, key = key, value = value };
                        }
                    }
                }
            }
        }

        public ICollection<Tkey> Keys => KeyCollection;

        public ICollection<Tvalue> Values => ValueCollection;

        public int Count => count;

        public bool IsReadOnly => false;

        public object ExceptionResource { get; private set; }

        public void trueAdd(Tkey key, Tvalue value, int tempHash,int index)
        {
            Entry temp = new Entry { hashCode = tempHash, key = key, value = value };
            HashTable[index].Add(temp);
            KeyCollection.Add(key);
            ValueCollection.Add(value);
            ++count;
        }
        public void Add(Tkey key, Tvalue value)
        {
            int tempHash = key.GetHashCode();
            int index = Math.Abs(tempHash % HashTable.Capacity);
            if (HashTable[index].Count != 0)
            {
                foreach (Entry i in HashTable[index])
                {
                    if (i.hashCode == tempHash)
                    {
                        throw new Exception("AddingDuplicate");
                    }
                }
            }
            trueAdd(key, value, tempHash, index);
        }

        public void Add(KeyValuePair<Tkey, Tvalue> item)
        {
            if (Contains(item))
            {
                   throw new Exception("AddingDuplicate");
            }
            int tempHash = item.Key.GetHashCode();
            int index = tempHash % HashTable.Capacity;
            trueAdd(item.Key, item.Value, tempHash,index);
        }

        public void Clear()
        {
            HashTable = new List<List<Entry>>();
            KeyCollection = new List<Tkey>();
            ValueCollection = new List<Tvalue>();
            count = 0;
        }

        public bool Contains(KeyValuePair<Tkey, Tvalue> item)
        {
            Tkey key = item.Key;
            Tvalue value = item.Value;
            int tempHash = key.GetHashCode();
            int index = tempHash % HashTable.Capacity;
            if (HashTable[index].Count != 0)
            {
                foreach (Entry i in HashTable[index])
                {
                    if (i.hashCode == tempHash && i.value.Equals(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsKey(Tkey key)
        {
            int tempHash = key.GetHashCode();
            int index = tempHash % HashTable.Capacity;
            if (HashTable[index].Count != 0)
            {
                foreach (Entry i in HashTable[index])
                {
                    if (i.hashCode == tempHash)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void CopyTo(KeyValuePair<Tkey, Tvalue>[] array, int arrayIndex)
        {
            for(int i = 0; i < count; ++i)
            {
                array[arrayIndex + i] = new KeyValuePair<Tkey, Tvalue>(KeyCollection[i], ValueCollection[i]);
            }
        }

        public IEnumerator<KeyValuePair<Tkey, Tvalue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(Tkey key)
        {
            int tempHash = key.GetHashCode();
            int index = tempHash % HashTable.Capacity;
            Entry tempEntry=new Entry();
            bool good = false;
            if (HashTable[index].Count != 0)
            {
                foreach (Entry i in HashTable[index])
                {
                    if (i.hashCode == tempHash)
                    {
                        good=true;
                        tempEntry = i;
                        break;
                    }
                }
            }
            if (good)
            {
                KeyCollection.Remove(key);
                ValueCollection.Remove(tempEntry.value);
                HashTable[index].Remove(tempEntry);
                --count;
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<Tkey, Tvalue> item)
        {
            Tkey key = item.Key;
            int tempHash = key.GetHashCode();
            int index = tempHash % HashTable.Capacity;
            Entry tempEntry = new Entry();
            bool good = false;
            if (HashTable[index].Count != 0)
            {
                foreach (Entry i in HashTable[index])
                {
                    if (i.hashCode == tempHash)
                    {
                        good = true;
                        tempEntry = i;
                        break;
                    }
                }
            }
            if (good)
            {
                KeyCollection.Remove(key);
                ValueCollection.Remove(tempEntry.value);
                HashTable[index].Remove(tempEntry);
                --count;
                return true;
            }
            return false;
        }

        public bool TryGetValue(Tkey key, [MaybeNullWhen(false)] out Tvalue value)
        {
            int tempHash = key.GetHashCode();
            int index = tempHash % HashTable.Capacity;
            if (HashTable[index].Count != 0)
            {
                foreach (Entry i in HashTable[index])
                {
                    if (i.hashCode == tempHash)
                    {
                        value = i.value;
                        return true;
                    }
                }
            }
            value = default(Tvalue);
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
