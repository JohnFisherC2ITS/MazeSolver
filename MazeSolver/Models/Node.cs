using System;

namespace MazeSolver.Models
{
    public class Node
    {
        public readonly int Row;
        public readonly int Col;

        private readonly int mHashCode;

        public Node(int row, int col)
        {
            Row = row;
            Col = col;
            mHashCode = GetKey().GetHashCode();
        }

        public static string GetKey(int row, int col)
        {
            return $"{row}_{col}";
        }

        public string GetKey()
        {
            return GetKey(Row, Col);
        }

        public int GetDistanceTo(Node goal)
        {
            return Math.Abs(Row - goal.Row) + Math.Abs(Col - goal.Col);
        }

        public override int GetHashCode()
        {
            return mHashCode;
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return obj != null && node.Row == Row && node.Col == Col;
        }
    }
}
