using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;
namespace E2
{
    public class Q2Manchester : Processor
    {
        public Q2Manchester(string testDataName) : base(testDataName)
        {
            ExcludeTestCases(1);
        }

        public override string Process(string inStr)
            => E2Processors.Q2Processor(inStr, Solve);

        /**
         * W[i] = Number of wins i-th team currently has.
         * R[i] = Total number of remaining games for i-th team.
         * G[i][j] = Number of upcoming games between teams i and j.
         * 
         * Should return whether the team can get first place.
         */
        public bool Solve(long[] W, long[] R, long[][] G)
        {
            int LastTeam = R.Length - 1;
            long TotalWinLastTeam = R[LastTeam] + W[LastTeam];
            long[] WinCapacityEachTeam = new long[W.Length - 1];
            for (int i = 0 ; i < W.Length ; i++)
            {
                if (W[i] > TotalWinLastTeam)
                    return false;
                WinCapacityEachTeam[i] = TotalWinLastTeam - W[i];
            }
            List<List<(int , int , int)>> adjacency = new List<List<(int , int , int)>>();
            long total_remaining = 0;
            foreach (var i in R)
            {
                total_remaining += i;
            } 
            total_remaining /= 2;
            for (int i = 0 ; i < total_remaining + R.Length + 2 ; i++)
            {
                adjacency.Add(new List<(int, int, int)>());
            }
            throw new NotImplementedException();
        }
    }
}
