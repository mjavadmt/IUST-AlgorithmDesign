using System;
using System.Collections.Generic;
using System.Linq;

namespace A9
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            double s = -0.94;
            int f = (int)s;
            System.Console.WriteLine();
            int[] get_sub = new int[]{1,2,3,4,5,6};
            var subsets = SubSetsOf(get_sub.ToList()).ToList();
            var ans = subsets.Where(x => x.Count == 3).ToList();
            System.Console.WriteLine();
        }
        public static List<List<T>> SubSetsOf<T>(List<T> source)
        {
            if (!source.Any())
                return Enumerable.Repeat(Enumerable.Empty<T>().ToList(), 1).ToList();

            var element = source.Take(1);

            var haveNots = SubSetsOf(source.Skip(1).ToList());
            var haves = haveNots.Select(set => element.Concat(set).ToList()).ToList();

            return haves.Concat(haveNots).ToList();
        }
    }
}
