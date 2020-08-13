using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Modules;

namespace Sandbox.ASP {
    [Route("[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase {
        [HttpPost("Execute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult Execute(string key) {
            try {
                ModuleManager.Instance.TryExecute(key);
            } catch {
                return StatusCode(418);
            }

            return Ok("This is a test.");
        }
    }
}