using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MazeSolver.Models;

namespace MazeSolver.Helpers
{
    public class AStarSolver : BaseSolver
    {
        private AStarSolver(string maze) : base(maze)
        {
            var lines = Regex.Split(maze, @"\r\n|\r|\n", RegexOptions.Compiled);
            Array = lines.Select(x => x.ToCharArray()).ToArray();
            Height = Array.Length;
            Width = Array.Max(x => x.Length);
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

        /// <summary>
        /// The A* algorithm, from the pseudocode on Wikipedia (https://en.wikipedia.org/wiki/A*_search_algorithm)
        /// </summary>
        /// <returns></returns>
        Path FindAStarPath()
        {
            var start = Beginning;
            var end = Goal;

            // The set of nodes already evaluated
            var closedSet = new HashSet<Node>();

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new HashSet<Node>();
            openSet.Add(start);

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<Node, Node>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<Node, int>();  // Default value should be infinity.
            // The cost of going from start to start is zero.
            gScore.Add(start, 0);

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Node, int>();  // Default value should be infinity.
            // For the first node, that value is completely heuristic.
            fScore.Add(start, EstimateCost(start, end));

            while (openSet.Any())
            {
                var curr = openSet.Join(fScore, x => x, x => x.Key, (x, y) => new { Key = x, Cost = y.Value }).OrderBy(x => x.Cost).First();
                var current = curr.Key;

                if (current.Equals(end))
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);

                    var neighborScore = gScore.ContainsKey(neighbor) ? gScore[neighbor] : int.MaxValue;
                    var tentativeScore = gScore.ContainsKey(current) ? gScore[current] : int.MaxValue;
                    tentativeScore += current.GetDistanceTo(neighbor);
                    if (tentativeScore >= neighborScore)
                        continue;

                    // This path is the best until now.
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeScore;
                    fScore[neighbor] = tentativeScore + neighbor.GetDistanceTo(end);
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
                    if (row < 0 || row >= Height || col < 0 || col >= Width)
                        continue;

                    switch (Array[row][col])
                    {
                        case Clear:
                        case End:
                            results.Add(new Node(row, col));
                            break;
                    }
                }
            }

            return results;
        }

        private Path ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            var nodes = new List<Node>();
            var node = current;

            if (!node.Equals(Beginning) && !node.Equals(Goal))
                nodes.Add(node);

            while (node != null && cameFrom.ContainsKey(node))
            {
                node = cameFrom[node];
                if (!node.Equals(Beginning) && !node.Equals(Goal))
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
    }
}