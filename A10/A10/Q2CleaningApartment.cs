using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;
namespace A10
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        {
            // ExcludeTestCases()
            // ExcludeTestCaseRangeInclusive(10, 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public String[] Solve(int V, int E, long[,] matrix)
        {
            List<string> clauses = new List<string>();
            List<(int, int)> NonConnected = new List<(int, int)>();
            EachVertexBelongsPath(V, clauses);
            EachVertexAppearsOnce(V, clauses);
            EachPathBelongsVertex(V, clauses);
            EachPositionOccupiedOnce(V, clauses);
            long[,] AdjacencyMatrix = ADJMatrix(V, E, matrix);
            ExtractNonConnected(V, AdjacencyMatrix, NonConnected);
            NoSuccessiveNonConnected(V, NonConnected, clauses);
            clauses.Insert(0, $"{clauses.Count} {V * V * 2}");
            return clauses.ToArray();
        }

        private void ExtractNonConnected(int V, long[,] AdjacencyMatrix, List<(int, int)> NonConnected)
        {
            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    if (i == j)
                        continue;
                    if (AdjacencyMatrix[i, j] == 0)
                        NonConnected.Add((i, j));
                }
            }
        }
        private void NoSuccessiveNonConnected(int V, List<(int, int)> NonConnected, List<string> clauses)
        {
            for (int k = 1; k <= V - 1; k++)
            {
                foreach (var nc in NonConnected)
                {
                    clauses.Add($"-{(nc.Item1 + 1) * V + k} -{(nc.Item2 + 1) * V + (k + 1)} 0");
                }
            }
        }

        private static long[,] ADJMatrix(int V, int E, long[,] matrix)
        {
            long[,] AdjacencyMatrix = new long[V, V];
            for (int i = 0; i < E; i++)
            {
                AdjacencyMatrix[matrix[i, 0] - 1, matrix[i, 1] - 1] = 1;
                AdjacencyMatrix[matrix[i, 1] - 1, matrix[i, 0] - 1] = 1;
            }

            return AdjacencyMatrix;
        }

        private void EachVertexBelongsPath(int v, List<string> clauses)
        {
            for (int i = 1; i <= v; i++)
            {
                string each_vertex = "";
                for (int j = 1; j <= v; j++)
                {
                    each_vertex += $"{i * v + j} ";
                }
                each_vertex += "0";
                clauses.Add(each_vertex);
            }
        }
        private void EachVertexAppearsOnce(int v, List<string> clauses)
        {
            for (int i = 1; i <= v; i++)
            {
                for (int j = 1; j <= v; j++)
                {
                    for (int k = j + 1; k <= v; k++)
                    {
                        clauses.Add($"-{i * v + j} -{i * v + k} 0");
                    }
                }
            }
        }
        private void EachPathBelongsVertex(int v, List<string> clauses)
        {
            for (int i = 1; i <= v; i++)
            {
                string each_vertex = "";
                for (int j = 1; j <= v; j++)
                {
                    each_vertex += $"{j * v + i} ";
                }
                each_vertex += "0";
                clauses.Add(each_vertex);
            }
        }
        private void EachPositionOccupiedOnce(int v, List<string> clauses)
        {
            for (int i = 1; i <= v; i++)
            {
                for (int j = 1; j <= v; j++)
                {
                    for (int k = j + 1; k <= v; k++)
                    {
                        clauses.Add($"-{j * v + i} -{k * v + i} 0");
                    }
                }
            }
        }
    }
}
