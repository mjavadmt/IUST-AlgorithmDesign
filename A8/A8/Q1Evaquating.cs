using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName) {
            ExcludeTestCases(48 , 49 , 50 ,51 ,52 , 53, 54);
            // ExcludeTestCaseRangeInclusive(1,33);
         }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            List<List<VCF>> AdjacentVCF = new List<List<VCF>>();
            for (long i = 0; i < nodeCount; i++)
            {
                AdjacentVCF.Add(new List<VCF>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                VCF edge = new VCF((int)edges[i][1] - 1, (int)edges[i][2], 0);
                var answ = AdjacentVCF[(int)edges[i][0] - 1].FirstOrDefault(x => x.vertex == edge.vertex);
                if (answ == null)
                    AdjacentVCF[(int)edges[i][0] - 1].Add(edge);
                else
                    answ.capacity += (int)edges[i][2];
            }
            long f = 0;
            while (true)
            {
                var ResidualGraph = MakeResidualGraph(AdjacentVCF);
                var FindPath = BFS(ResidualGraph , 0 , nodeCount - 1);
                if (FindPath.Item1.Count == 0)
                    return f;
                f += FindPath.Item2 ;
                AddFlow(AdjacentVCF , FindPath.Item1 , (int)FindPath.Item2);
            }
        }
        public void AddFlow(List<List<VCF>> AdjacentVCF , List<(long , long)> path , int FlowToAdd)
        {
            for (int i = 0 ; i < path.Count - 1 ; i++)
            {
                var FoundNeighbour = AdjacentVCF[(int)path[i].Item1].FirstOrDefault(x => x.vertex == path[i+1].Item1);
                if (FoundNeighbour == null)
                {
                    FoundNeighbour = AdjacentVCF[(int)path[i + 1].Item1].FirstOrDefault(x => x.vertex == path[i].Item1);
                    FoundNeighbour.flow -= FlowToAdd;
                    // i dont know why += and -= doesn't differ here
                }
                else
                {
                    FoundNeighbour.flow += FlowToAdd;
                }
                
            }
        } 
        public List<List<(long , long)>> MakeResidualGraph(List<List<VCF>> AdjacentVCF)
        {
            List<List<(long , long)>> AdjacentWeights = new List<List<(long, long)>>();
            for (long i = 0; i < AdjacentVCF.Count; i++)
            {
                AdjacentWeights.Add(new List<(long , long)>());
            }
            for (int i = 0; i < AdjacentVCF.Count; i++)
            {
                foreach(var edge in AdjacentVCF[i])
                {
                    long ForwardEdge = edge.capacity - edge.flow ;
                    long BackwardEdge = edge.flow ;
                    if (ForwardEdge != 0)
                    {
                        var found_index = FindItem(AdjacentWeights , i , edge.vertex);
                        if (found_index == -1)
                            AdjacentWeights[i].Add((edge.vertex , ForwardEdge));
                        else
                        {
                            var f = AdjacentWeights[i][found_index] ;
                            AdjacentWeights[i][found_index] = (f.Item1 , f.Item2 + ForwardEdge);
                        }
                            
                    }
                    if (BackwardEdge != 0)
                    {
                        var found_index = FindItem(AdjacentWeights , edge.vertex , i);
                        if (found_index == -1)
                            AdjacentWeights[edge.vertex].Add((i , BackwardEdge));
                        else
                        {
                            var f = AdjacentWeights[edge.vertex][found_index] ;
                            AdjacentWeights[edge.vertex][found_index] = (f.Item1 , f.Item2 + BackwardEdge);
                        }
                    }
                }
            }
            return AdjacentWeights;
        }
        int FindItem(List<List<(long , long)>> AdjacentWeights , int locindex , int target)
        {
            for (int i = 0 ; i < AdjacentWeights[locindex].Count ; i++)
            {
                if (AdjacentWeights[locindex][i].Item1 == target)
                    return i;
            }
            return -1;
        }
        public (List<(long , long)> , long) BFS(List<List<(long,long)>> adjacent_list, long startnode, long endnode)
        {
            int[] distance = Enumerable.Repeat(int.MaxValue , adjacent_list.Count).ToArray();
            Queue<long> vertexes = new Queue<long>();
            (long , long)[] parent = new (long , long)[adjacent_list.Count];
            vertexes.Enqueue(startnode);
            parent[startnode] = (startnode , 0);
            distance[startnode] = 0;
            while (vertexes.Any())
            {
                long dequeued = vertexes.Dequeue();
                foreach (var item in adjacent_list[(int)dequeued])
                {
                    if (distance[item.Item1] == int.MaxValue)
                    {
                        vertexes.Enqueue(item.Item1);
                        distance[item.Item1] = distance[dequeued] + 1;
                        parent[item.Item1] = (dequeued , item.Item2);
                    }
                    if (item.Item1 == endnode)
                    {
                        return BacktrackPath(parent , startnode , endnode);
                    }
                }
            }
            return (new List<(long , long)>() , 0);
        }
        public (List<(long ,long)>,long) BacktrackPath((long, long)[] parent , long startnode , long endnode)
        {
            List<(long,long)> path = new List<(long , long)>();
            long MinValue = int.MaxValue;
            while(parent[endnode].Item1 != endnode)
            {
                path.Add((endnode , parent[endnode].Item2));
                if (parent[endnode].Item2 < MinValue)
                    MinValue = parent[endnode].Item2;
                endnode = parent[endnode].Item1;
            }
            path.Add((startnode , 0 ));
            path.Reverse();
            return (path , MinValue);
        }

        

    }

    public class VCF
    {
        public int vertex;
        public long capacity;
        public long flow;
        public VCF(int v, int c, int f)
        {
            vertex = v;
            capacity = c;
            flow = f;
        }
        public override string ToString()
        {
            return $"v:{vertex} c:{capacity} f:{flow}";
        }
    }
}
