using System;
using System.Collections.Generic;
using System.Linq;
namespace A8
{
    class Program
    {
        static void Main(string[] args)
        {
            List<(long , long)> a = new List<(long, long)>();
            var answ = a.FirstOrDefault(x => x.Item1 == 2);
            Console.WriteLine("Hello World!");
        }
    }
}
