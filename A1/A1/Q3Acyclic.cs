using System;
using System.Collections.Generic;
using TestCommon;
using System.Linq;

namespace A12
{
    public class Q3Acyclic : Processor
    {
        public Q3Acyclic(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            List<List<long>> adjacent_list = new List<List<long>>();
            for (long i = 0 ; i < nodeCount ; i++)
            {
                adjacent_list.Add(new List<long>());
            }
            for (int i = 0 ; i < edges.Length ; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
            }
            return DFS(adjacent_list);
        }
        private long DFS(List<List<long>> adjacent_list)
        {
            // make representation list of red green blue
            List<char> visited = new List<char>();
            for ( int i = 0 ; i< adjacent_list.Count ; i++)
            {
                visited.Add('r');
            }
            for (int i = 0 ; i< adjacent_list.Count ; i++)
            {
                if (visited[i] != 'b')
                {
                    if (Explore(i , visited , adjacent_list) == 1)
                        return 1;
                }
            }
            return 0;
        }
        private long Explore(int vertex, List<char> visited, List<List<long>> adjacent_list)
        {
            if (visited[vertex] == 'g')
                return 1;
            visited[vertex] = 'g';
            foreach (var neighbor in adjacent_list[vertex])
            {
                if (visited[(int)neighbor] != 'b')
                {
                    if (Explore((int)neighbor , visited , adjacent_list) == 1)
                    {
                        return 1;
                    } 
                }
            }
            visited[vertex] = 'b';
            return 0;
        }
    }
}