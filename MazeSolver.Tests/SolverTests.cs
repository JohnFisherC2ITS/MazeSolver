using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MazeSolver.Controllers;
using MazeSolver.Helpers;

namespace MazeSolver.Tests
{
    [TestClass]
    public class SolverTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new SolverController();

            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze1.txt"));
            var result = controller.SolveMaze(maze);
        }

        private string GetExecutingPath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var executingFile = Regex.Replace(codeBase, @"^[a-z]+\:+(\/|\\)+", "");
            var executingPath = Path.GetDirectoryName(executingFile);
            return executingPath;
        }

        [TestMethod]
        public void TestMaze1WithBruteForce()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze1.txt"));
            var result = BruteForceSolver.Solve(maze);

        }

        [TestMethod]
        public void TestMaze2WithBruteForce()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze2.txt"));
            var result = BruteForceSolver.Solve(maze);
        }

        [TestMethod]
        public void TestMaze1()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze1.txt"));

            var sw = new Stopwatch();
            sw.Start();
            var result = AStarSolver.Solve(maze);
            sw.Stop();

            Console.WriteLine("Solutions: " + 1 + " found in " + sw.ElapsedMilliseconds + " milliseconds.");
            Console.WriteLine();
            Console.WriteLine("Steps: " + result.Steps);
            Console.WriteLine(result.Solution);
        }

        [TestMethod]
        public void TestMaze2()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze2.txt"));

            var sw = new Stopwatch();
            sw.Start();
            var result = AStarSolver.Solve(maze);
            sw.Stop();

            Console.WriteLine("Solutions: " + 1 + " found in " + sw.ElapsedMilliseconds + " milliseconds.");
            Console.WriteLine();
            Console.WriteLine("Steps: " + result.Steps);
            Console.WriteLine(result.Solution);
        }

        [TestMethod]
        public void TestMaze3()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze3.txt"));

            var sw = new Stopwatch();
            sw.Start();
            var result = AStarSolver.Solve(maze);
            sw.Stop();

            Console.WriteLine("Solutions: " + 1 + " found in " + sw.ElapsedMilliseconds + " milliseconds.");
            Console.WriteLine();
            Console.WriteLine("Steps: " + result.Steps);
            Console.WriteLine(result.Solution);
        }
    }
}
