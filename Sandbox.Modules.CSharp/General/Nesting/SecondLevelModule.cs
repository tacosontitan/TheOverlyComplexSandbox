using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General.Nesting {
    [SandboxModule("Second Layer", "cs-nesting-2", "Tests the nesting of modules within the UI.")]
    [ModuleTags("general", "sandbox", "testing")]
    public class SecondLevelModule : SandboxModule {
        protected override void Execute() => SendResponse(SandboxEventType.None, "A nested module was executed successfully.");
    }
}