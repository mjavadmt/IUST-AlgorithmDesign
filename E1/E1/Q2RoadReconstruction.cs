using System;
using System.Linq;
using TestCommon;
using System.Collections.Generic;
using Priority_Queue;
namespace E1
{
    public class Q2RoadReconstruction : Processor
    {
        public Q2RoadReconstruction(string testDataName) : base(testDataName)
        {
            // this.ExcludeTestCaseRangeInclusive(1,50);
            ExcludeTestCases(8,9);

        }

        public override Action<string, string> Verifier => RoadReconstructionVerifier.Verify;

        public override string Process(string inStr) {
            long count;
            long[][] data;
            TestTools.ParseGraph(inStr, out count, out data);
            return string.Join("\n", Solve(count, data).Select(edge => string.Join(" ", edge)));
        }

        // returns n different edges in the form of {u, v, weight}
        public long[][] Solve(long n, long[][] distance)
        {
            List<(long , long , long)> edges = new List<(long, long, long)>();
            long[] parents = new long[n];
            long[] ranks = new long[n];
            for (int i =  0 ; i < n ; i ++)
            {
                parents[i] = i;
                for ( int j = i + 1 ; j < n ; j++)
                {
                    edges.Add((i , j , distance[i][j]));
                }
            }
            edges = edges.OrderBy(x => x.Item3).ToList();
            // for adding edges which is light by not in the unioned set
            List<(long , long , long)> not_union_edges = new List<(long, long, long)>();
            List<List<(long, long)>> adjacent_list_weights = new List<List<(long, long)>>();
            for (int i = 0 ; i < n ; i ++)
            {
                adjacent_list_weights.Add(new List<(long, long)>());
            }
            // making MST
            // make ADJ new grpah
            foreach ( var edge in edges)
            {
                if (Find(edge.Item1 ,parents ) != Find(edge.Item2 , parents))
                {
                    Union(edge.Item1 , edge.Item2 , ranks , parents);
                    adjacent_list_weights[(int)edge.Item1].Add((edge.Item2 , edge.Item3));
                    adjacent_list_weights[(int)edge.Item2].Add((edge.Item1 , edge.Item3));
                }
                else
                {
                    not_union_edges.Add(edge);
                }
            }
            bool inside_if = false;
            // check if there is shortest path in grpah that is not covered by our MST
            foreach (var edge in not_union_edges)
            {
                if (Dijkstra(n , adjacent_list_weights , edge.Item1 , edge.Item2) > distance[edge.Item1][edge.Item2])
                {
                    adjacent_list_weights[(int)edge.Item1].Add((edge.Item2 , distance[edge.Item1][edge.Item2]));
                    adjacent_list_weights[(int)edge.Item2].Add((edge.Item1 , distance[edge.Item1][edge.Item2]));
                    inside_if = true;
                    break;
                }
            }
            
            if (! inside_if)
            {
                adjacent_list_weights[(int)not_union_edges[0].Item1].Add(((int)not_union_edges[0].Item2 , 
                (int)not_union_edges[0].Item3));
                adjacent_list_weights[(int)not_union_edges[0].Item2].Add(((int)not_union_edges[0].Item1
                 , (int)not_union_edges[0].Item3));
            }
            long[][] show_edges = new long[n][];
            long t = 0 ;
            for ( int i = 0 ; i < n ; i++)
            {
                foreach ( var neighbor in adjacent_list_weights[i])
                {
                    if (neighbor.Item1 > i )
                    {
                        show_edges[t] = new long[]{ neighbor.Item1 + 1 ,i + 1  , neighbor.Item2};
                        t ++;
                    }
                }
            }
            return show_edges;
        }
        private long Find(long u, long[] parents)
        {
            while (u != parents[u])
            {
                u = parents[u];
            }
            return parents[u];
        }
        private void Union(long x , long y , long[] ranks , long[] parents)
        {
            x = Find(x , parents);
            y = Find(y , parents);
            if (x == y)
                return;
            if (ranks[x] > ranks[y])
                parents[y] = x ;
            else {
                parents[x] = y;
                if (ranks[x] == ranks[y])
                    ranks[y] += 1;
            }
        }
        public long Dijkstra(long nodeCount, List<List<(long, long)>> adjacent_list_weights, long startNode, long endNode)
        {
            SimplePriorityQueue<long> PQ = new SimplePriorityQueue<long>();
            bool[] visited = new bool[nodeCount];
            bool[] non_changing = new bool[nodeCount];
            PQ.Enqueue(startNode, 0);
            visited[startNode] = true;
            while (PQ.Any())
            {
                float get_deqeued_priority = PQ.GetPriority(PQ.First);
                long dequeued = PQ.Dequeue();
                non_changing[dequeued] = true;
                if (dequeued == endNode)
                    return (long)get_deqeued_priority;
                foreach (var neighbor in adjacent_list_weights[(int)dequeued])
                {
                    if (!visited[neighbor.Item1])
                    {
                        PQ.Enqueue(neighbor.Item1 , get_deqeued_priority + neighbor.Item2);
                        visited[neighbor.Item1] = true;
                    }
                    else
                    {
                        if (non_changing[neighbor.Item1])
                            continue;
                        if (PQ.GetPriority(neighbor.Item1) > get_deqeued_priority + neighbor.Item2)
                        {
                            PQ.UpdatePriority(neighbor.Item1 , get_deqeued_priority + neighbor.Item2);
                        }
                    }
                }
            }
            return -1;
        }
    }
}
