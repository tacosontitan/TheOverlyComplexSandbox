using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Core;
using Sandbox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Sandbox.ASP {
    [Route("[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase {
        [HttpPost("Execute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult Execute([FromBody]object jsonData) {
            try {
                Dictionary<string, object> deserializedData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData.ToString());
                if (Guid.TryParse(deserializedData["id"].ToString(), out Guid id)) {
                    ModuleParameter[] parameters = JsonSerializer.Deserialize<ModuleParameter[]>(deserializedData["parameters"].ToString(), new JsonSerializerOptions() {
                        PropertyNameCaseInsensitive = true
                    });
                    foreach (ModuleParameter parameter in parameters)
                        parameter.Value = ((JsonElement)parameter.Value).ToString();

                    ModuleService.Instance.TryExecute(id, parameters);
                }
            } catch {
                return StatusCode(418);
            }

            return Ok();
        }
        [HttpPost("GetDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult GetDetails([FromBody] object request) {
            Dictionary<string, object> deserializedData = JsonSerializer.Deserialize<Dictionary<string, object>>(request.ToString());
            if (Guid.TryParse(deserializedData["id"].ToString(), out Guid moduleID)) {
                try {
                    return Ok(ModuleService.Instance.Modules.Single(s => s.ID == moduleID));
                } catch {
                    return StatusCode(418);
                }
            }

            return StatusCode(418);
        }
        [HttpPost("GetParameters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult GetParameters([FromBody]object request) {
            Dictionary<string, object> deserializedData = JsonSerializer.Deserialize<Dictionary<string, object>>(request.ToString());
            if (Guid.TryParse(deserializedData["id"].ToString(), out Guid moduleID)) {
                ModuleParameter[] parameters;
                try {
                    parameters = ModuleService.Instance.GetModuleParameters(moduleID);
                } catch {
                    return StatusCode(418);
                }

                return Ok(parameters);
            } return StatusCode(418);
        }
    }
}