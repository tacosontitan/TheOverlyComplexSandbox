using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Modules.General {
    [ModuleDescription("Hello World", "hello", "A simple module to test using reflection to discover modules.")]
    public class HelloModule : SandboxModule {
        public override void Execute() {
            OnExecutionStarted();
            SendResponse(ModuleCommunicationType.None, "Hello World!");
            OnExecutionCompleted();
        }

        protected override void ProcessResponse(ModuleCommunicationData data) {
            
        }
    }
}
