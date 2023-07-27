using System;
using System.Collections.Generic;
using System.Linq;
using System.IO ;
namespace A6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Dictionary<char, int> repetition = new Dictionary<char, int>()
            {
                {'A' , 0} , {'C' , 0} , {'G' , 0} , {'T' , 0} , {'$' , 0}
            };
            List<Dictionary<char, int>> CountArray = new List<Dictionary<char, int>>();
            CountArray.Add(repetition);
            repetition = new Dictionary<char, int>(repetition);
            repetition['C'] += 1;
            System.Console.WriteLine();
            Q2ReconstructStringFromBWT q2 = new Q2ReconstructStringFromBWT("");
            var answer = q2.Solve(File.ReadAllText(@"C:\git\AD99002\A6\A6.Tests\TestData\A6.TD2\In_40.txt").Trim());
            System.Console.WriteLine();
        }
        
        
    }
}
