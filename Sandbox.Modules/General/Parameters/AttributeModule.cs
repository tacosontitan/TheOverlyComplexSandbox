using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General.Parameters {
    [SandboxModule("Attributes", "attr", "Tests the module parameter attribute object.")]
    public class AttributeModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Message", "What message should be echoed?", DisplayElement.Textbox, Required = true)]
        public string Message { get; set; }
        [ModuleParameter("Iterations", "How many times should we echo the message?", DisplayElement.Textbox, Required = true)]
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