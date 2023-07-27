using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TestCommon;

namespace A12
{
    public class Q4OrderOfCourse : Processor
    {
        public Q4OrderOfCourse(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long[]>)Solve);

        public long[] Solve(long nodeCount, long[][] edges)
        {
            List<List<long>> adjacent_list = new List<List<long>>();
            for (long i = 0; i < nodeCount; i++)
            {
                adjacent_list.Add(new List<long>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
            }
            List<long> added = new List<long>();
            DFS(adjacent_list , added);
            added.Reverse();
            return added.ToArray(); 
        }

        private void DFS(List<List<long>> adjacent_list, List<long> added)
        {
            List<bool> visited = Enumerable.Repeat(false, adjacent_list.Count).ToList();
            for (int i = 0; i < adjacent_list.Count; i++)
            {
                if (!visited[i])
                {
                    Explore(i, visited, adjacent_list, added);
                }
            }
        }
        private void Explore(int vertex, List<bool> visited, List<List<long>> adjacent_list, List<long> added)
        {
            visited[vertex] = true;
            foreach (var neighbor in adjacent_list[vertex])
            {
                if (!visited[(int)neighbor])
                {
                    Explore((int)neighbor, visited, adjacent_list, added);
                }
            }
            added.Add(vertex + 1);
        }

        public override Action<string, string> Verifier { get; set; } = TopSortVerifier;

        public static void TopSortVerifier(string inFileName, string strResult)
        {
            long[] topOrder = strResult.Split(TestTools.IgnoreChars)
                .Select(x => long.Parse(x)).ToArray();

            long count;
            long[][] edges;
            TestTools.ParseGraph(File.ReadAllText(inFileName), out count, out edges);

            // Build an array for looking up the position of each node in topological order
            // for example if topological order is 2 3 4 1, topOrderPositions[2] = 0, 
            // because 2 is first in topological order.
            long[] topOrderPositions = new long[count];
            for (int i = 0; i < topOrder.Length; i++)
                topOrderPositions[topOrder[i] - 1] = i;
            // Top Order nodes is 1 based (not zero based).

            // Make sure all direct depedencies (edges) of the graph are met:
            //   For all directed edges u -> v, u appears before v in the list
            foreach (var edge in edges)
                if (topOrderPositions[edge[0] - 1] >= topOrderPositions[edge[1] - 1])
                    throw new InvalidDataException(
                        $"{Path.GetFileName(inFileName)}: " +
                        $"Edge dependency violoation: {edge[0]}->{edge[1]}");

        }
    }
}
