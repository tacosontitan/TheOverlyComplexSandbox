using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sandbox.Core;
using Sandbox.Modules;

namespace Sandbox.ASP.Pages {
    public class IndexModel : PageModel {

        public SandboxModule[] DiscoveredModules => ModuleManager.Instance.Modules;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        public void OnGet() {

        }
    }
}
