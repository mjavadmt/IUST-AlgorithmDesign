using System;
using System.Collections.Generic;
using TestCommon;
using System.Linq;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName)
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        /// <summary>
        /// Construct the suffix array of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> SuffixArray(Text), that is, the list of starting positions
        /// (0-based) of sorted suffixes separated by spaces </returns>
        public long[] Solve(string text)
        {
            List<(string, long)> text_clockwised = new List<(string , long)>();
            text_clockwised.Add((text , 0));
            int i = text.Length - 1;
            while (i != 0)
            {
                text_clockwised.Add((text.Substring(i) + text.Substring(0, i) , i));
                i -= 1;
            }
            return text_clockwised.OrderBy(x => x.Item1).Select(x => x.Item2).ToArray();
        }
    }
}
