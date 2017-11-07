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
    public class Solver
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

            var solver = new Solver(maze);
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

        private Solver(string maze)
        {
            var lines = Regex.Split(maze, @"\r\n|\r|\n", RegexOptions.Compiled);
            mArray = lines.Select(x => x.ToCharArray()).ToArray();
            mHeight = mArray.Length;
            mWidth = mArray.Max(x => x.Length);
        }

        private Step FindStart()
        {
            var row = -1;
            var col = -1; // Set col to -1 until Start is found.

            // Find A, the loops will stop when row and col are set.
            for (var r = 0; r < mHeight && col < 0; ++r)
            {
                for (var c = 0; c < mWidth && row < 0; ++c)
                {
                    var spot = mArray[r][c];
                    if (spot == Start)
                    {
                        row = r;
                        col = c;
                    }
                }
            }

            return new Step { Row = row, Col = col };
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
            var step = path?.LastStep ?? FindStart();
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
    }
}