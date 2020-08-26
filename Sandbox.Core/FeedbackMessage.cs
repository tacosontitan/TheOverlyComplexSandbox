using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Core {
    public class FeedbackMessage {

        #region Properties

        /// <summary>
        /// The display name of the user that submitted this message.
        /// </summary>
        public string UserDisplayName { get; set; }
        /// <summary>
        /// The feedback provided by an end user.
        /// </summary>
        public string Message { get; set; }

        #endregion

        #region Constructors

        public FeedbackMessage(string userDisplayName, string message) {
            UserDisplayName = userDisplayName;
            Message = message;
        }

        #endregion

    }
}