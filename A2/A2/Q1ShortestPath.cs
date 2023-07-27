using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A2
{
    public class Q1ShortestPath : Processor
    {
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

        public long Solve(long NodeCount, long[][] edges,
                          long StartNode, long EndNode)
        {
            int[] distance = Enumerable.Repeat(int.MaxValue , (int)NodeCount).ToArray();
            List<List<long>> adjacent_list = new List<List<long>>();
            for (long i = 0; i < NodeCount; i++)
            {
                adjacent_list.Add(new List<long>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
                adjacent_list[(int)edges[i][1] - 1].Add((int)edges[i][0] - 1);
            }
            
            BFS(adjacent_list , startnode: StartNode -1 , distance , endnode:EndNode -1);
            if (distance[EndNode - 1] == int.MaxValue)
            {
                return -1;
            }
            return distance[EndNode - 1];
        }
        private void BFS(List<List<long>> adjacent_list, long startnode, int[] distance , long endnode)
        {
            Queue<long> vertexes = new Queue<long>();
            vertexes.Enqueue(startnode);
            distance[startnode] = 0 ;
            while (vertexes.Any())
            {
                long dequeued = vertexes.Dequeue();
                foreach (var item in adjacent_list[(int)dequeued])
                {
                    if (distance[item] == int.MaxValue)
                    {
                        vertexes.Enqueue(item);
                        distance[item] = distance[dequeued] + 1;
                    }
                    if (item == endnode)
                    {
                        return ;
                    }
                }
            }
        }
    }
}
