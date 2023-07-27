using System;
using TestCommon;
using System.Collections.Generic ;
using System.Linq;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Construct the Burrows–Wheeler transform of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> BWT(Text) </returns>
        public string Solve(string text)
        {
            List<string> text_clockwised = new List<string>();
            text_clockwised.Add(text);
            int i = text.Length - 1 ;
            while (i != 0)
            {
                text_clockwised.Add(text.Substring(i) + text.Substring(0 , i));
                i -= 1;
            } 
            return new string(text_clockwised.OrderBy(x => x).Select(x => x[x.Length - 1]).ToArray());
        }
    }
}
