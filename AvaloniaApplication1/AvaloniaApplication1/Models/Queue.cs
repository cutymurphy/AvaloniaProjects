using System;

namespace AvaloniaApplication1.Models
{
    class Queue<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public int Count => count;
        public bool IsEmpty => count == 0;
        public string CurrentElement => head != null ? head.Data.ToString() : "None";

        public void Enqueue(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (IsEmpty)
            {
                head = tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }

            count++;
        }

        public void Dequeue()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            head = head.Next;
            if (head == null)
            {
                tail = null;
            }

            count--;
        }

        public void Clear()
        {
            head = tail = null;
            count = 0;
        }

        public string Print()
        {
            string str = "";
            Node<T> current = head;
            while (current != null)
            {
                str += current.Data + " ";
                current = current.Next;
            }

            return str;
        }
    }
}