using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q1ConstructTrie : Processor
    {
        public Q1ConstructTrie(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, String[], String[]>) Solve);

        public string[] Solve(long n, string[] patterns)
        {
            Dictionary<long, Dictionary<string, long>> tree = new Dictionary<long, Dictionary<string, long>>();
            for (int i = 0; i < patterns[0].Length; i++)
            {
                tree[i] = new Dictionary<string, long>();
                tree[i][patterns[0][i].ToString()] = i + 1;
            }
            long current_max_idx = patterns[0].Length;
            tree[current_max_idx] = new Dictionary<string, long>();
            for (int i = 1; i < patterns.Length; i++)
            {
                var (ind, j) = traverse_index(patterns[i], tree);
                current_max_idx = build_leaf(ind, j, current_max_idx, tree, patterns[i]);
            }
            List<string> result = new List<string>();
            foreach (var node in tree)
            {
                foreach (var c in tree[node.Key])
                {
                    result.Add($"{node.Key}->{tree[node.Key][c.Key]}:{c.Key}");
                }
            }
            return result.ToArray();
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
            return last_index;
        }
    }
}
