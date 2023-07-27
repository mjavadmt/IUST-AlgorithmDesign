using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, string pattern)
        {
            if (pattern.Length > text.Length)
                return new long[] { -1 };
            List<long> result = new List<long>();
            string make_str = pattern + "$" + text;
            var prefix_array = compute_prefix_function(make_str);
            for (int i = pattern.Length + 1; i < make_str.Length; i++)
            {
                if (prefix_array[i] == pattern.Length)
                    result.Add(i - 2 * pattern.Length);
            }
            return result.ToArray();
        }
        private long[] compute_prefix_function(string p)
        {
            long[] s = new long[p.Length];
            long border = 0;
            for (int i = 1; i < p.Length; i++)
            {
                while (border > 0 && p[i] != p[(int)border])
                {
                    border = s[border - 1];
                }
                if (p[i] == p[(int)border])
                    border += 1;
                else
                    border = 0;
                s[i] = border;
            }
            return s;
        }
    }
}
