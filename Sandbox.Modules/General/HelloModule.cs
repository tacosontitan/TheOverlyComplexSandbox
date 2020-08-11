using Sandbox.Core;

namespace Sandbox.Modules.General {
    [SandboxModule("Hello World", "hello", "A simple module to test using reflection to discover modules.")]
    public class HelloModule : SandboxModule {
        public override void Execute() {
            OnExecutionStarted();
            SendResponse(ModuleCommunicationType.None, "Hello World!");
            OnExecutionCompleted();
        }

        protected override void ProcessResponse(ModuleCommunicationData data) {
            // TODO, create an interface so this isn't forced on modules that don't need it.
        }
    }
}
