using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace C6
{
    public class Q1BoyerMoore : Processor
    {
        public Q1BoyerMoore(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, string pattern)
        {
            var answer = Q1BoyerMoore.bad_character(text.ToCharArray(), pattern.ToCharArray()).ToArray();
            return answer.Length == 0 ? new long[] { -1 } : answer;
        }

        static List<long> bad_character(char[] text, char[] pattern)
        {
            int pattern_length = pattern.Length;
            int text_length = text.Length;
            Dictionary<char, int> last_occurence = new Dictionary<char, int>(){
                {'A' , -1} , {'C' , -1} , {'G' , -1} , {'T' , -1}
            };
            for (int i = pattern_length - 1; i >= 0; i--)
            {
                if (pattern[i] == 'A' && last_occurence['A'] == -1)
                {
                    last_occurence['A'] = i;
                }
                else if (pattern[i] == 'C' && last_occurence['C'] == -1)
                {
                    last_occurence['C'] = i;
                }
                else if (pattern[i] == 'G' && last_occurence['G'] == -1)
                {
                    last_occurence['G'] = i;
                }
                else if (pattern[i] == 'T' && last_occurence['T'] == -1)
                {
                    last_occurence['T'] = i;
                }
            }
            List<long> result = new List<long>();
            int current_shift = 0;
            while (current_shift <= (text_length - pattern_length))
            {
                int index_of_matched = pattern_length - 1;
                while (index_of_matched >= 0 && pattern[index_of_matched] == text[current_shift + index_of_matched])
                    index_of_matched--;

                if (index_of_matched < 0)
                {
                    result.Add(current_shift);
                    if (current_shift + pattern_length < text_length)
                    {
                        current_shift += pattern_length - last_occurence[text[current_shift + pattern_length]];
                    }
                    else
                    {
                        current_shift += 1;
                    }
                }
                else
                    current_shift += Math.Max(1, index_of_matched - last_occurence[text[current_shift + index_of_matched]]);
            }
            return result;
        }
        void good_suffix(char[] pattern, char[] text)
        {
            int current_index = 0;
            List<long> subindex_list = new List<long>();
            while (current_index + pattern.Length <= text.Length)
            {
                int from_bottom = current_index + pattern.Length;
                int pattern_end = pattern.Length;
                while (text[from_bottom] == pattern[pattern_end])
                {
                    from_bottom--;
                    pattern_end--;
                }
                if (pattern_end == -1)
                {
                    subindex_list.Add(current_index);
                    current_index++;
                }
                else
                {
                    int substring_index = case_1(pattern, pattern_end);
                    if (substring_index != -1)
                    {
                        current_index += (pattern_end + 1) - substring_index;
                        continue ;
                    }
                    substring_index = case_2(pattern , pattern_end);
                    if (substring_index != -1)
                    {
                        current_index += substring_index ;
                        continue;
                    }
                    else
                        current_index += pattern.Length ;
                }
            }
        }

        int case_1(char[] pattern, int stopped)
        {
            int matched = stopped;
            int pattern_length = pattern.Length;
            int equal_length = pattern_length - stopped;
            while (stopped >= equal_length)
            {
                int move = stopped;
                bool is_equal = true;
                int counter = 1;
                for (int j = stopped - equal_length; j < stopped; j++)
                {
                    if (pattern[j] != pattern[matched + counter])
                    {
                        is_equal = false;
                        break;
                    }
                    counter++;
                }
                if (is_equal)
                    return stopped - equal_length;
                stopped--;
            }
            // return the index which exactly matches that suffix
            return -1;
        }
        int case_2(char[] pattern, int stopped)
        {
            int start = stopped + 1;
            while (start < pattern.Length)
            {
                bool is_equal = true;
                for (int j = 0; j < pattern.Length - start; j++)
                {
                    if (pattern[j] != pattern[start + j])
                    {
                        is_equal = false;
                        break;
                    }
                }
                if (is_equal)
                {
                    return start;
                }
                start++;
            }
            // return index which is prefix equal to suffix of t 
            return -1;
        }

    }
}
