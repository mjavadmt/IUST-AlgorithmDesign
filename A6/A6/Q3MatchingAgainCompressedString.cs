using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName)
        : base(testDataName) {
            // ExcludeTestCases(1 , 2);
         }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        /// <summary>
        /// Implement BetterBWMatching algorithm
        /// </summary>
        /// <param name="text"> A string BWT(Text) </param>
        /// <param name="n"> Number of patterns </param>
        /// <param name="patterns"> Collection of n strings Patterns </param>
        /// <returns> A list of integers, where the i-th integer corresponds
        /// to the number of substring matches of the i-th member of Patterns
        /// in Text. </returns>
        public long[] Solve(string text, long n, String[] patterns)
        {
            Dictionary<char, int> repetition = new Dictionary<char, int>()
            {
                {'A' , 0} , {'C' , 0} , {'G' , 0} , {'T' , 0} , {'$' , 0}
            };
            Dictionary<char, bool> happened = new Dictionary<char, bool>()
            {
                {'A' , false} , {'C' , false} , {'G' , false} , {'T' , false} , {'$' , false}
            };
            Dictionary<char, int> FirstOcc = new Dictionary<char, int>();
            List<Dictionary<char, int>> CountArray = new List<Dictionary<char, int>>();
            MakeFirstOccurenceAndCount(CountArray,ref happened, ref FirstOcc, repetition, text);
            long[] result = new long[patterns.Length];
            int i = 0 ;
            foreach (var pattern in patterns)
            {
                int top = 0;
                int bottom = text.Length - 1;
                int last_ind = pattern.Length - 1;
                while (top <= bottom)
                {
                    if (last_ind >= 0)
                    {
                        char symbol = pattern[last_ind];
                        if (happened[symbol])
                        {
                            top = FirstOcc[symbol] + CountArray[top][symbol];
                            bottom = FirstOcc[symbol] + CountArray[bottom + 1][symbol] - 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        result[i] = bottom - top + 1;
                        break ;
                    }
                    last_ind -= 1;
                }
                if (last_ind >= 0)
                {
                    result[i] = 0;
                }
                i += 1;
            }
            return result;
        }

        private void MakeFirstOccurenceAndCount(List<Dictionary<char, int>> CountArray, ref Dictionary<char, bool> happened,
            ref Dictionary<char, int> FirstOcc, Dictionary<char, int> repetition, string LastColumn)
        {
            for (int i = 0; i < LastColumn.Length; i++)
            {
                CountArray.Add(repetition);
                repetition = new Dictionary<char, int>(repetition);
                repetition[LastColumn[i]] += 1;
                happened[LastColumn[i]] = true ;
            }
            CountArray.Add(repetition);
            FirstOcc = new Dictionary<char, int>(){
                {'$' , 0} , {'A' , 1} , {'C' , repetition['A'] + 1} , {'G' , repetition['C'] + repetition['A'] + 1},
                {'T' , repetition['G'] + repetition['C'] + repetition['A'] + 1}
            };
        }
    }
}
