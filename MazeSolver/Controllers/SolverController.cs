using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using MazeSolver.Helpers;

namespace MazeSolver.Controllers
{
    public class SolverController : ApiController
    {
        [Route("solveMaze")]
        [HttpPost]
        public object SolveMaze([FromBody]string maze)
        {
            var result = AStarSolver.Solve(maze);
            return Ok(new { steps = result.Steps, solution = result.Solution });
        }
    }
}
