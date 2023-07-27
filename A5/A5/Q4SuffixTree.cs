using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q4SuffixTree : Processor
    {
        public Q4SuffixTree(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
            ExcludeTestCaseRangeInclusive(30, 50);
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String[]>)Solve);

        public string[] Solve(string text)
        {
            Dictionary<long, Dictionary<string, long>> tree = new Dictionary<long, Dictionary<string, long>>();
            for (int i = 0; i < text.Length; i++)
            {
                tree[i] = new Dictionary<string, long>();
                tree[i][text[i].ToString()] = i + 1;
            }
            long current_max_idx = text.Length;
            tree[current_max_idx] = new Dictionary<string, long>();
            for (int i = 1; i < text.Length; i++)
            {
                var subtext = text.Substring(i);
                var (ind, j) = traverse_index(subtext, tree);
                current_max_idx = build_leaf(ind, j, current_max_idx, tree, subtext);
            }
            var new_tree = new Dictionary<long, Dictionary<string, long>>();
            ShortenTree(tree , 0 , new_tree);
            List<string> result = new List<string>();
            foreach ( var item in new_tree)
            {
                foreach (var str in new_tree[item.Key])
                {
                    result.Add(str.Key);
                }
            }
            return result.ToArray();
        }
        private void ShortenTree(Dictionary<long, Dictionary<string, long>> tree, long start_index , Dictionary<long, Dictionary<string, long>> new_tree)
        {
            new_tree[start_index] = new Dictionary<string, long>();
            foreach ( var i in tree[start_index])
            {
                var (idx , make_str) = how_far_can_go(tree , tree[start_index][i.Key]);
                new_tree[start_index][i.Key + make_str] = idx ;
                ShortenTree(tree , idx , new_tree);
            }
        }

        private (long , string) how_far_can_go(Dictionary<long, Dictionary<string, long>> tree, long ind)
        {
            string make_str = "" ;
            while (tree[ind].Keys.Count == 1)
            {
                make_str += new List<string>(tree[ind].Keys)[0];
                ind = tree[ind][new List<string>(tree[ind].Keys)[0]];
                if (tree[ind] == new Dictionary<string, long>())
                {
                    return (ind , make_str);
                }
            }
            return (ind , make_str);
        }

        private (long, long) traverse_index(string pattern ,Dictionary<long, Dictionary<string, long>> tree)
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
            return last_index;
        }
    }
}
