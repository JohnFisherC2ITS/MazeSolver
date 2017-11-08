## How to run the project

You may simply debug the MazeSolver project.
The `index.html` page provides a way to test the three test mazes provided 
in the challenge.

If you wish, you may execute the unit tests from `MazeSolver.Tests` in the `SolverTests.cs` file.
* All of the above test methods will provide readable output through use of `Console.WriteLine()`.  Resharper's unit test features handle it well, though the maze 3 is large enough that you will likely need to click the "Show stack trace in a new window" button.
* The test methods containing "WithBruteForce" in their name will provide multiple solutions, 
sorting to place the solution with the fewest steps at the top of the list. 
(Maze 3 will run out of memory and take far too long in the brute force solver, so there is no brute force test for it.)
* The other test methods will use the A* algorithm to provide one solution.

Enjoy!