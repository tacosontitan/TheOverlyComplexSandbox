using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General {
    [SandboxModule("Hello World", "cs-hello", "A simple module to test using reflection to discover modules.")]
    [ModuleTags("general", "sandbox", "testing")]
    public class HelloModule : SandboxModule {
        protected override void Execute() {
            SendResponse(SandboxEventType.None, "Hello World!");
        }
    }
}
