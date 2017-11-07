using System.Collections.Generic;
using MazeSolver.Helpers;

namespace MazeSolver.Models
{
    class Path
    {
        public Step LastStep { get; set; }
        public PathStatus Status { get; set; } = PathStatus.Incomplete;
        public HashSet<string> UsedSteps { get; set; }

        public Path AddStep(int row, int col)
        {
            var clone = new Path
            {
                LastStep = new Step {Row = row, Col = col},
                Status = Status,
                // Clone the used steps and add the current.
                UsedSteps = new HashSet<string>(UsedSteps ?? new HashSet<string>()) {GetKey(row, col)}
            };
            return clone;
        }

        public int Steps => UsedSteps?.Count ?? 0;

        public bool ContainsStep(int row, int col)
        {
            return UsedSteps?.Contains(GetKey(row, col)) ?? false;
        }

        string GetKey(int row, int col)
        {
            return $"{row}_{col}";
        }
    }
}