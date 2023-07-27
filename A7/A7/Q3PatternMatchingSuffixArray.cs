using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q3PatternMatchingSuffixArray : Processor
    {
        public Q3PatternMatchingSuffixArray(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, string[], long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, long n, string[] patterns)
        {
            text = text + "$";
            Q2CunstructSuffixArray q2 = new Q2CunstructSuffixArray("Q2");
            var suff_array = q2.buildsuffixarray(text);
            HashSet<long> matched_indexes = new HashSet<long>();
            foreach (var p in patterns)
            {
                var min_index = 0;
                var max_index = text.Length;
                while (min_index < max_index)
                {
                    int middle_index = (int)((min_index + max_index) / 2);
                    int suffix_index = (int)suff_array[middle_index];
                    int compare = p.CompareTo(text.Substring(suffix_index));
                    if (compare > 0)
                    {
                        min_index = middle_index + 1;
                    }
                    else if (compare == 0)
                    {
                        break;
                    }
                    else
                    {
                        max_index = middle_index;
                    }
                }
                int found_min_index = min_index;
                while (min_index > 0)
                {
                    if (text.Length - suff_array[min_index - 1] - 1 < p.Length)
                        break;
                    if (text.Substring((int)suff_array[min_index - 1], p.Length) != p)
                        break;
                    min_index--;
                }
                max_index = found_min_index;
                while (max_index != suff_array.Length)
                {
                    if (text.Length - suff_array[max_index] - 1 < p.Length)
                        break;
                    if (text.Substring((int)suff_array[max_index], p.Length) != p)
                        break;
                    max_index++;
                }
                for (int i = min_index; i < max_index; i++)
                {
                    matched_indexes.Add(suff_array[i]);
                }
            }
            if (matched_indexes.Count == 0) return new long[] { -1 };
            return matched_indexes.ToArray();
        }
    }
}
