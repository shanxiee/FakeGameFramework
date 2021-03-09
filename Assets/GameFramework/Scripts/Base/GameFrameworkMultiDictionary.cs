using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class GameFrameworkMultiDictionary<Tkey,TValue> : IEnumerable<KeyValuePair<Tkey,GameFrameworkLinkedListRange<TValue>>>,IEnumerable
    {
        private readonly GameFrameworkLinkedList<TValue> m_LinkedList;
        private readonly Dictionary<Tkey, GameFrameworkLinkedListRange<TValue>> m_Dictionary;

        public GameFrameworkMultiDictionary()
        {
            m_LinkedList = new GameFrameworkLinkedList<TValue>();
            m_Dictionary = new Dictionary<Tkey, GameFrameworkLinkedListRange<TValue>>();
        }

        public int Count
        {
            get
            {
                return m_Dictionary.Count;
            }
        }

        public GameFrameworkLinkedListRange<TValue> this[Tkey key]
        {
            get
            {
                GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
                m_Dictionary.TryGetValue(key, out range);
                return range;
            }
        }

        public void Clear()
        {
            m_Dictionary.Clear();
            m_LinkedList.Clear();
        }

        public bool Contains(Tkey key)
        {
            return m_Dictionary.ContainsKey(key);
        }

        public bool Contains(Tkey key,TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if(m_Dictionary.TryGetValue(key,out range))
            {
                return range.Contains(value);
            }
            return false;
        }

        public bool TryGetValue(Tkey key,out GameFrameworkLinkedListRange<TValue> range)
        {
            return m_Dictionary.TryGetValue(key, out range);
        }

        public void Add(Tkey key,TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if(m_Dictionary.TryGetValue(key,out range))
            {
                m_LinkedList.AddBefore(range.Terminal, value);
            }
            else
            {
                LinkedListNode<TValue> first = m_LinkedList.AddLast(value);
                LinkedListNode<TValue> terminal = m_LinkedList.AddLast(default(TValue));
                m_Dictionary.Add(key, new GameFrameworkLinkedListRange<TValue>(first, terminal));
            }
        }

        public bool Remove(Tkey key,TValue value)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if(m_Dictionary.TryGetValue(key,out range))
            {
                for(LinkedListNode<TValue> current = range.First;current !=null && current !=range.Terminal;current = current.Next)
                {
                    if (current.Value.Equals(value))
                    {
                        if(current == range.First)
                        {
                            LinkedListNode<TValue> next = current.Next;
                            if(next == range.Terminal)
                            {
                                m_LinkedList.Remove(next);
                                m_Dictionary.Remove(key);
                            }
                            else
                            {
                                m_Dictionary[key] = new GameFrameworkLinkedListRange<TValue>(next, range.Terminal);
                            }
                        }
                        m_LinkedList.Remove(current);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveAll(Tkey key)
        {
            GameFrameworkLinkedListRange<TValue> range = default(GameFrameworkLinkedListRange<TValue>);
            if(m_Dictionary.TryGetValue(key,out range))
            {
                m_Dictionary.Remove(key);

                LinkedListNode<TValue> current = range.First;
                while(current != null)
                {
                    LinkedListNode<TValue> next = current != range.Terminal ? current.Next : null;
                    m_LinkedList.Remove(current);
                    current = next;
                }
                return true;
            }
            return false;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dictionary);
        }

        IEnumerator<KeyValuePair<Tkey,GameFrameworkLinkedListRange<TValue>>> IEnumerable<KeyValuePair<Tkey, GameFrameworkLinkedListRange<TValue>>>.GetEnumerator()
        {
            return GetEnumerator();
        }
          
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator, IEnumerator<KeyValuePair<Tkey, GameFrameworkLinkedListRange<TValue>>>
        {
            private Dictionary<Tkey, GameFrameworkLinkedListRange<TValue>>.Enumerator m_Enumerator;

            internal Enumerator(Dictionary<Tkey,GameFrameworkLinkedListRange<TValue>> dictionary)
            {
                if(dictionary == null)
                {
                    throw new System.Exception("Dictionary is invalid.");
                }
                m_Enumerator = dictionary.GetEnumerator();
            }
            
            public  KeyValuePair<Tkey,GameFrameworkLinkedListRange<TValue>> Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<Tkey, GameFrameworkLinkedListRange<TValue>>>)m_Enumerator).Reset();
            }
        }
    }
}

