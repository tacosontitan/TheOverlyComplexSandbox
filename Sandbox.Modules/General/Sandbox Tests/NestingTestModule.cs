using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Modules.General.Sandbox_Tests {
    [SandboxModule("Nested Module Test", "nest", "Tests the nesting of modules within the UI.")]
    public class NestingTestModule : SandboxModule {
        protected override void Execute() => SendResponse(SandboxEventType.None, "A nested module was executed successfully.");
    }
}