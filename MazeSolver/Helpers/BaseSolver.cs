using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MazeSolver.Models;

namespace MazeSolver.Helpers
{
    public abstract class BaseSolver
    {
        protected const char Start = 'A';
        protected const char Clear = '.';
        protected const char Blocked = '#';
        protected const char Used = '@';
        protected const char End = 'B';

        protected int Height;
        protected int Width;
        protected char[][] Array;
        protected Node Beginning;
        protected Node Goal;

        protected BaseSolver(string maze)
        {
            var lines = Regex.Split(maze, @"\r\n|\r|\n", RegexOptions.Compiled);
            Array = lines.Select(x => x.ToCharArray()).ToArray();
            Height = Array.Length;
            Width = Array.Max(x => x.Length);
            Beginning = Find(Start);
            Goal = Find(End);
        }

        protected Node Find(char uniqueCharacter)
        {
            var row = -1;
            var col = -1; // Set col to -1 until Start is found.

            // Find A, the loops will stop when row and col are set.
            for (var r = 0; r < Height && col < 0; ++r)
            {
                for (var c = 0; c < Width && row < 0; ++c)
                {
                    var spot = Array[r][c];
                    if (spot == uniqueCharacter)
                    {
                        row = r;
                        col = c;
                    }
                }
            }

            return new Node(row, col);
        }

        protected string GenerateSolution(Path path)
        {
            var result = new StringBuilder();
            for (var r = 0; r < Height; ++r)
            {
                for (var c = 0; c < Width; ++c)
                {
                    var ch = path.ContainsStep(r, c) ? Used : Array[r][c];
                    result.Append(ch);
                }
                result.AppendLine();
            }

            return result.ToString();
        }
    }
}