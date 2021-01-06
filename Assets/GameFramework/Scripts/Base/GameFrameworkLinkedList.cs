using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public sealed class GameFrameworkLinkedList<T> : ICollection<T>,IEnumerable<T>,ICollection,IEnumerable
    {
        private readonly LinkedList<T> m_LinkedList;
        private readonly Queue<LinkedListNode<T>> m_CacheNodes;

        public GameFrameworkLinkedList()
        {
            m_LinkedList = new LinkedList<T>();
            m_CacheNodes = new Queue<LinkedListNode<T>>();
        }


        public int Count
        {
            get
            {
                return m_LinkedList.Count;
            }
        }

        public int CachedNodeCount
        {
            get
            {
                return m_CacheNodes.Count;
            }
        }

        public LinkedListNode<T> First
        {
            get
            {
                return m_LinkedList.First;
            }
        }

        public LinkedListNode<T> Last
        {
            get
            {
                return m_LinkedList.Last;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<T>)m_LinkedList).IsReadOnly;
            }
        }

        public object SyncRoot
        {
            get
            {
                return ((ICollection)m_LinkedList).SyncRoot;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return ((ICollection)m_LinkedList).IsSynchronized;
            }
        }

        public void Add(T value)
        {
            AddLast(value);
        }
        
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node,T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddAfter(node, newNode);
            return newNode;
        }

        public void AddAfter(LinkedListNode<T> node,LinkedListNode<T> newNode)
        {
            m_LinkedList.AddAfter(node, newNode);
        }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node,T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddBefore(node, newNode);
            return newNode;
        }

        public void AddBefore(LinkedListNode<T> node,LinkedListNode<T> newNode)
        {
            m_LinkedList.AddBefore(node, newNode);
        }

        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddFirst(node);
            return node;
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            m_LinkedList.AddFirst(node);
        }
        
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddLast(node);
            return node;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            m_LinkedList.AddLast(node);
        }

        public void Clear()
        {
            LinkedListNode<T> current = m_LinkedList.First;
            while(current != null)
            {
                ReleaseNode(current);
                current = current.Next;
            }
            m_LinkedList.Clear();
        }

        public void ClearCachedNodes()
        {
            m_CacheNodes.Clear();
        }

        public bool Contains(T value)
        {
            return m_LinkedList.Contains(value);
        }

        public void CopyTo(T[] array,int index)
        {
            m_LinkedList.CopyTo(array, index);
        }

        public void CopyTo(Array array,int index)
        {
            ((ICollection)m_LinkedList).CopyTo(array, index);
        }

        public LinkedListNode<T> Find(T value)
        {
            return m_LinkedList.Find(value);
        }

        public LinkedListNode<T> FindLast(T value)
        {
            return m_LinkedList.FindLast(value);
        }


        public bool Remove(T value)
        {
            LinkedListNode<T> node = m_LinkedList.Find(value);
            if(node != null)
            {
                m_LinkedList.Remove(node);
                ReleaseNode(node);
                return true;
            }
            return false;
        }

        public void Remove(LinkedListNode<T> node)
        {
            m_LinkedList.Remove(node);
            ReleaseNode(node);
        }

        public void RemoveFirst()
        {
            LinkedListNode<T> first = m_LinkedList.First;
            if(first == null)
            {
                throw new Exception("First is invalid");
            }

            m_LinkedList.RemoveFirst();
            ReleaseNode(first);
        }
        
        public void RemoveLast()
        {
            LinkedListNode<T> last = m_LinkedList.Last;
            if(last == null)
            {
                throw new Exception("Last is invalid");
            }
            m_LinkedList.RemoveLast();
            ReleaseNode(last);
        }

        private LinkedListNode<T> AcquireNode(T value)
        {
            LinkedListNode<T> node = null;
            if(m_CacheNodes.Count > 0)
            {
                node = m_CacheNodes.Dequeue();
                node.Value = value;
            }
            else
            {
                node = new LinkedListNode<T>(value);
            }
            return node;
        }

        private void ReleaseNode(LinkedListNode<T> node)
        {
            node.Value = default(T);
            m_CacheNodes.Enqueue(node);
        }


        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_LinkedList);
        }

        public struct Enumerator : IEnumerator, IEnumerator<T>
        {
            private LinkedList<T>.Enumerator m_Enumerator;

            internal Enumerator(LinkedList<T> linkedList)
            {
                if(linkedList == null)
                {
                    throw new Exception("Linked list is invalid");
                }
                m_Enumerator = linkedList.GetEnumerator();
            }

            public T Current
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
                ((IEnumerator<T>)m_Enumerator).Reset();
            }
        }

    }
}
