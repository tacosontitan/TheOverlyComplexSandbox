using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Core;

namespace Sandbox.ASP {
    [Route("[controller]")]
    [ApiController]
    public class QueueController : ControllerBase {
        [HttpPost("GetMessages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult GetMessages() {
            List<SandboxEventArgs> args = new List<SandboxEventArgs>();
            try {
                while (MessageQueue.Instance.Count > 0)
                    args.Add(MessageQueue.Instance.Dequeue());
            } catch {
                return StatusCode(418);
            }

            return Ok(args.ToArray());
        }
    }
}
