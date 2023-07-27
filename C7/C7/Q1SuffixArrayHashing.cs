using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace C7
{
    public class Q1SuffixArrayHashing : Processor
    {
        const int MOD = 1_000_000_000 + 9;
        const int P = 31;

        public Q1SuffixArrayHashing(string testDataName) : base(testDataName)
        {
            ExcludeTestCaseRangeInclusive(9,12);
        }

        public override string Process(string inStr) => TestTools.Process(inStr, (Func<String, long[]>)Solve);

        public static long[] BuildHash(string text)
        {
            long p = 31;
            long m = (long)Math.Pow(10, 9) + 9;
            long[] prefix_hash = new long[text.Length];
            prefix_hash[0] = text[0] - '`';
            for (int i = 1; i < text.Length; i++)
            {
                prefix_hash[i] = (prefix_hash[i - 1] + (text[i] - '`') * (long)Math.Pow(p, i)) % m;
            }
            return prefix_hash;
        }

        public long[] Solve(string text)
        {
            text += "$";
            List<(string, long)> text_clockwised = new List<(string, long)>();
            text_clockwised.Add((text, 0));
            int i = text.Length - 1;
            while (i != 0)
            {
                text_clockwised.Add((text.Substring(i) + text.Substring(0, i), i));
                i -= 1;
            }
            return text_clockwised.OrderBy(x => x.Item1).Select(x => x.Item2).ToArray();
        }

        private bool equal_hash(int i, int j, int length, long[] prefix_hash)
        {
            return prefix_hash[i + length] - prefix_hash[i - 1] == prefix_hash[j + length] - prefix_hash[j - 1];
        }

    }
}
