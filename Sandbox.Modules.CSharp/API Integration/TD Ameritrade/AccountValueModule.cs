using Sandbox.Core;
using System;

namespace Sandbox.Modules.CSharp.API_Integration.TD_Ameritrade {
    [SandboxModule("Account Value", "cs-td-bal", "Fetches the current account value for the supplied account number and OAuth information. This module does not store personal information and simply communicates with the TD Ameritrade API on your behalf.")]
    [ModuleTags("finance", "investing", "api", "web-services")]
    class AccountValueModule : SandboxModule {

        [ModuleParameter("Message", "What message should be echoed?", DisplayElement.Textbox, Required = true)]
        public string Message { get; set; }

        protected override void Execute() {
            throw new NotImplementedException();
        }
    }
}
