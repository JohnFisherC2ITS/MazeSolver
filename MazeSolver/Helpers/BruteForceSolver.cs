﻿using System.Collections.Generic;
using System.Linq;
using MazeSolver.Models;

namespace MazeSolver.Helpers
{
    /// <summary>
    /// This works well for small mazes, but runs out of time and memory with large ones.
    /// </summary>
    public class BruteForceSolver : BaseSolver
    {
        public static List<SolverResult> SolveAll(string maze)
        {
            var solver = new BruteForceSolver(maze);
            var paths = solver.FindCompletePaths();
            return paths.OrderBy(x => x.Steps).Select(path =>
                new SolverResult
                {
                    Steps = path.Steps + 1, // The path steps don't record taking the last step onto the finish location, so add 1.
                    Solution = solver.GenerateSolution(path)
                }).ToList();
        }

        private BruteForceSolver(string maze) : base(maze)
        {
        }

        private List<Path> FindCompletePaths()
        {
            var paths = ExtendPath(null);
            var completePaths = new List<Path>();
            while (paths.Any())
            {
                var path = paths.First(); // get the one that's currently nearest to the goal.
                paths.RemoveAt(0); // Remove it.
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
                            paths.Add(p);
                        }
                        break;
                }
            }

            return completePaths;
        }

        private List<Path> ExtendPath(Path path)
        {
            var result = new List<Path>();

            // Find the last step, or create the first step and path if path is null.
            var step = path?.LastNode ?? Beginning;
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
                    if (row < 0 || row >= Height || col < 0 || col >= Width)
                        continue;

                    switch (Array[row][col])
                    {
                        case Clear:
                            // Don't go where this path has already been
                            if (!path.ContainsStep(row, col))
                            {
                                // We found a valid next step.
                                result.Add(path.AddStep(row, col));
                            }
                            break;
                        case End:
                            path.Status = PathStatus.Finished;
                            result.Add(path); // No added steps, but make sure that the fact that this is finished gets returned!
                            break;
                    }
                }
            }

            return result;
        }
    }
}