using System;
using TestCommon;
using System.Collections.Generic ;
using System.Linq ;
namespace A2
{
    public class Q2BipartiteGraph : Processor
    {
        public Q2BipartiteGraph(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long NodeCount, long[][] edges)
        {
            List<char> vertex_color = Enumerable.Repeat('w' , (int)NodeCount).ToList();
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

            for (int i = 0 ; i < NodeCount ; i++)
            {
                if (vertex_color[i] == 'w')
                {
                    if (BFS(adjacent_list , i , vertex_color) == 0)
                        return 0;
                }
            }
            return 1;
        }

        private long BFS(List<List<long>> adjacent_list , long startnode , List<char> vertex_color)
        {
            Queue<long> vertexes = new Queue<long>();
            vertexes.Enqueue(startnode);
            while (vertexes.Any())
            {
                long dequeued = vertexes.Dequeue();
                foreach (var item in adjacent_list[(int)dequeued])
                {
                    if (vertex_color[(int)item] == 'w')
                    {
                        vertex_color[(int)item] = (vertex_color[(int)dequeued] == 'b') ? 'g' : 'b';
                        vertexes.Enqueue(item);
                    }
                    else
                    {
                        if (vertex_color[(int)item] == vertex_color[(int)dequeued])
                        {
                            return 0;
                        }
                    }
                }
            }
            return 1; 
        }
    }
}
