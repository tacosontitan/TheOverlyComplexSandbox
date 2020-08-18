using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sandbox.Core;
using Sandbox.Services;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox.ASP.Pages {
    public class IndexModel : PageModel {

        #region Fields

        private readonly ILogger<IndexModel> _logger;

        #endregion

        #region Properties

        public string[] ModuleCategories { get; private set; }
        public SandboxModule[] DiscoveredModules => ModuleService.Instance.Modules;

        #endregion

        #region Page Setup

        public IndexModel(ILogger<IndexModel> logger) => _logger = logger;
        public void OnGet() {
            SandboxModule[] firstModuleFromEachCategory = DiscoveredModules.GroupBy(g => g.Category).Select(s => s.First()).ToArray();
            List<string> categories = new List<string>();
            foreach (SandboxModule module in firstModuleFromEachCategory)
                categories.Add(module.Category);

            ModuleCategories = categories.ToArray();
            ModuleService.Instance.ExecutionStarted += QueueEvent;
            ModuleService.Instance.ExecutionCancelled += QueueEvent;
            ModuleService.Instance.ExecutionFailed += QueueEvent;
            ModuleService.Instance.ExecutionCompleted += QueueEvent;
            ModuleService.Instance.RequestReceived += QueueEvent;
            ModuleService.Instance.ResponseReceived += QueueEvent;
        }

        #endregion

        #region Module Manager Events

        private void QueueEvent(object sender, SandboxEventArgs e) => MessageService.Instance.Enqueue(e);

        #endregion

    }
}