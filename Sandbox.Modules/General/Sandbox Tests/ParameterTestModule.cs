using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Modules.General.Sandbox_Tests {
    [SandboxModule("Parameter Attribute Test", "params", "Tests the module parameter attribute object.")]
    public class ParameterTestModule : SandboxModule {

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