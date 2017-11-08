using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
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

        protected int mHeight;
        protected int mWidth;
        protected char[][] mArray;
        protected Node mBeginning;
        protected Node mGoal;

        protected BaseSolver(string maze)
        {
            var lines = Regex.Split(maze, @"\r\n|\r|\n", RegexOptions.Compiled);
            mArray = lines.Select(x => x.ToCharArray()).ToArray();
            mHeight = mArray.Length;
            mWidth = mArray.Max(x => x.Length);
            mBeginning = Find(Start);
            mGoal = Find(End);
        }

        protected Node Find(char uniqueCharacter)
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

        protected string GenerateSolution(Path path)
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