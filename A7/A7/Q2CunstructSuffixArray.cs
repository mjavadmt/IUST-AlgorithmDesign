using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q2CunstructSuffixArray : Processor
    {
        public Q2CunstructSuffixArray(string testDataName) : base(testDataName)
        {
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        protected virtual long[] Solve(string text)
        {
            return buildsuffixarray(text);
        }
        long[] SortCharactares(string s)
        {
            long[] orders = new long[s.Length];
            long[] count = new long[5];
            Dictionary<char, int> map = new Dictionary<char, int>{
                {'$' , 0} , {'A' , 1} , {'C' , 2} , {'G' , 3} , {'T' , 4}
            };
            for (int i = 0; i < s.Length; i++)
            {
                count[map[s[i]]] = count[map[s[i]]] + 1;
            }
            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }
            for (int i = s.Length - 1; i >= 0; i--)
            {
                var c = map[s[i]];
                count[c] -= 1;
                orders[count[c]] = i;
            }
            return orders;
        }
        long[] ComputeCharClasses(string s, long[] order)
        {
            long[] classified = new long[s.Length];
            classified[order[0]] = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (s[(int)order[i]] != s[(int)order[i - 1]])
                {
                    classified[order[i]] = classified[order[i - 1]] + 1;
                }
                else
                {
                    classified[order[i]] = classified[order[i - 1]];
                }
            }
            return classified;
        }
        long[] SortDoubled(string s, long l, long[] order, long[] classified)
        {
            long[] count = new long[s.Length];
            long[] neworder = new long[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                count[classified[i]] += 1;
            }
            for (int i = 1; i < s.Length; i++)
            {
                count[i] += count[i - 1];
            }
            for (int i = s.Length - 1; i >= 0; i--)
            {
                var start = (order[i] - l + s.Length) % s.Length;
                var cl = classified[start];
                count[cl] -= 1;
                neworder[count[cl]] = start;
            }
            return neworder;
        }
        long[] updateclasses(long[] neworder , long[] classified , long l)
        {
            var n = neworder.Length ;
            long[] newclass = new long[n];
            newclass[neworder[0]] = 0;
            for (int i = 1 ; i < n ; i++)
            {
                var cur = neworder[i] ;
                var prev = neworder[i - 1];
                var mid = (cur + l ) % n;
                var midprev = (prev + l ) % n;
                if (classified[cur] != classified[prev] || classified[mid] != classified[midprev])
                {
                    newclass[cur] = newclass[prev] + 1 ;
                }
                else
                {
                    newclass[cur] = newclass[prev] ;
                }
            }
            return newclass;
        }
        public long[] buildsuffixarray(string s)
        {
            var order = SortCharactares(s);
            var classified = ComputeCharClasses(s , order);
            var l = 1 ;
            while ( l < s.Length)
            {
                order = SortDoubled(s , l , order , classified);
                classified = updateclasses(order , classified , l);
                l *= 2;
            }
            return order;
        }
    }
}
