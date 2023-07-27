using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q3ExchangingMoney : Processor
    {
        public Q3ExchangingMoney(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, string[]>)Solve);

        public string[] Solve(long nodeCount, long[][] edges, long startNode)
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
            distance[startNode - 1] = 0;
            reachable[startNode - 1] = true;
            for (int repetition = 0; repetition < nodeCount + 5; repetition++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    if (reachable[j])
                    {
                        for (int k = 0; k < adjacent_list[j].Count; k++)
                        {
                            reachable[adjacent_list[j][k]] = true;
                            if (distance[adjacent_list[j][k]] > distance[j] + edges_weight[j][k])
                            {
                                distance[adjacent_list[j][k]] = distance[j] + (int)edges_weight[j][k];
                                if (repetition >= nodeCount - 1)
                                {
                                    infinite_arbitrage[adjacent_list[j][k]] = true;
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < nodeCount; i++)
            {
                if (infinite_arbitrage[i])
                {
                    foreach (var neighbor in adjacent_list[i])
                    {
                        if (!infinite_arbitrage[neighbor])
                            DFS(neighbor, adjacent_list, infinite_arbitrage);
                    }
                }
            }
            List<string> answers = new List<string>();
            for (int i = 0; i < nodeCount; i++)
            {
                if (infinite_arbitrage[i])
                    answers.Add("-");
                else if (! reachable[i])
                    answers.Add("*");
                else
                    answers.Add(distance[i].ToString());
            }
            return answers.ToArray();
        }
        private void DFS(long startnode, List<List<long>> adj_list, bool[] infinite_arbitrage)
        {
            infinite_arbitrage[startnode] = true;
            foreach (var neighbor in adj_list[(int)startnode])
            {
                if (!infinite_arbitrage[startnode])
                {
                    DFS(neighbor, adj_list, infinite_arbitrage);
                }
            }
        }

    }
}
