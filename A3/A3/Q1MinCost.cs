using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Priority_Queue;

namespace A3
{
    public class Q1MinCost : Processor
    {
        public Q1MinCost(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
        {
            List<List<(long, long)>> adjacent_list_weights = new List<List<(long, long)>>();
            List<List<long>> edges_weight = new List<List<long>>();
            for (long i = 0; i < nodeCount; i++)
            {
                adjacent_list_weights.Add(new List<(long, long)>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list_weights[(int)edges[i][0] - 1].Add(((int)edges[i][1] - 1, (int)edges[i][2]));
            }
            SimplePriorityQueue<long> PQ = new SimplePriorityQueue<long>();
            bool[] visited = new bool[nodeCount];
            bool[] non_changing = new bool[nodeCount];
            PQ.Enqueue(startNode - 1, 0);
            visited[startNode - 1] = true;
            while (PQ.Any())
            {
                float get_deqeued_priority = PQ.GetPriority(PQ.First);
                long dequeued = PQ.Dequeue();
                non_changing[dequeued] = true;
                if (dequeued == endNode -1)
                    return (long)get_deqeued_priority;
                foreach (var neighbor in adjacent_list_weights[(int)dequeued])
                {
                    if (!visited[neighbor.Item1])
                    {
                        PQ.Enqueue(neighbor.Item1 , get_deqeued_priority + neighbor.Item2);
                        visited[neighbor.Item1] = true;
                    }
                    else
                    {
                        if (non_changing[neighbor.Item1])
                            continue;
                        if (PQ.GetPriority(neighbor.Item1) > get_deqeued_priority + neighbor.Item2)
                        {
                            PQ.UpdatePriority(neighbor.Item1 , get_deqeued_priority + neighbor.Item2);
                        }
                    }
                }
            }
            return -1;
        }
    }
}
