using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class GenericLinkList<T>
    {
        private class Node
        {
            private Node _next;
            private T _data;

            public Node next { get { return _next; } set { _next = value; } }
            public T data { get { return _data; } set { _data = value; } }

            public Node(T t)
            {
                next = null;
                data = t;
            }
        }

        private Node head;

        public GenericLinkList(T data)
        {
            head = new Node(data);
        }

        public GenericLinkList(T[] data)
        {
            head = new Node(data[0]);
            Node ptr = head;

            foreach (T d in data.Skip(1))
            {
                ptr.next = new Node(d);
                ptr = ptr.next;
            }
        }

        public void Append(T data)
        {
            Node ptr = head;
            while (ptr.next != null)
            {
                ptr = ptr.next;
            }
            ptr.next = new Node(data);
        }

        public IEnumerable<T> GetEnumerator()
        {
            Node ptr = head;

            while (ptr.next != null)
            {
                yield return ptr.data;
                ptr = ptr.next;
            }
        }
    }
}
