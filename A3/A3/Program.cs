using System;
using Priority_Queue;

namespace A3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            SimplePriorityQueue<int> s = new SimplePriorityQueue<int>();
            s.Enqueue(0, 0);
            s.Enqueue(1, 1);
            Console.WriteLine(s.Dequeue());
        }
    }
}
