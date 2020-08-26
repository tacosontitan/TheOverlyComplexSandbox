using Sandbox.Core;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Sandbox.Modules.CSharp.API_Integration.Twilio {
    [SandboxModule("Send a Text", "cs-twilio-text", "Sends a text message using Twilio's restful APIs through their C# helper package offered in NuGet.")]
    [ModuleTags("twilio", "sms", "api", "texting", "messaging")]
    public class TextMessageModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Account SID", "Pleast provide your Twilio SID.", DisplayElement.Textbox, Required = true)]
        public string AccountSID { get; set; }
        [ModuleParameter("Authentication Token", "Please provide your Twilio authentication token.", DisplayElement.Textbox, Required = true)]
        public string AuthenticationToken { get; set; }
        [ModuleParameter("Origin Number", "What phone number should the text message be sent from? Format is 5555555555.", DisplayElement.Textbox, Required = true)]
        public string PhoneNumberFrom { get; set; }
        [ModuleParameter("Destination Number", "What phone number should the text message be sent to? Format is 5555555555.", DisplayElement.Textbox, Required = true)]
        public string PhoneNumberTo { get; set; }
        [ModuleParameter("Message", "What should this text message say?", DisplayElement.RichTextbox, Required = true)]
        public string TextMessage { get; set; }

        #endregion

        #region Module Implementation

        /// <summary>
        /// Executes the TextMessageModule module.
        /// </summary>
        protected override void Execute() {
            TwilioClient.Init(AccountSID, AuthenticationToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber($"+1{PhoneNumberTo}"));
            messageOptions.From = new PhoneNumber($"+1{PhoneNumberFrom}");
            messageOptions.Body = TextMessage;

            var message = MessageResource.Create(messageOptions);
            SendResponse(SandboxEventType.Information, message.Body);
        }

        #endregion

    }
}