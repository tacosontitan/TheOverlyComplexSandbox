using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
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
            ModuleManager.Instance.ExecutionStarted += QueueEvent;
            ModuleManager.Instance.ExecutionCancelled += QueueEvent;
            ModuleManager.Instance.ExecutionFailed += QueueEvent;
            ModuleManager.Instance.ExecutionCompleted += QueueEvent;
            ModuleManager.Instance.RequestReceived += QueueEvent;
            ModuleManager.Instance.ResponseReceived += QueueEvent;
        }

        #endregion

        #region Module Manager Events

        private void QueueEvent(object sender, SandboxEventArgs e) => MessageQueue.Instance.Enqueue(e);

        #endregion

    }
}