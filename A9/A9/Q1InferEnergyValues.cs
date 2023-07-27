using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
            // ExcludeTestCases(1,2,3);
            // ExcludeTestCaseRangeInclusive(1, 13);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            for (int i = 0; i < MATRIX_SIZE; i++)
            {
                //search for max in cuurent col
                double current_max = Math.Abs(matrix[i, i]);
                int max_row = i;
                for (int k = i + 1; k < MATRIX_SIZE; k++)
                {
                    if (Math.Abs(matrix[k, i]) > current_max)
                    {
                        current_max = Math.Abs(matrix[k, i]);
                        max_row = k;
                    }
                }
                // swap maximum row with current row
                for (int k = i; k < MATRIX_SIZE + 1; k++)
                {
                    (matrix[i, k], matrix[max_row, k]) = (matrix[max_row, k], matrix[i, k]);
                }
                // make all below rows in current column zero 
                for (int k = i + 1; k < MATRIX_SIZE; k++)
                {
                    double c = -matrix[k, i] / matrix[i, i];
                    for (int j = i; j < MATRIX_SIZE + 1; j++)
                    {
                        if (i == j)
                            matrix[k, j] = 0;
                        else
                            matrix[k, j] += c * matrix[i, j];

                    }
                }
            }
            double[] result = new double[MATRIX_SIZE];
            // calculate for upper triangle matrix 
            for (int i = (int)MATRIX_SIZE - 1; i >= 0; i--)
            {
                result[i] = matrix[i, MATRIX_SIZE] / matrix[i, i];
                for (int j = i - 1; j >= 0; j--)
                {
                    matrix[j, MATRIX_SIZE] -= matrix[j, i] * result[i];
                }
                if (Math.Abs(result[i]) - Math.Abs((int)result[i]) < 0.25)
                {
                    if (result[i] > 0)
                        result[i] = Math.Floor(result[i]);
                    else
                        result[i] = (int)result[i];
                }

                else if (Math.Abs(result[i]) - Math.Abs((int)result[i]) > 0.75)
                {
                    if (result[i] > 0)
                        result[i] = Math.Ceiling(result[i]);
                    else
                        result[i] = (int)result[i] - 1;
                }

                else
                    result[i] = Math.Floor(result[i]) + 0.5;
            }
            return result;
        }
    }
}
