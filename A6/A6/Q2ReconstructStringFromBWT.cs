using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace A6
{
    public class Q2ReconstructStringFromBWT : Processor
    {
        public Q2ReconstructStringFromBWT(string testDataName)
        : base(testDataName)
        {
            ExcludeTestCaseRangeInclusive(31, 40);
        }
        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Reconstruct a string from its Burrows–Wheeler transform
        /// </summary>
        /// <param name="bwt"> A string Transform with a single “$” sign </param>
        /// <returns> The string Text such that BWT(Text) = Transform.
        /// (There exists a unique such string.) </returns>
        public string Solve(string bwt)
        {
            Dictionary<char, int> repetition = new Dictionary<char, int>()
            {
                {'A' , 0} , {'C' , 0} , {'G' , 0} , {'T' , 0} , {'$' , 0}
            };
            List<string> bwt_last_column = new List<string>();
            foreach (var c in bwt)
            {
                bwt_last_column.Add($"{c}{repetition[c]}");
                repetition[c] += 1;
            }
            int idx = 0;
            StringBuilder make_str = new StringBuilder();
            for (int i = 0; i < bwt.Length - 1; i++)
            {
                var s = bwt_last_column[idx];
                make_str.Append(s[0]);
                if (s[0] == 'A')
                {
                    idx = repetition['$'] + int.Parse(s.Substring(1));
                }
                else if (s[0] == 'C')
                {
                    idx = repetition['$'] + repetition['A'] + int.Parse(s.Substring(1));
                }
                else if (s[0] == 'G')
                {
                    idx = repetition['$'] + repetition['A'] + repetition['C'] + int.Parse(s.Substring(1));
                }
                else if (s[0] == 'T')
                {
                    idx = repetition['$'] + repetition['A'] + repetition['C'] + repetition['G'] + int.Parse(s.Substring(1));
                }
            }

            return new string(make_str.ToString().Reverse().ToArray()) + "$";
        }
    }
}
