using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;
using System.IO;
namespace C5
{
    public class Q1LazyTypist : Processor
    {
        public Q1LazyTypist(string testDataName) : base(testDataName) { 
        }

        public override string Process(string inStr)
        {
            var lines = inStr.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            return Solve(long.Parse(lines[0]), lines.Skip(1).ToArray()).ToString();
        }

        public long Solve(long n, string[] words)
        {
            Dictionary<long, Dictionary<string, long>> tree = new Dictionary<long, Dictionary<string, long>>();
            long count = 0;
            long max_hieght = words.Max(x => x.Length);
            for (int i = 0; i < words[0].Length; i++)
            {
                tree[i] = new Dictionary<string, long>();
                tree[i][words[0][i].ToString()] = i + 1;
                // count += 1 ;
            }
            long current_max_idx = words[0].Length;
            tree[current_max_idx] = new Dictionary<string, long>();
            for (int i = 1; i < words.Length; i++)
            {
                var (ind, j) = traverse_index(words[i], tree);
                // count += words[i].Length - j ;
                current_max_idx = build_leaf(ind, j, current_max_idx, tree, words[i]);
            }
            Queue<long> vertexes = new Queue<long>();
            vertexes.Enqueue(0);
            while (vertexes.Any())
            {
                var current_node = vertexes.Dequeue();
                foreach (var item in tree[current_node])
                {
                    vertexes.Enqueue(item.Value);
                    count += 1;
                }
            }
            return count * 2 - max_hieght + words.Length ;
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
            // count -= 1;
            for (int k = 0; k < pattern.Length - j; k++)
            {
                tree[ind][pattern[(int)(j + k)].ToString()] = last_index + 1;
                ind = last_index + 1;
                tree[ind] = new Dictionary<string, long>();
                last_index += 1;
                // count += 1;
            }
            tree[ind] = new Dictionary<string, long>();
            return last_index;
        }
    }
}
