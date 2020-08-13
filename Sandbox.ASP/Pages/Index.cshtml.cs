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

        #region Fields

        private readonly ILogger<IndexModel> _logger;

        #endregion

        #region Properties

        public string[] ModuleCategories { get; private set; }
        public SandboxModule[] DiscoveredModules => ModuleManager.Instance.Modules;

        #endregion

        #region Page Setup

        public IndexModel(ILogger<IndexModel> logger) => _logger = logger;
        public void OnGet() {
            SandboxModule[] firstModuleFromEachCategory = DiscoveredModules.GroupBy(g => g.Category).Select(s => s.First()).ToArray();
            List<string> categories = new List<string>();
            foreach (SandboxModule module in firstModuleFromEachCategory)
                categories.Add(module.Category);

            ModuleCategories = categories.ToArray();
        }

        #endregion

        #region Exposed Methods

        public void ExecuteModule(string executionKey) => ModuleManager.Instance.TryExecute(executionKey);

        #endregion

    }
}