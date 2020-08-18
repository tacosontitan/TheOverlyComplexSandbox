using Sandbox.Core;
using System;

namespace Sandbox.Modules.CSharp.General {
    [SandboxModule("Template Test", "template", "A simple module created to test the sandbox module item template. New versions of the template will replace this module as they are released. This module reflects version 1.0.0.0.")]
    public class TemplateTestModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Echo", "What text should we echo?", DisplayElement.Textbox)]
        public string ExampleParameter { get; set; } = "Hello from the sandbox module template v1.0.0.0.";

        #endregion

        #region Module Implementation

        /// <summary>
        /// Executes the TemplateTestModule module.
        /// </summary>
        protected override void Execute() {
            SendResponse(SandboxEventType.Information, ExampleParameter);
        }

        #endregion

    }
}