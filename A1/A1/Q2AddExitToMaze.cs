using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A12
{
    public class Q2AddExitToMaze : Processor
    {
        public Q2AddExitToMaze(string testDataName) : base(testDataName)
        {
            // ExcludeTestCaseRangeInclusive(30 , 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
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
            return BFS(adjacent_list);
        }
        private long DFS(List<HashSet<long>> adjacent_list)
        {
            List<bool> visited = Enumerable.Repeat(false, adjacent_list.Count).ToList();
            long count = 0;
            for (int i = 0; i < adjacent_list.Count; i++)
            {
                if (!visited[i])
                {
                    Explore(i, visited, adjacent_list);
                    count++;
                }
            }
            return count;
        }
        private long BFS(List<List<long>> adjacent_list)
        {
            bool[] visited = new bool[adjacent_list.Count];
            long cc = 0 ;
            for (int i = 0; i < adjacent_list.Count; i++)
            {
                if (!visited[i])
                {
                    Distance(adjacent_list , visited , i);
                    cc ++ ;
                }
            }
            return cc;
        }
        private void Distance(List<List<long>> adjacent_list, bool[] visited, long vertex)
        {
            visited[vertex] = true;
            Queue<long> q = new Queue<long>();
            q.Enqueue(vertex);
            while (q.Any())
            {
                long dequeued = q.Dequeue();
                foreach (var neighbor in adjacent_list[(int)dequeued])
                {
                    if (!visited[neighbor])
                    {
                        visited[neighbor] = true;
                        q.Enqueue(neighbor);
                    }
                }
            }
        }
        private void Explore(int vertex, List<bool> visited, List<HashSet<long>> adjacent_list)
        {
            foreach (var neighbor in adjacent_list[vertex])
            {
                if (!visited[(int)neighbor])
                {
                    visited[(int)neighbor] = true;
                    Explore((int)neighbor, visited, adjacent_list);
                }
            }
        }
    }
}
