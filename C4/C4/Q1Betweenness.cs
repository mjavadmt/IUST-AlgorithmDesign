using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace E1
{
    public class Q1Betweenness : Processor
    {
        public Q1Betweenness(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);


        public long[] Solve(long NodeCount, long[][] edges)
        {
            // when adj made sort each one of them based on vertex
            List<List<long>> adjacent_list = new List<List<long>>();
            long[] repetition = new long[NodeCount];
            for (long i = 0; i < NodeCount; i++)
            {
                adjacent_list.Add(new List<long>());
                repetition[i] = 0;
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
            }
            for (int i = 0; i < NodeCount; i++)
            {
                adjacent_list[i] = adjacent_list[i].OrderByDescending(x => x).ToList();
            }
            for (int i = 0; i < NodeCount; i++)
            {
                int[] distance = Enumerable.Repeat(int.MaxValue , (int)NodeCount).ToArray();
                BFS(adjacent_list, i, distance, repetition);
            }
            return repetition;
        }
        private static List<long> BFS(List<List<long>> adjacent_list, long startnode, int[] distance, long[] repetition)
        {
            Queue<long> vertexes = new Queue<long>();
            vertexes.Enqueue(startnode);
            distance[startnode] = 0;
            long[] pre = new long[adjacent_list.Count];
            pre[startnode] = -1;
            while (vertexes.Any())
            {
                long dequeued = vertexes.Dequeue();
                foreach (var item in adjacent_list[(int)dequeued])
                {
                    if (distance[item] == int.MaxValue)
                    {
                        vertexes.Enqueue(item);
                        distance[item] = distance[dequeued] + 1;
                        pre[item] = dequeued;
                    }
                }
            }
            for (int i = 0 ; i < adjacent_list.Count ; i++)
            {
                if ( i == startnode || distance[i] == int.MaxValue)
                    continue;
                shortestpath(pre , i , startnode , repetition);
            }
            return null;
        }
        private static void shortestpath(long[] pre, long endnode, long startnode , long[] repetition)
        {
            List<long> path = new List<long>();
            while (pre[endnode] != startnode)
            {
                repetition[pre[endnode]] += 1;
                endnode = pre[endnode];
            }
        }
    }
}
