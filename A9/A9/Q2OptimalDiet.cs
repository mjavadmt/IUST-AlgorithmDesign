using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;

namespace A9
{
    public class Q2OptimalDiet : Processor
    {
        public Q2OptimalDiet(string testDataName) : base(testDataName)
        {
            // ExcludeTestCases(1,2,3);
            // ExcludeTestCaseRangeInclusive(1, 21);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int N, int M, double[,] matrix1)
        {
            List<double[]> mat = new List<double[]>();
            for (int i = 0; i < N; i++)
            {
                double[] temp = new double[M + 1];
                for (int j = 0; j < M + 1; j++)
                {
                    temp[j] = matrix1[i, j];
                }
                mat.Add(temp);
            }
            for (int i = 0; i < M; i++)
            {
                double[] temp = new double[M + 1];
                temp[i] = -1;
                mat.Add(temp);
            }
            var SolveSubSetM = SubSetsOf(mat).Where(x => x.Count == M).ToList();
            double MaxEnergy = int.MinValue;
            double[] ans = new double[M];
            int s = 0;
            foreach (var subset in SolveSubSetM)
            {
                if (s == 5)
                {
                    System.Console.WriteLine();
                }
                double temp = 0;
                List<double[]> copied = new List<double[]>();
                subset.ForEach(x => copied.Add((double[])x.Clone()));
                double[] fake_result = new double[M];
                double[] correct_result = new double[M];
                try
                {
                    (fake_result , correct_result) =  LinearEquation(M, copied);
                }
                catch
                {
                    continue;
                }

                for (int i = 0; i < M; i++)
                {
                    temp += matrix1[N, i] * correct_result[i];
                }
                if (temp > MaxEnergy && ConditionHolds(mat , correct_result))
                {
                    MaxEnergy = temp;
                    ans = fake_result;
                }
                s++;
            }
            if (MaxEnergy == int.MinValue)
                return "No Solution";
            return "Bounded Solution" + '\n' + string.Join(" ", ans);
        }
        bool ConditionHolds(List<double[]> mat, double[] result)
        {
            foreach (var cond in mat)
            {
                double temp = 0 ; 
                for (int i = 0; i < cond.Length - 1; i++)
                {
                    temp += cond[i]*result[i];
                }
                if (temp > cond[cond.Length - 1])
                    return false;
            }
            return true;
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
        public (double[] , double[]) LinearEquation(long MATRIX_SIZE, List<double[]> matrix)
        {
            for (int i = 0; i < MATRIX_SIZE; i++)
            {
                //search for max in cuurent col
                double current_max = Math.Abs(matrix[i][i]);
                int max_row = i;
                for (int k = i + 1; k < MATRIX_SIZE; k++)
                {
                    if (Math.Abs(matrix[k][i]) > current_max)
                    {
                        current_max = Math.Abs(matrix[k][i]);
                        max_row = k;
                    }
                }
                // swap maximum row with current row
                for (int k = i; k < MATRIX_SIZE + 1; k++)
                {
                    (matrix[i][k], matrix[max_row][k]) = (matrix[max_row][k], matrix[i][k]);
                }
                // make all below rows in current column zero 
                for (int k = i + 1; k < MATRIX_SIZE; k++)
                {
                    double c = -matrix[k][i] / matrix[i][i];
                    for (int j = i; j < MATRIX_SIZE + 1; j++)
                    {
                        if (i == j)
                            matrix[k][j] = 0;
                        else
                            matrix[k][j] += c * matrix[i][j];

                    }
                }
            }
            double[] fake_result = new double[MATRIX_SIZE];
            double[] correct_result = new double[MATRIX_SIZE];
            // calculate for upper triangle matrix 
            for (int i = (int)MATRIX_SIZE - 1; i >= 0; i--)
            {
                fake_result[i] = matrix[i][MATRIX_SIZE] / matrix[i][i];
                correct_result[i] = fake_result[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    matrix[j][MATRIX_SIZE] -= matrix[j][i] * fake_result[i];
                }
                if (Math.Abs(fake_result[i]) - Math.Abs((int)fake_result[i]) < 0.25)
                {
                    if (fake_result[i] > 0)
                        fake_result[i] = Math.Floor(fake_result[i]);
                    else
                        fake_result[i] = (int)fake_result[i];
                }

                else if (Math.Abs(fake_result[i]) - Math.Abs((int)fake_result[i]) > 0.75)
                {
                    if (fake_result[i] > 0)
                        fake_result[i] = Math.Ceiling(fake_result[i]);
                    else
                        fake_result[i] = (int)fake_result[i] - 1;
                }

                else
                    fake_result[i] = Math.Floor(fake_result[i]) + 0.5;
            }
            return (fake_result , correct_result);
        }
    }
}
