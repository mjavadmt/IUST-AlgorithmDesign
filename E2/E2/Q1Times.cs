using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace E2
{
    public class Q1Times : Processor
    {
        public Q1Times(string testDataName) : base(testDataName)
        {
            // ExcludeTestCaseRangeInclusive(1, 18);
            ExcludeTestCases(41);
        }

        public override string Process(string inStr)
            => E2Processors.Q1Processor(inStr, Solve);

        public string[] Solve(char[][] board, string[] words)
        {
            List<string> PatternsFound = new List<string>();
            foreach (var pattern in words)
            {
                bool[][] visited = new bool[board.Length][];
                for (int i = 0; i < visited.Length; i++)
                {
                    visited[i] = new bool[board[i].Length];
                }
                int s = 0;
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = 0; j < board[i].Length; j++)
                    {
                        if (DFS(board, i, j, pattern, 0, visited))
                        {
                            PatternsFound.Add(pattern);
                            s = 1;
                            break;
                        }
                        for (int k = 0; k < visited.Length; k++)
                        {
                            visited[k] = new bool[board[k].Length];
                        }
                    }
                    if (s == 1)
                        break;
                }
            }
            return PatternsFound.OrderBy(x => x).ToArray();
        }
        private bool DFS(char[][] board, int i, int j, string pattern, int MovedSoFar, bool[][] visited)
        {
            visited[i][j] = true;
            if (board[i][j] != pattern[MovedSoFar])
                return false;
            if (MovedSoFar == pattern.Length - 1)
                return true;
            if (i - 1 >= 0)
            {
                if (!visited[i - 1][j])
                {
                    if (DFS(board, i - 1, j, pattern, MovedSoFar + 1, visited))
                        return true;
                    else
                        visited[i - 1][j] = false;
                }
            }

            if (i + 1 < board.Length)
            {
                if (!visited[i + 1][j])
                {
                    if (DFS(board, i + 1, j, pattern, MovedSoFar + 1, visited))
                        return true;
                    else
                        visited[i + 1][j] = false;
                }
            }

            if (j - 1 >= 0)
            {
                if (!visited[i][j - 1])
                {
                    if (DFS(board, i, j - 1, pattern, MovedSoFar + 1, visited))
                        return true;
                    else
                        visited[i][j - 1] = false;
                }
            }

            if (j + 1 < board[0].Length)
            {
                if (!visited[i][j + 1])
                {
                    if (DFS(board, i, j + 1, pattern, MovedSoFar + 1, visited))
                        return true;
                    else
                        visited[i][j + 1] = false;
                }
            }

            return false;


        }
    }
}
