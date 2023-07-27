using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q2DetectingAnomalies : Processor
    {
        public Q2DetectingAnomalies(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long[] dist;

        public long Solve(long nodeCount, long[][] edges)
        {
            List<List<long>> adjacent_list = new List<List<long>>();
            List<List<long>> edges_weight = new List<List<long>>();
            for (long i = 0; i < nodeCount; i++)
            {
                adjacent_list.Add(new List<long>());
                edges_weight.Add(new List<long>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
                edges_weight[(int)edges[i][0] - 1].Add((int)edges[i][2]);
            }
            bool[] reachable = new bool[nodeCount];
            bool[] infinite_arbitrage = new bool[nodeCount];
            int[] distance = Enumerable.Repeat(int.MaxValue, (int)nodeCount).ToArray();
            distance[0] = 0;
            for (int repetition = 0; repetition < nodeCount + 5; repetition++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    for (int k = 0; k < adjacent_list[j].Count; k++)
                    {
                        if (distance[adjacent_list[j][k]] > distance[j] + edges_weight[j][k])
                        {
                            distance[adjacent_list[j][k]] = distance[j] + (int)edges_weight[j][k];
                            if (repetition >= nodeCount - 1)
                            {
                                return 1;
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }
}
