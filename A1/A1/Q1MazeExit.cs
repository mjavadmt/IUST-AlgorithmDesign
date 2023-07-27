using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A12
{
    public class Q1MazeExit : Processor
    {
        public Q1MazeExit(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

        public long Solve(long nodeCount, long[][] edges, long StartNode, long EndNode)
        {
            List<List<long>> adjacent_list = new List<List<long>>();
            for (long i = 0; i < nodeCount; i++)
            {
                adjacent_list.Add(new List<long>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
                adjacent_list[(int)edges[i][1] - 1].Add((int)edges[i][0] - 1);
            }
            List<bool> visited = Enumerable.Repeat(false,(int) nodeCount).ToList();
            return Explore((int)StartNode - 1 , visited , adjacent_list , EndNode - 1);
        }
        
        private long Explore(int vertex, List<bool> visited, List<List<long>> adjacent_list , long wanted_vertex)
        {
            if (vertex == wanted_vertex)
                return 1;
            visited[vertex] = true ;
            foreach (var neighbor in adjacent_list[vertex])
            {
                if (!visited[(int)neighbor])
                {
                    if (Explore((int)neighbor, visited, adjacent_list , wanted_vertex) == 1)
                       return 1;
                }
            }
            return 0 ;
        }
     }
}
