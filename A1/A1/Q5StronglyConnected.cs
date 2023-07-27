using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TestCommon;

namespace A12
{
    public class Q5StronglyConnected : Processor
    {
        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            List<List<long>> adjacent_list_reverse = new List<List<long>>();
            List<List<long>> adjacent_list = new List<List<long>>();
            int number = 1;
            long[] pre_visited = new long[nodeCount];
            long[] post_visited = new long[nodeCount];
            for (long i = 0; i < nodeCount; i++)
            {
                adjacent_list_reverse.Add(new List<long>());
                adjacent_list.Add(new List<long>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list_reverse[(int)edges[i][1] - 1].Add((int)edges[i][0] - 1);
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
            }
            DFS(adjacent_list_reverse, pre_visited, post_visited, ref number);
            List<(long, long)> vertixes_post = new List<(long, long)>();
            for (int i = 0; i < nodeCount; i++)
            {
                vertixes_post.Add((i, post_visited[i]));
            }
            List<bool> visited = Enumerable.Repeat(false, (int)nodeCount).ToList();
            vertixes_post = vertixes_post.OrderByDescending(x => x.Item2).ToList();
            int x = 0;
            long count = 0;
            foreach (var item in vertixes_post)
            {
                if (!visited[(int)item.Item1])
                {
                    Explore((int)item.Item1, visited, adjacent_list, new long[nodeCount], new long[nodeCount], ref x);
                    count ++ ;
                }
            }
            return count ;
        }
        private void DFS(List<List<long>> adjacent_list, long[] pre, long[] post, ref int number)
        {
            List<bool> visited = new List<bool>();
            for (int i = 0; i < adjacent_list.Count; i++)
            {
                visited.Add(false);
            }
            for (int i = 0; i < adjacent_list.Count; i++)
            {
                if (!visited[i])
                {
                    Explore(i, visited, adjacent_list, pre, post, ref number);
                }
            }
        }
        private void Explore(int vertex, List<bool> visited, List<List<long>> adjacent_list, long[] pre, long[] post, ref int number)
        {
            pre[vertex] = number;
            visited[vertex] = true;
            foreach (var neighbor in adjacent_list[vertex])
            {
                if (!visited[(int)neighbor])
                {
                    number++;
                    Explore((int)neighbor, visited, adjacent_list, pre, post, ref number);

                }
            }
            number++;
            post[vertex] = number;
        }
    }
}
