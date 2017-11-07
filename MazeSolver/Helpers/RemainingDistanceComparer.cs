using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using MazeSolver.Models;

namespace MazeSolver.Helpers
{
    class RemainingDistanceComparer : IComparer<Path>
    {
        private Node mGoal;

        public RemainingDistanceComparer(Node goal)
        {
            mGoal = goal;
        }

        public int Compare(Path x, Path y)
        {
            var xNode = x.LastNode;
            var yNode = y.LastNode;
            return Math.Abs(xNode.Row - mGoal.Row) + Math.Abs(xNode.Col - mGoal.Col);
        }
    }
}