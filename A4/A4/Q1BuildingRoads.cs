using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Priority_Queue;

namespace A4
{
    public class Q1BuildingRoads : Processor
    {
        public Q1BuildingRoads(string testDataName) : base(testDataName) {
         }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], double>)Solve);

        public double Solve(long pointCount, long[][] points)
        {
            long[] parents = new long[pointCount];
            long[] ranks = new long[pointCount];
            List<(long , long , double)> distances = new List<(long, long, double)>(); 
            for (int i = 0 ; i < pointCount ; i ++)
            {
                parents[i] = i;
                ranks[i] = 1;
                for ( int j = i+1 ; j < pointCount ; j++)
                {
                    distances.Add((i , j ,GetDistance(points[i] , points[j])));
                }
            }
            distances = distances.OrderBy(x => x.Item3).ToList();
            double s = 0 ;
            foreach (var edge in distances)
            {
                if (Find(edge.Item1 , parents) != Find(edge.Item2 , parents))
                {
                    Union(edge.Item1 , edge.Item2 , ranks , parents);
                    s += edge.Item3 ;
                }
            }
            return Math.Round(s , 6);
        }
        private double GetDistance (long[] first_point , long[] second_point)
        {
            return Math.Sqrt(Math.Pow((first_point[0] - second_point[0] ) , 2) + Math.Pow((first_point[1] - second_point[1] ) , 2) ); 
        } 
        private long Find(long u, long[] parents)
        {
            while (u != parents[u])
            {
                u = parents[u];
            }
            return parents[u];
        }
        private void Union(long x , long y , long[] ranks , long[] parents)
        {
            x = Find(x , parents);
            y = Find(y , parents);
            if (x == y)
                return;
            if (ranks[x] > ranks[y])
                parents[y] = x ;
            else {
                parents[x] = y;
                if (ranks[x] == ranks[y])
                    ranks[y] += 1;
            }
        }
    }
}
