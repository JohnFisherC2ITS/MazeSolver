using System;
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
        public void TestMaze1()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze1.txt"));
            var result = Solver.Solve(maze);

        }

        [TestMethod]
        public void TestMaze2()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze2.txt"));
            var result = Solver.Solve(maze);
        }

        [TestMethod]
        public void TestMaze3()
        {
            var maze = File.ReadAllText(Path.Combine(GetExecutingPath(), "maze3.txt"));
            var result = Solver.Solve(maze);

        }
    }
}
