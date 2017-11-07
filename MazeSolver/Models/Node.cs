using System;

namespace MazeSolver.Models
{
    class Node
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

        public decimal GetDistanceTo(Node goal)
        {
            return (decimal)Math.Sqrt(Math.Pow(goal.Row - Row, 2) + Math.Pow(goal.Col - Col, 2));
        }
    }
}