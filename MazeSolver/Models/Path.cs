using System.Collections.Generic;
using MazeSolver.Helpers;

namespace MazeSolver.Models
{
    public class Path
    {
        public Node LastNode { get; set; }
        public PathStatus Status { get; set; } = PathStatus.Incomplete;
        public HashSet<string> UsedSteps { get; set; }

        public Path AddStep(int row, int col)
        {
            var clone = new Path
            {
                LastNode = new Node {Row = row, Col = col},
                Status = Status,
                // Clone the used steps and add the current.
                UsedSteps = new HashSet<string>(UsedSteps ?? new HashSet<string>())
                {
                    Node.GetKey(row, col)
                }
            };
            return clone;
        }

        public int Steps => UsedSteps?.Count ?? 0;

        public bool ContainsStep(int row, int col)
        {
            return UsedSteps?.Contains(Node.GetKey(row, col)) ?? false;
        }

        public override int GetHashCode()
        {
            return (LastNode?.GetKey() ?? "empty").GetHashCode();
        }
    }
}