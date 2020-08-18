using Sandbox.Core;

namespace Sandbox.Modules.CSharp.General.Parameters {
    [SandboxModule("Display Elements", "test-disp", @"Parameters can utilize the DisplayElement property to adjust how the web and windows forms applications will display them.\n\nAs more element types are supported, this module will be updated to test them. All responses given are simply echoed back to ensure the supplied value was received.")]
    public class DisplayElementModule : SandboxModule {

        #region Parameters

        [ModuleParameter("Is User's Birthday", "Is today your birthday?", DisplayElement.Checkbox, Required = true)]
        public bool IsUsersBirthday { get; set; }
        [ModuleParameter("First Name", "What's your first name?", DisplayElement.Textbox, Required = true)]
        public string FirstName { get; set; }
        [ModuleParameter("Last Name", "What's your last name?", DisplayElement.RichTextbox, Required = true)]
        public string LastName { get; set; }
        [ModuleParameter("About Me", "Tell us about yourself:", DisplayElement.Textbox, Required = true)]
        public string AboutMe { get; set; }

        #endregion

        #region Module Implementation

        protected override void Execute() {
            string response = $@"Hey there {FirstName} from the house of {LastName}! Thanks for testing out the sandbox! We're overjoyed that you took the time to stop by and tell us about yourself. You told us:

{AboutMe}

Oh and, today is {(IsUsersBirthday ? string.Empty : "NOT")} your birthday.{(IsUsersBirthday ? " Happy Birthday!" : string.Empty)}";
            SendResponse(SandboxEventType.None, response);
        }

        #endregion

    }
}