using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General.Parameters {
    [SandboxModule("Echo", "cs-echo", "This is a simple echo test crafted to test the initial parameter attribute used on properties within modules. It simply echos the specified text a specified number of times.")]
    [ModuleTags("general", "sandbox", "testing")]
    public class EchoModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Message", "What message should be echoed?", DisplayElement.Textbox, Required = true)]
        public string Message { get; set; }
        [ModuleParameter("Iterations", "How many times should it be echoed?", DisplayElement.Textbox, Required = true)]
        public int Iterations { get; set; }

        #endregion

        #region Module Implementation

        protected override void Execute() {
            for (int i = 0; i < Iterations; i++)
                SendResponse(SandboxEventType.None, Message);
        }

        #endregion

    }
}