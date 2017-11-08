using System;

namespace MazeSolver.Models
{
    public class Node
    {
        public int Row;
        public int Col;

        public static string GetKey(int row, int col)
        {
            return $"{row}_{col}";
        }

        public string GetKey()
        {
            return GetKey(Row, Col);
        }

        public static Node FromKey(string key)
        {
            var split = key.Split('_');
            var row = int.Parse(split[0]);
            var col = int.Parse(split[1]);
            return new Node {Row = row, Col = col};
        }

        public int GetDistanceTo(Node goal)
        {
            return Math.Abs(Row - goal.Row) + Math.Abs(Col - goal.Col);
        }

        public override int GetHashCode()
        {
            return GetKey().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return obj != null && node.Row == Row && node.Col == Col;
        }
    }
}
