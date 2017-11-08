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
            var xVal = x.LastNode.GetDistanceTo(mGoal);
            var yVal = y.LastNode.GetDistanceTo(mGoal);

            if (xVal <= yVal)
                return -1;
            return 1;

            //return xVal.CompareTo(yVal);
        }
    }
}