using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General {
    [SandboxModule("Hello World", "cs-hello", "This module is a simple test of integrating the C# module collection with the module service.")]
    [ModuleTags("general", "sandbox", "testing")]
    public class HelloModule : SandboxModule {
        protected override void Execute() {
            SendResponse(SandboxEventType.None, "Hello from C#!");
        }
    }
}
