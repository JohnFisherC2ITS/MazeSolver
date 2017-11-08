using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MazeSolver.Models;

namespace MazeSolver.Helpers
{
    public class AStarSolver : BaseSolver
    {
        private AStarSolver(string maze) : base(maze)
        {
            var lines = Regex.Split(maze, @"\r\n|\r|\n", RegexOptions.Compiled);
            mArray = lines.Select(x => x.ToCharArray()).ToArray();
            mHeight = mArray.Length;
            mWidth = mArray.Max(x => x.Length);
        }

        public static SolverResult Solve(string maze)
        {
            var solver = new AStarSolver(maze);
            var path = solver.FindAStarPath();
            if (path == null)
                return null;

            return new SolverResult
            {
                Steps = path.Steps + 1, // The path steps don't record taking the last step onto the finish location, so add 1.
                Solution = solver.GenerateSolution(path)
            };
        }

        Path FindAStarPath()
        {
            var start = mBeginning;
            var end = mGoal;
            var endKey = end.GetKey();

            // The set of nodes already evaluated
            var closedSet = new HashSet<string>();

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new HashSet<string>();
            openSet.Add(start.GetKey());

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<string, Node>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<string, int>();  // Default value should be infinity.
            // The cost of going from start to start is zero.
            gScore.Add(start.GetKey(), 0);

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<string, int>();  // Default value should be infinity.
            // For the first node, that value is completely heuristic.
            fScore.Add(start.GetKey(), EstimateCost(start, end));

            while (openSet.Any())
            {
                var curr = openSet.Join(fScore, x => x, x => x.Key, (x, y) => new { Key = x, Cost = y.Value }).OrderBy(x => x.Cost).First();
                var currentKey = curr.Key;
                var currentNode = Node.FromKey(currentKey);

                if (currentKey.Equals(endKey))
                {
                    return ReconstructPath(cameFrom, currentNode);
                }

                openSet.Remove(currentKey);
                closedSet.Add(currentKey);

                foreach (var neighborNode in GetNeighbors(currentNode))
                {
                    var neighborKey = neighborNode.GetKey();
                    if (closedSet.Contains(neighborKey))
                        continue;

                    if (!openSet.Contains(neighborKey))
                        openSet.Add(neighborNode.GetKey());

                    var neighborScore = gScore.ContainsKey(neighborKey) ? gScore[neighborKey] : int.MaxValue;
                    var tentativeScore = gScore.ContainsKey(currentKey) ? gScore[currentKey] : int.MaxValue;
                    tentativeScore += currentNode.GetDistanceTo(neighborNode);
                    if (tentativeScore >= neighborScore)
                        continue;

                    // This path is the best until now.
                    cameFrom[neighborKey] = currentNode;
                    gScore[neighborKey] = tentativeScore;
                    fScore[neighborKey] = gScore[neighborKey] + neighborNode.GetDistanceTo(end);
                }
            }

            return null;
        }

        private List<Node> GetNeighbors(Node current)
        {
            var results = new List<Node>();

            for (var x = -1; x <= 1; ++x)
            {
                for (var y = -1; y <= 1; ++y)
                {
                    // Don't use diagonals or self
                    if ((x != 0 && y != 0) || (x == 0 && y == 0))
                        continue;

                    var row = current.Row + y;
                    var col = current.Col + x;

                    // Don't go out of bounds
                    if (row < 0 || row >= mHeight || col < 0 || col >= mWidth)
                        continue;

                    switch (mArray[row][col])
                    {
                        case Clear:
                        case End:
                            results.Add(new Node { Row = row, Col = col });
                            break;
                    }
                }
            }

            return results;
        }

        private Path ReconstructPath(Dictionary<string, Node> cameFrom, Node current)
        {
            var nodes = new List<Node>();
            var node = current;

            if (!node.Equals(mBeginning) && !node.Equals(mGoal))
                nodes.Add(node);

            while (node != null && cameFrom.ContainsKey(node.GetKey()))
            {
                node = cameFrom[node.GetKey()];
                if (!node.Equals(mBeginning) && !node.Equals(mGoal))
                    nodes.Add(node);
            }

            return new Path
            {
                LastNode = current,
                Status = PathStatus.Finished,
                UsedSteps = new HashSet<string>(nodes.Select(x => x.GetKey()))
            };
        }

        int EstimateCost(Node start, Node goal)
        {
            return start.GetDistanceTo(goal);
        }

        //function A*(start, goal)
            //    // The set of nodes already evaluated
            //    closedSet := {}
            //
            //    // The set of currently discovered nodes that are not evaluated yet.
            //    // Initially, only the start node is known.
            //    openSet := {start}
            //
            //    // For each node, which node it can most efficiently be reached from.
            //    // If a node can be reached from many nodes, cameFrom will eventually contain the
            //    // most efficient previous step.
            //    cameFrom := an empty map
            //
            //    // For each node, the cost of getting from the start node to that node.
            //    gScore := map with default value of Infinity
            //
            //    // The cost of going from start to start is zero.
            //    gScore[start] := 0
            //
            //    // For each node, the total cost of getting from the start node to the goal
            //    // by passing by that node. That value is partly known, partly heuristic.
            //    fScore := map with default value of Infinity
            //
            //    // For the first node, that value is completely heuristic.
            //    fScore[start] := heuristic_cost_estimate(start, goal)
            //
            //    while openSet is not empty
            //        current := the node in openSet having the lowest fScore[] value
            //        if current = goal
            //            return reconstruct_path(cameFrom, current)
            //
            //        openSet.Remove(current)
            //        closedSet.Add(current)
            //
            //        for each neighbor of current
            //            if neighbor in closedSet
            //                continue		// Ignore the neighbor which is already evaluated.
            //
            //            if neighbor not in openSet	// Discover a new node
            //                openSet.Add(neighbor)
            //            
            //            // The distance from start to a neighbor
            //            tentative_gScore := gScore[current] + dist_between(current, neighbor)
            //            if tentative_gScore >= gScore[neighbor]
            //                continue		// This is not a better path.
            //
            //            // This path is the best until now. Record it!
            //            cameFrom[neighbor] := current
            //            gScore[neighbor] := tentative_gScore
            //            fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal) 
            //
            //    return failure
            //
            //function reconstruct_path(cameFrom, current)
            //    total_path := [current]
            //    while current in cameFrom.Keys:
            //        current := cameFrom[current]
            //        total_path.append(current)
            //    return total_path
        }
}