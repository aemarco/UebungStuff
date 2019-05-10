using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class MyLinkedList<T>
        where T : IEquatable<T>
    {
        #region fields

        private MyLinkedListNode<T> _head;
        private long _count = 0;

        #endregion

        #region ctor

        public MyLinkedList()
        {
        }

        #endregion

        #region props

        public long Count
        {
            get { return _count; }
        }

        public MyLinkedListNode<T> First
        {
            get { return _head; }
        }
        public MyLinkedListNode<T> Last
        {
            get
            {
                return (_count > 1) ? GetNodeAt(_count - 1) : _head;
            }
        }

        #endregion

        #region indexStuff

        public MyLinkedListNode<T> this[long index]
        {
            get
            {
                var current = GetNodeAt(index);
                return current;
            }
            set
            {
                var current = GetNodeAt(index);
                value.Next = current.Next;
                value.Previous = current.Previous;
                if (value.Next != null)
                {
                    value.Next.Previous = value;
                }

                if (value.Previous != null)
                {
                    value.Previous.Next = value;
                }

                if (current == _head)
                {
                    _head = value;
                }
            }
        }
        private MyLinkedListNode<T> GetNodeAt(long index)
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException();
            }

            var current = _head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            return current;
        }
        private long GetIndexFrom(MyLinkedListNode<T> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }
            var current = _head;
            long index = 0;
            for (int i = 0; i < _count; i++)
            {
                if (node == current)
                {
                    return index;
                }
                else
                {
                    current = current.Next;
                    index++;
                }
            }
            throw new ArgumentOutOfRangeException();
        }

        #endregion

        #region addStuff

        public void Add(MyLinkedListNode<T> item)
        {
            InsertAt(_count, item);
        }
        public MyLinkedListNode<T> Add(T item)
        {
            var itemToAdd = new MyLinkedListNode<T>(item);
            Add(itemToAdd);
            return itemToAdd;
        }

        public void AddBefore(MyLinkedListNode<T> itemToAdd, MyLinkedListNode<T> relItem)
        {
            var index = GetIndexFrom(relItem);
            InsertAt(index, itemToAdd);
        }
        public MyLinkedListNode<T> AddBefore(T value, MyLinkedListNode<T> relItem)
        {
            var itemToAdd = new MyLinkedListNode<T>(value);
            AddBefore(itemToAdd, relItem);
            return itemToAdd;
        }
        public void AddAfter(MyLinkedListNode<T> itemToAdd, MyLinkedListNode<T> relItem)
        {
            var index = GetIndexFrom(relItem);
            InsertAt(index + 1, itemToAdd);
        }
        public MyLinkedListNode<T> AddAfter(T value, MyLinkedListNode<T> relItem)
        {
            var itemToAdd = new MyLinkedListNode<T>(value);
            AddAfter(itemToAdd, relItem);
            return itemToAdd;
        }

        internal void InsertAt(long index, MyLinkedListNode<T> item)
        {
            if (_count == index) //inserting on first non existing index is permitted :)
            {
                if (_head == null)
                {
                    _head = item;
                }
                else
                {
                    var last = GetNodeAt(_count - 1);
                    last.Next = item;
                    item.Previous = last;
                }
            }
            else //otherwise there must be nodes already in the list
            {
                var nodeAfter = GetNodeAt(index);
                var nodeBefore = nodeAfter.Previous;

                item.Previous = nodeBefore;
                item.Next = nodeAfter;

                if (nodeAfter == _head)
                {
                    _head = item;
                }
                else
                {
                    nodeBefore.Next = item;
                }
                nodeAfter.Previous = item;
            }
            _count++;
        }

        #endregion

        #region removeStuff

        public void Remove(MyLinkedListNode<T> item)
        {
            if (_head == null)
            {
                return;
            }

            var node = _head;
            var index = 0;
            do
            {
                if (node == item)
                {
                    RemoveAt(index);
                    break;
                }
                node = node.Next;
                index++;

            } while (node != null);          
        }

        public void RemoveAt(long index)
        {
            var nodeToRemove = GetNodeAt(index);

            if (index > 0 && index < _count - 1) //remove between
            {
                nodeToRemove.Previous.Next = nodeToRemove.Next;
                nodeToRemove.Next.Previous = nodeToRemove.Previous;
            }
            else if (index == 0) //remove first
            {
                _head = nodeToRemove.Next;
                if (_head != null)
                {
                    _head.Previous = null;
                }
            }
            else //remove last
            {
                nodeToRemove.Previous.Next = null;
            }

            nodeToRemove.Previous = null;
            nodeToRemove.Next = null;
            _count--;
        }

        public void Clear()
        {
            if (_head == null) return;

            long index = _count - 1;
            do
            {
                RemoveAt(index--);
            } while (index > -1);
        }

        #endregion

        #region findingStuff

        public bool Contains(T value)
        {
            if (_head == null) return false;

            long index = 0;
            do
            {
                if (GetNodeAt(index++).Item.Equals(value))
                {
                    return true;
                }
            } while (index < _count);

            return false;
        }
        public bool Contains(MyLinkedListNode<T> item)
        {
            if (_head == null) return false;

            long index = 0;
            do
            {
                if (GetNodeAt(index++) == item)
                {
                    return true;
                }
            } while (index < _count);

            return false;
        }

        #endregion



    }

}
