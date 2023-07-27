using System;
using TestCommon;
using System.Collections.Generic;
using System.Linq;

namespace E2
{
    public class Q0Chart : Processor
    {
        public Q0Chart(string testDataName) : base(testDataName)
        {
            // ExcludeTestCases(1 , 2, 3 , 4);
            // ExcludeTestCaseRangeInclusive(16,50);
        }

        public override string Process(string inStr) =>
            E2Processors.Q0Processor(inStr, Solve);

        public string[] Solve(int professorsCount,
                              int classCount,
                              int timeCount,
                              long[,,] professorsCanTeach,
                              long[,,] classCanBeOccupied)
        {
            HashSet<string> clauses = new HashSet<string>();
            RemoveCantTeachOccupy(professorsCanTeach, classCanBeOccupied, clauses, professorsCount, classCount, timeCount);
            for (int i = 0; i < professorsCount; i++)
            {
                for (int k = 0; k < timeCount; k++)
                {
                    AtMostOneclassFixedTeacherTime(i, k, classCount, clauses, professorsCount, timeCount);
                }
            }
            for (int j = 0; j < classCount; j++)
            {
                for (int k = 0; k < timeCount; k++)
                {
                    AtMostOneTeacherFixedClassTime(j, k, classCount, clauses, professorsCount, timeCount);
                }
            }
            EachTeacherAtLeastOneTime(professorsCount, classCount, timeCount, clauses);
            EachClassAtLeastOneTime(professorsCount, classCount, timeCount, clauses);
            var new_clauses = clauses.ToList();
            new_clauses.Insert(0, $"{new_clauses.Count} {professorsCount * classCount * timeCount}");

            return new_clauses.ToArray();
        }

        private void EachClassAtLeastOneTime(int professorsCount, int classCount, int timeCount, HashSet<string> clauses)
        {
            for (int j = 0; j < classCount; j++)
            {
                string MakeAtLeasOneClass = "";
                for (int i = 0; i < professorsCount; i++)
                {
                    for (int k = 0; k < timeCount; k++)
                    {
                        MakeAtLeasOneClass += $"{UniqueIdentfier(i, j, k, professorsCount, classCount, timeCount)} ";
                    }
                }
                MakeAtLeasOneClass += "0";
                clauses.Add(MakeAtLeasOneClass);
            }
        }

        private void EachTeacherAtLeastOneTime(int professorsCount, int classCount, int timeCount, HashSet<string> clauses)
        {
            for (int i = 0; i < professorsCount; i++)
            {
                string MakeAtLeasOneTeacher = "";
                for (int j = 0; j < classCount; j++)
                {
                    for (int k = 0; k < timeCount; k++)
                    {
                        MakeAtLeasOneTeacher += $"{UniqueIdentfier(i, j, k, professorsCount, classCount, timeCount)} ";
                    }
                }
                MakeAtLeasOneTeacher += "0";
                clauses.Add(MakeAtLeasOneTeacher);
            }
        }

        private int UniqueIdentfier(int i, int j, int k, int professorsCount, int classCount, int timeCount)
        {
            return i * timeCount * classCount + j * timeCount + k + 1;
        }
        private void RemoveCantTeachOccupy(long[,,] professorsCanTeach, long[,,] classCanBeOccupied, HashSet<string> clauses, int professorsCount,
                                            int classCount, int timeCount)
        {
            for (int i = 0; i < professorsCount; i++)
            {
                for (int j = 0; j < classCount; j++)
                {
                    for (int k = 0; k < timeCount; k++)
                    {
                        if (professorsCanTeach[i, j, k] == -1)
                            clauses.Add($"-{UniqueIdentfier(i, j, k, professorsCount, classCount, timeCount)} 0");
                        if (classCanBeOccupied[i, j, k] == -1)
                            clauses.Add($"-{UniqueIdentfier(i, j, k, professorsCount, classCount, timeCount)} 0");
                    }
                }
            }
        }
        private void AtMostOneclassFixedTeacherTime(int i, int k, int classCount, HashSet<string> clauses, int professorsCount, int timeCount)
        {
            for (int f_j = 0; f_j < classCount; f_j++)
            {
                for (int s_j = f_j + 1; s_j < classCount; s_j++)
                {
                    // fpct => first professor class time
                    long fpct = UniqueIdentfier(i, f_j, k, professorsCount, classCount, timeCount);
                    // spct => second professor class time
                    long spct = UniqueIdentfier(i, s_j, k, professorsCount, classCount, timeCount);
                    clauses.Add($"-{fpct} -{spct} 0");
                }
            }
        }
        private void AtMostOneTeacherFixedClassTime(int j, int k, int classCount, HashSet<string> clauses, int professorsCount, int timeCount)
        {
            for (int f_i = 0; f_i < professorsCount; f_i++)
            {
                for (int s_i = f_i + 1; s_i < professorsCount; s_i++)
                {
                    // fpct => first professor class time
                    long fpct = UniqueIdentfier(f_i, j, k, professorsCount, classCount, timeCount);
                    // spct => second professor class time
                    long spct = UniqueIdentfier(s_i, j, k, professorsCount, classCount, timeCount);
                    clauses.Add($"-{fpct} -{spct} 0");
                }
            }
        }
        // private void RemoveCantOccupy(long[,,] classCanBeOccupied, HashSet<string> clauses, int professorsCount,
        //                                     int classCount, int timeCount)
        // {
        //     for (int i = 0; i < professorsCount; i++)
        //     {
        //         for (int j = 0; j < classCount; j++)
        //         {
        //             for (int k = 0; k < timeCount; k++)
        //             {
        //                 if (classCanBeOccupied[i, j, k] == -1)
        //                     clauses.Add($"-{UniqueIdentfier(i, j, k, professorsCount, classCount)} 0");
        //             }
        //         }
        //     }
        // }


        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

    }

}