using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;

namespace PS1
{
    public class Q1GumballMachine : Processor
    {
        public Q1GumballMachine(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long>)Solve);

        public long Solve(long x, long y)
        {
            var last_node = Math.Max(2 * x, 2*y);
            List<List<long>> adj = MakeAdj(last_node);
            var distance = Enumerable.Repeat(int.MaxValue, (int)last_node + 1).ToArray();
            distance[x] = 0;
            return BFS(StartNode: x, adj, distance, y);
        }

        private long BFS(long StartNode, List<List<long>> adj, int[] distance, long y)
        {
            Queue<long> vertixes = new Queue<long>();
            vertixes.Enqueue(StartNode);
            while (vertixes.Any())
            {
                long dequeued = vertixes.Dequeue();
                foreach (var neighbor in adj[(int)dequeued])
                {
                    if (distance[neighbor] == int.MaxValue)
                    {
                        vertixes.Enqueue(neighbor);
                        distance[neighbor] = distance[dequeued] + 1;
                        if (neighbor == y)
                        {
                            return distance[neighbor];
                        }
                    }
                }
            }
            return -1;
        }

        private List<List<long>> MakeAdj(long last_node)
        {
            List<List<long>> adj = new List<List<long>>();
            for (int i = 0; i <= last_node; i++)
            {
                if (i == 0)
                {
                    adj.Add(new List<long>());
                    continue;
                }
                if (i <= last_node / 2)
                {
                    adj.Add(new List<long>() { i - 1, i * 2 });
                }
                else
                {
                    adj.Add(new List<long>() { i - 1 });
                }

            }
            return adj;
        }
    }
}
