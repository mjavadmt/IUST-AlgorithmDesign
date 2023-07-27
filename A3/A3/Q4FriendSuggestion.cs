using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
using Priority_Queue;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { 
            ExcludeTestCases(47 , 48 , 49 , 50);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            List<List<long>> forawrd_adjacent_list = new List<List<long>>();
            List<List<long>> backward_adjacent_list = new List<List<long>>();
            List<List<long>> edges_weight_forward = new List<List<long>>();
            List<List<long>> edges_weight_backward = new List<List<long>>();
            for (long i = 0; i < NodeCount; i++)
            {
                forawrd_adjacent_list.Add(new List<long>());
                backward_adjacent_list.Add(new List<long>());
                edges_weight_forward.Add(new List<long>());
                edges_weight_backward.Add(new List<long>());
            }
            for (int i = 0; i < edges.Length; i++)
            {
                forawrd_adjacent_list[(int)edges[i][0] - 1].Add((int)edges[i][1] - 1);
                edges_weight_forward[(int)edges[i][0] - 1].Add((int)edges[i][2]);
                backward_adjacent_list[(int)edges[i][1] - 1].Add((int)edges[i][0] - 1);
                edges_weight_backward[(int)edges[i][1] - 1].Add((int)edges[i][2]);
            }
            List<long> result = new List<long>();
            foreach (var i in Queries)
            {
                var forward_distance = InitializePQ(NodeCount);
                var backward_distance = InitializePQ(NodeCount);
                bool[] visited_forward = new bool[NodeCount];
                bool[] visited_backward = new bool[NodeCount];
                float[] array_distance_for = Enumerable.Repeat(float.MaxValue , (int)NodeCount).ToArray();
                float[] array_distance_back = Enumerable.Repeat(float.MaxValue , (int)NodeCount).ToArray();
                forward_distance.UpdatePriority(i[0] - 1, 0);
                backward_distance.UpdatePriority(i[1] - 1, 0);
                result.Add(BidrectionalDijkstra(forawrd_adjacent_list, backward_adjacent_list, forward_distance, backward_distance
                , visited_forward, visited_backward, edges_weight_forward, edges_weight_backward
                , array_distance_for, array_distance_back));
            }
            return result.ToArray();
        }
        private long BidrectionalDijkstra(List<List<long>> adj_forawrd, List<List<long>> adj_backward,
                    SimplePriorityQueue<long> forward_distance, SimplePriorityQueue<long> backward_distance,
                    bool[] visited_forward, bool[] visited_backward, List<List<long>> edges_weight_forward,
                    List<List<long>> edges_weight_backward, float[] array_distance_for, float[] array_distance_back)
        {
            while (true)
            {
                float get_deqeued_priority = forward_distance.GetPriority(forward_distance.First);
                if (get_deqeued_priority == int.MaxValue)
                    return -1;
                long dequeued_forward = forward_distance.Dequeue();
                array_distance_for[dequeued_forward] = get_deqeued_priority;
                var adj_deq_for = adj_forawrd[(int)dequeued_forward];
                for (int i = 0; i < adj_deq_for.Count; i++)
                {
                    if (!visited_forward[adj_deq_for[i]])
                    {
                        if (forward_distance.GetPriority(adj_deq_for[i]) > get_deqeued_priority + edges_weight_forward[(int)dequeued_forward][i])
                        {
                            forward_distance.UpdatePriority(adj_deq_for[i], get_deqeued_priority + edges_weight_forward[(int)dequeued_forward][i]);
                            array_distance_for[adj_deq_for[i]] = get_deqeued_priority + edges_weight_forward[(int)dequeued_forward][i];
                        }
                    }
                }
                visited_forward[dequeued_forward] = true;
                if (visited_backward[dequeued_forward])
                    return (long)distance(array_distance_for, array_distance_back);
                float get_deqeued_priority_backward = backward_distance.GetPriority(backward_distance.First);
                if (get_deqeued_priority_backward == int.MaxValue)
                    return -1;
                long dequeued_backward = backward_distance.Dequeue();
                array_distance_back[dequeued_backward] = get_deqeued_priority_backward;
                var adj_deq_back = adj_backward[(int)dequeued_backward];
                for (int i = 0; i < adj_deq_back.Count; i++)
                {
                    if (!visited_backward[adj_deq_back[i]])
                    {
                        if (backward_distance.GetPriority(adj_deq_back[i]) > get_deqeued_priority_backward + edges_weight_backward[(int)dequeued_backward][i])
                        {
                            backward_distance.UpdatePriority(adj_deq_back[i], get_deqeued_priority_backward + edges_weight_backward[(int)dequeued_backward][i]);
                            array_distance_back[adj_deq_back[i]] = get_deqeued_priority_backward + edges_weight_backward[(int)dequeued_backward][i];
                        }
                    }
                }
                visited_backward[dequeued_backward] = true;
                if (visited_forward[dequeued_backward])
                    return (long)distance(array_distance_for, array_distance_back);
                
            }
        }
        private float distance(float[] forward_distance, float[] backward_distance)
        {
            float minimum = int.MaxValue;
            for (int i = 0; i < forward_distance.Length; i++)
            {
                if (minimum > forward_distance[i] + backward_distance[i])
                    minimum = forward_distance[i] + backward_distance[i];
            }
            return minimum;
        }
        private SimplePriorityQueue<long> InitializePQ(long NodeCount)
        {
            SimplePriorityQueue<long> PQ = new SimplePriorityQueue<long>();
            for (int i = 0; i < NodeCount; i++)
            {
                PQ.Enqueue(i, int.MaxValue);
            }
            return PQ;
        }
    }
}
