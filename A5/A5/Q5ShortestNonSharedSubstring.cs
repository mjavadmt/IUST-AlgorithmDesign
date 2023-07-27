using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q5ShortestNonSharedSubstring : Processor
    {
        public Q5ShortestNonSharedSubstring(string testDataName) : base(testDataName)
        {
            ExcludeTestCaseRangeInclusive(26,50);
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, String>)Solve);

        private string Solve(string text1, string text2)
        {
            string WholeStr = text2 + "#" + text1 + "$";
            Node root = new Node();
            string PickedString = "";
            int MinLength = int.MaxValue;
            for (int i = 0; i < WholeStr.Length - 1; i++)
            {
                Node CopyRoot = root;
                for (int j = i; j < WholeStr.Length - 1; j++)
                {
                    if (!CopyRoot.Children.ContainsKey(WholeStr[j]))
                    {
                        Node NewNode = new Node();
                        CopyRoot.Children[WholeStr[j]] = NewNode;
                        CopyRoot = NewNode;
                        if (i > text1.Length && MinLength > j - i + 1)
                        {
                            PickedString = WholeStr.Substring(i, j - i + 1);
                            MinLength = PickedString.Length;
                            break;
                        }
                    }
                    else
                    {
                        CopyRoot = CopyRoot.Children[WholeStr[j]];
                    }
                    if (MinLength == 1)
                        break;
                }
            }
            return PickedString;
        }

    }
    class Node
    {
        public Dictionary<char, Node> Children = new Dictionary<char, Node>();
    }
}
