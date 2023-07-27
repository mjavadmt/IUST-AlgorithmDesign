using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using GeoCoordinatePortable;
using Priority_Queue;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public static readonly char[] IgnoreChars = new char[] { '\n', '\r', ' ' };
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        private static double[][] ReadTree(IEnumerable<string> lines)
        {
            return lines.Select(line =>
                line.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(n => double.Parse(n)).ToArray()
                            ).ToArray();
        }
        public override string Process(string inStr)
        {
            return Process(inStr, (Func<long, long, double[][], double[][], long,
                                    long[][], double[]>)Solve);
        }
        public static string Process(string inStr, Func<long, long, double[][]
                                  , double[][], long, long[][], double[]> processor)
        {
            var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
            long[] count = lines.First().Split(IgnoreChars,
                                               StringSplitOptions.RemoveEmptyEntries)
                                         .Select(n => long.Parse(n))
                                         .ToArray();
            double[][] points = ReadTree(lines.Skip(1).Take((int)count[0]));
            double[][] edges = ReadTree(lines.Skip(1 + (int)count[0]).Take((int)count[1]));
            long queryCount = long.Parse(lines.Skip(1 + (int)count[0] + (int)count[1])
                                         .Take(1).FirstOrDefault());
            long[][] queries = ReadTree(lines.Skip(2 + (int)count[0] + (int)count[1]))
                                        .Select(x => x.Select(z => (long)z).ToArray())
                                        .ToArray();

            return string.Join("\n", processor(count[0], count[1], points, edges,
                                queryCount, queries));
        }
        public double[] Solve(long nodeCount,
                            long edgeCount,
                            double[][] points,
                            double[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            List<List<(long, double)>> adjacent_list_weights = new List<List<(long, double)>>();
            for (long i = 0; i < nodeCount; i++)
            {
                adjacent_list_weights.Add(new List<(long, double)>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list_weights[(int)edges[i][0] - 1].Add(((int)edges[i][1] - 1, edges[i][2]));
            }
            double[] results = new double[queries.Length];
            long initialzier = 0 ;
            foreach (var item in queries)
            {
                results[initialzier] = A_star(adjacent_list_weights , nodeCount , item[0] - 1 , item[1] - 1 , points);
                initialzier ++;
            }
            
            return results;
        }

        private long A_star(List<List<(long, double)>> adjacent_list_weights ,long nodeCount  , long startNode , long endNode , double[][] points)
        {
            
            SimplePriorityQueue<long> PQ = new SimplePriorityQueue<long>();
            bool[] visited = new bool[nodeCount];
            bool[] non_changing = new bool[nodeCount];
            PQ.Enqueue(startNode, (float)GetDistance(points[startNode] , points[endNode]));
            visited[startNode] = true ;
            while (PQ.Any())
            {
                float get_deqeued_priority = (float)(PQ.GetPriority(PQ.First) - GetDistance(points[PQ.First] , points[endNode]));
                long dequeued = PQ.Dequeue();
                non_changing[dequeued] = true;
                if (dequeued == endNode)
                    return (long)get_deqeued_priority;
                foreach (var neighbor in adjacent_list_weights[(int)dequeued])
                {
                    if (!visited[neighbor.Item1])
                    {
                        PQ.Enqueue(neighbor.Item1 ,(float)(get_deqeued_priority + neighbor.Item2 + GetDistance(points[neighbor.Item1] , points[endNode])));
                        visited[neighbor.Item1] = true;
                    }
                    else
                    {
                        if (non_changing[neighbor.Item1])
                            continue;
                        if (PQ.GetPriority(neighbor.Item1) > (float)(get_deqeued_priority + neighbor.Item2 + GetDistance(points[neighbor.Item1] , points[endNode])))
                        {
                            PQ.UpdatePriority(neighbor.Item1 , (float)(get_deqeued_priority + neighbor.Item2 + GetDistance(points[neighbor.Item1] , points[endNode])));
                        }
                    }
                }
            }
            return -1 ;
        }
        private double GetDistance (double[] first_point , double[] second_point)
        {
            return Math.Sqrt(Math.Pow((first_point[0] - second_point[0] ) , 2) + Math.Pow((first_point[1] - second_point[1] ) , 2) ); 
        } 
    }
}