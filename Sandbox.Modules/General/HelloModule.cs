using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General {
    [SandboxModule("Hello World", "hello", "A simple module to test using reflection to discover modules.")]
    public class HelloModule : SandboxModule {
        protected override void Execute() {
            OnExecutionStarted();
            SendResponse(SandboxEventType.None, "Hello World!");
            OnExecutionCompleted();
        }
    }
}
