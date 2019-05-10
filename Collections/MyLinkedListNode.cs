using System;

namespace Collections
{
    public class MyLinkedListNode<T>
        where T: IEquatable<T>
    {
        public MyLinkedListNode(T item)
        {
            Item = item;
        }

        public MyLinkedListNode<T> Previous { get; set; }

        public MyLinkedListNode<T> Next { get; set; }

        public T Item { get; set; }
    }

}
