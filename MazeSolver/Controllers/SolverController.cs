using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MazeSolver.Helpers;

namespace MazeSolver.Controllers
{
    public class SolverController : ApiController
    {
        [Route("solveMaze")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult SolveMaze(string maze)
        {
            var result = Solver.Solve(maze);
            return Ok(new { steps = result.Steps, solution = result.Solution });
        }
    }
}
