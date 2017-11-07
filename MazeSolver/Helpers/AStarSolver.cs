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
    public class AStarSolver
    {
        private const char Start = 'A';
        private const char Clear = '.';
        private const char Blocked = '#';
        private const char Used = '@';
        private const char End = 'B';

        private int mHeight;
        private int mWidth;
        private char[][] mArray;

        public static SolverResult Solve(string maze)
        {
            var sw = new Stopwatch();
            sw.Start();

            var solver = new AStarSolver(maze);
            var paths = solver.FindCompletePaths();

            sw.Stop();

            Console.WriteLine("Solutions: " + paths.Count + " found in " + sw.ElapsedMilliseconds + " milliseconds.");
            Console.WriteLine();

            foreach (var p in paths)
            {
                Console.WriteLine("Steps: " + (p.Steps + 1));
                Console.WriteLine(solver.GenerateSolution(p));
                Console.WriteLine();
            }
            
            var path = paths.OrderBy(x => x.Steps).FirstOrDefault();
            if (path == null)
                return null;

            return new SolverResult
            {
                Steps = path.Steps + 1, // The path steps don't record taking the last step onto the finish location, so add 1.
                Solution = solver.GenerateSolution(path)
            };
        }

        private AStarSolver(string maze)
        {
            var lines = Regex.Split(maze, @"\r\n|\r|\n", RegexOptions.Compiled);
            mArray = lines.Select(x => x.ToCharArray()).ToArray();
            mHeight = mArray.Length;
            mWidth = mArray.Max(x => x.Length);
        }

        private Node Find(char uniqueCharacter)
        {
            var row = -1;
            var col = -1; // Set col to -1 until Start is found.

            // Find A, the loops will stop when row and col are set.
            for (var r = 0; r < mHeight && col < 0; ++r)
            {
                for (var c = 0; c < mWidth && row < 0; ++c)
                {
                    var spot = mArray[r][c];
                    if (spot == uniqueCharacter)
                    {
                        row = r;
                        col = c;
                    }
                }
            }

            return new Node { Row = row, Col = col };
        }

        private List<Path> FindCompletePaths()
        {
            var paths = ExtendPath(null);
            var completePaths = new List<Path>();
            while (paths.Any())
            {
                var path = paths.Pop();
                switch (path.Status)
                {
                    case PathStatus.Finished:
                        // Save this path
                        completePaths.Add(path);
                        break;

                    // A status of Blocked is not needed, since we remove the path from the stack and add no paths if there's nowhere to go.

                    case PathStatus.Incomplete:
                        foreach (var p in ExtendPath(path))
                        {
                            paths.Push(p);
                        }
                        break;
                }
            }

            return completePaths;
        }

        private Stack<Path> ExtendPath(Path path)
        {
            var result = new Stack<Path>();

            // Find the last step, or create the first step and path if path is null.
            var step = path?.LastNode ?? Find(Start);
            path = path ?? new Path();

            for (var x = -1; x <= 1; ++x)
            {
                for (var y = -1; y <= 1; ++y)
                {
                    // Don't use diagonals or self
                    if ((x != 0 && y != 0) || (x == 0 && y == 0))
                        continue;

                    var row = step.Row + y;
                    var col = step.Col + x;

                    // Don't go out of bounds
                    if (row < 0 || row >= mHeight || col < 0 || col >= mWidth)
                        continue;

                    switch (mArray[row][col])
                    {
                        case Clear:
                            // Don't go where this path has already been
                            if (!path.ContainsStep(row, col))
                            {
                                // We found a valid next step.
                                result.Push(path.AddStep(row, col));
                            }
                            break;
                        case End:
                            path.Status = PathStatus.Finished;
                            result.Push(path); // No added steps, but make sure that the fact that this is finished gets returned!
                            break;
                    }
                }
            }

            return result;
        }

        string GenerateSolution(Path path)
        {
            var result = new StringBuilder();
            for (var r = 0; r < mHeight; ++r)
            {
                for (var c = 0; c < mWidth; ++c)
                {
                    var ch = path.ContainsStep(r, c) ? Used : mArray[r][c];
                    result.Append(ch);
                }
                result.AppendLine();
            }
            
            return result.ToString();
        }


        Path FindAStarPath()
        {
            var start = Find(Start);
            var end = Find(End);

            // The set of nodes already evaluated
            var closedSet = new HashSet<Node>();

            // The set of currently discovered nodes that are not evaluated yet.
            // Initially, only the start node is known.
            var openSet = new HashSet<Node>();

            // For each node, which node it can most efficiently be reached from.
            // If a node can be reached from many nodes, cameFrom will eventually contain the
            // most efficient previous step.
            var cameFrom = new Dictionary<Node, Node>();

            // For each node, the cost of getting from the start node to that node.
            var gScore = new Dictionary<Node, decimal>();  // Default value should be infinity.
            // The cost of going from start to start is zero.
            gScore.Add(start, 0);

            // For each node, the total cost of getting from the start node to the goal
            // by passing by that node. That value is partly known, partly heuristic.
            var fScore = new Dictionary<Node, decimal>();  // Default value should be infinity.
            // For the first node, that value is completely heuristic.
            fScore.Add(start, EstimateCost(start, end));


            while (openSet.Any())
            {
                var current = openSet.Join(fScore, x => x, x => x.Key, (x, y) => new { Node = x, Cost = y }).OrderBy(x => x.Cost).First();

            }


            return null;
        }

        decimal EstimateCost(Node start, Node goal)
        {
            return 1;
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