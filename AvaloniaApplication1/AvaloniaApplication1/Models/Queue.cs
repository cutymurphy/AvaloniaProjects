using System;

namespace AvaloniaApplication1.Models
{
    class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    class Queue<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public int Count => count;
        public bool IsEmpty => count == 0;
        public T CurrentElement => head != null ? head.Data : default(T);
        public string CurrentElementDisplay => head != null ? head.Data.ToString() : "None";

        public void Enqueue(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (tail == null)
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

        public T Dequeue()
        {
            if (head == null)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            T data = head.Data;
            head = head.Next;
            if (head == null)
            {
                tail = null;
            }
            count--;
            return data;
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