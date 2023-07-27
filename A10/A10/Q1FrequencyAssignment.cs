using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;

namespace A10
{
    public class Q1FrequencyAssignment : Processor
    {
        public Q1FrequencyAssignment(string testDataName) : base(testDataName)
        {

        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);


        public String[] Solve(int V, int E, long[,] matrix)
        {
            List<string> clauses = new List<string>();
            OnlyOneOfColorAssign(V, clauses);
            for (int i = 0; i < matrix.Length / 2; i++)
            {
                NotSameColorAdjEdges(matrix[i, 0], matrix[i, 1], clauses);
            }
            clauses.Insert(0, $"{clauses.Count} {V * 3}");
            return clauses.ToArray();
        }
        private void NotSameColorAdjEdges(long LeftVertex, long RightVertex, List<string> clauses)
        {
            // both can't be red => ~(xir and xjr) => (~xir or ~xjr )
            clauses.Add($"-{(LeftVertex - 1) * 3 + 1}  -{(RightVertex - 1) * 3 + 1} 0");
            // both can't be green => ~(xig and xjg) => (~xig or ~xjg )
            clauses.Add($"-{(LeftVertex - 1) * 3 + 2}  -{(RightVertex - 1) * 3 + 2} 0");
            // both can't be blue => ~(xib and xjb) => (~xib or ~xjb )
            clauses.Add($"-{(LeftVertex - 1) * 3 + 3}  -{(RightVertex - 1) * 3 + 3} 0");
        }
        private void OnlyOneOfColorAssign(int V, List<string> clauses)
        {
            for (int i = 0; i < V; i++)
            {
                string r = $"{i * 3 + 1}";
                string g = $"{i * 3 + 2}";
                string b = $"{i * 3 + 3}";
                string rgb = $"{r} {g} {b} 0";
                clauses.Add(rgb);
                // bottom lines are not neceassary don't know why
                // clauses.Add($"-{r} -{g} 0");
                // clauses.Add($"-{r} -{b} 0");
                // clauses.Add($"-{g} -{b} 0");
            }
        }

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

    }
}
