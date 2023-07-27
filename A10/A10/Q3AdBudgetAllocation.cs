using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;

namespace A10
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName) : base(testDataName)
        {
            // ExcludeTestCases(1);
            // ExcludeTestCaseRangeInclusive(15,50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            List<string> BinaryAssignment = new List<string>() { "000", "001", "010", "011", "100", "101", "110", "111" };
            HashSet<string> clauses = new HashSet<string>();
            for (int i = 0; i < A.Length; i++)
            {
                foreach (var assignment in BinaryAssignment)
                {
                    int tmp = 0;
                    int sum = 0;
                    List<int> NonZero = new List<int>();
                    for (int j = 0; j < A[i].Length; j++)
                    {
                        if (A[i][j] != 0)
                        {
                            NonZero.Add(j + 1);
                            sum += (assignment[tmp] - '0') * (int)A[i][j];
                            tmp += 1;
                        }
                    }
                    if (sum > b[i])
                    {
                        string MakeClause = "";
                        int counter = 0;
                        foreach (var index in NonZero)
                        {
                            // MakeClause += assignment[counter] == '1' ? $"{index} " : $"-{index} ";
                            // doesnt differ from 
                            MakeClause += assignment[counter] == '1' ? $"-{index} " : $"{index} ";

                            counter += 1;
                        }
                        MakeClause += "0";
                        if (!clauses.Contains(MakeClause))
                            clauses.Add(MakeClause);
                    }
                }
            }
            // -1x1 + 3x2 + 4x3 <= 2
            // invalidate inequality
            // 0 1 1 -> not(not x1 and x2 and x3)
            // it means that 0 1 1 doesnt hold in inequality so we should add not this CNF that this 
            // doesnt happen
            
            var new_clauses = clauses.ToList();
            new_clauses.Insert(0, $"{clauses.Count} {varCount * 2} 0");
            return new_clauses.ToArray();
        }
    }
}
