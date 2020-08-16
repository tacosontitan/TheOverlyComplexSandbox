using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sandbox.Core;
using Sandbox.Modules;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                ModuleParameter[] parameters = JsonSerializer.Deserialize<ModuleParameter[]>(deserializedData["parameters"].ToString(), new JsonSerializerOptions() {
                    PropertyNameCaseInsensitive = true
                });
                foreach (ModuleParameter parameter in parameters)
                    parameter.Value = ((JsonElement)parameter.Value).ToString();

                ModuleManager.Instance.TryExecute(deserializedData["key"].ToString(), parameters);
            } catch {
                return StatusCode(418);
            }

            return Ok();
        }
        [HttpPost("GetParameters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        public IActionResult GetParameters(string key) {
            ModuleParameter[] parameters;
            try {
                parameters = ModuleManager.Instance.GetModuleParameters(key);
            } catch {
                return StatusCode(418);
            }

            return Ok(parameters);
        }
    }
}