using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Core;
using Sandbox.Services;
using System.Collections.Generic;

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
                while (MessageService.Instance.Count > 0)
                    args.Add(MessageService.Instance.Dequeue());
            } catch {
                return StatusCode(418);
            }

            return Ok(args.ToArray());
        }
    }
}
