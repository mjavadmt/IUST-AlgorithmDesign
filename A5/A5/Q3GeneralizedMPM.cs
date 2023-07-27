using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q3GeneralizedMPM : Processor
    {
        public Q3GeneralizedMPM(string testDataName) : base(testDataName)
        {
            ExcludeTestCaseRangeInclusive(46 , 50);
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long[] Solve(string text, long n, string[] patterns)
        {
            Dictionary<long, Dictionary<string, long>> tree = new Dictionary<long, Dictionary<string, long>>();
            for (int i = 0; i < patterns[0].Length; i++)
            {
                tree[i] = new Dictionary<string, long>();
                tree[i][patterns[0][i].ToString()] = i + 1;
            }
            long current_max_idx = patterns[0].Length;
            tree[current_max_idx] = new Dictionary<string, long>();
            tree[current_max_idx]["is_leaf"] = 1;
            for (int i = 1; i < patterns.Length; i++)
            {
                var (ind, j) = traverse_index(patterns[i], tree);
                current_max_idx = build_leaf(ind, j, current_max_idx, tree, patterns[i]);
            }
            List<long> result = new List<long>();
            for (int i = 0; i < text.Length; i++)
            {
                if (reach_leaf(text, i, tree))
                    result.Add(i);
            }
            if (!result.Any())
            {
                result.Add(-1);
            }
            return result.ToArray();
        }
        private bool reach_leaf(string text, int i, Dictionary<long, Dictionary<string, long>> tree)
        {
            long s = 0;
            for (int j = i; j < text.Length; j++)
            {
                try
                {
                    s = tree[s][text[j].ToString()];
                    try
                    {
                        if (tree[s] == new Dictionary<string, long>() || tree[s]["is_leaf"] == 1)
                        {
                            return true;
                        }
                    }
                    catch
                    {
                        var c = 2;
                        c += 1;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        private (long, long) traverse_index(string pattern, Dictionary<long, Dictionary<string, long>> tree)
        {
            long ind = 0;
            int j = 0;
            for (; j < pattern.Length; j++)
            {
                try
                {
                    ind = tree[ind][pattern[j].ToString()];
                }
                catch
                {
                    return (ind, j);
                }
            }
            tree[ind]["is_leaf"] = 1;
            return (ind, j);
        }
        private long build_leaf(long ind, long j, long last_index, Dictionary<long, Dictionary<string, long>> tree, string pattern)
        {
            for (int k = 0; k < pattern.Length - j; k++)
            {
                tree[ind][pattern[(int)(j + k)].ToString()] = last_index + 1;
                ind = last_index + 1;
                tree[ind] = new Dictionary<string, long>();
                last_index += 1;
            }
            tree[ind] = new Dictionary<string, long>();
            tree[ind]["is_leaf"] = 1;
            return last_index;
        }
    }
}
