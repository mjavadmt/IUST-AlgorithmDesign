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
            this.ExcludeTestCaseRangeInclusive(45, 50);
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
                repetition[i] = 0 ;
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
            }
            for (int i = 0 ; i < NodeCount ; i++)
            {
                adjacent_list[i] = adjacent_list[i].OrderByDescending(x => x).ToList();
            }
            for (int i = 0 ; i < NodeCount ; i++)
            {
                for (int j = 0 ; j < NodeCount ; j++ )
                {
                    if (i == j)
                        continue;
                    int[] distance = new int[NodeCount];
                    for(int k = 0 ; k < NodeCount ; k++)
                    {
                        distance[k] = int.MaxValue;
                    }
                    var path = BFS(adjacent_list , i , distance , j);
                    if ( path != null)
                    {
                        foreach(var item in path)
                        {
                            repetition[item] += 1;
                        }
                    }
                }
            }
            return repetition ;
        }
        private static List<long> BFS(List<List<long>> adjacent_list, long startnode, int[] distance , long endnode)
        {
            Queue<long> vertexes = new Queue<long>();
            vertexes.Enqueue(startnode);
            distance[startnode] = 0 ;
            long[] pre = new long[adjacent_list.Count];
            pre[startnode] = -1 ;
            while (vertexes.Any())
            {
                long dequeued = vertexes.Dequeue();
                foreach (var item in adjacent_list[(int)dequeued])
                {
                    if (distance[item] == int.MaxValue)
                    {
                        vertexes.Enqueue(item);
                        distance[item] = distance[dequeued] + 1;
                        pre[item] = dequeued ;
                    }
                    if (item == endnode)
                    {
                        return shortestpath(pre , endnode , startnode);
                    }
                }
            }
            return null ;
        }
        private static List<long> shortestpath(long[] pre , long endnode , long startnode)
        {
            List<long> path = new List<long>();
            while (pre[endnode] != startnode)
            {
                path.Add(pre[endnode]);
                endnode = pre[endnode];
            }
            return path;
        }
    }
}
