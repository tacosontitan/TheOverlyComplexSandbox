using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Services;

namespace Sandbox.Presentation.Web.APIControllers {
    [Route("[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase {
        [HttpPost("Submit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult SubmitFeedback([FromBody] object request) {
            Dictionary<string, object> deserializedData = JsonSerializer.Deserialize<Dictionary<string, object>>(request.ToString());
            try {
                FeedbackService.Instance.Submit(deserializedData["displayName"].ToString(), deserializedData["feedback"].ToString());
                return Ok();
            } catch {
                return StatusCode(418);
            }
        }
        [HttpPost("Fetch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult FetchFeedback() {
            try {
                return Ok(FeedbackService.Instance.GetFeedback());
            } catch {
                return StatusCode(418);
            }
        }
    }
}
