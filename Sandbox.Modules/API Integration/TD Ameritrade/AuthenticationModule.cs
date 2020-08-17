﻿using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Modules.API_Integration.TD_Ameritrade {
    [SandboxModule("TD Ameritrade Authentication", "tdam", "Integrates with the TD Ameritrade API.")]
    public class AuthenticationModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Redirect URL", "What is the redirect URL for your application?", DisplayElement.Textbox, Required = true)]
        public string RedirectURL { get; set; }
        [ModuleParameter("Client ID", "What is your client ID?", DisplayElement.Textbox, Required = true)]
        public string ClientID { get; set; }
        [ModuleParameter("Initial Access Code", "What is your initial OAuth2.0 code?", DisplayElement.RichTextbox, Required = true)]
        public string InitialAccessCode { get; set; }

        #endregion

        #region Module Implementation

        protected override void Execute() {
            TDAmeritradeService.Instance.AuthenticationSucceeded += SendInformation;
            TDAmeritradeService.Instance.AuthenticationFailed += SendInformation;
            TDAmeritradeService.Instance.Initialize(RedirectURL, ClientID, InitialAccessCode);
        }
        private void SendInformation(object sender, string e) => SendResponse(SandboxEventType.Information, e);

        #endregion

    }
}