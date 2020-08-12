using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Core {
    public class SandboxEventArgs {

        #region Properties

        public object Data { get; set; }
        public string Message { get; set; }
        public SandboxEventType EventType { get; set; }

        #endregion

        #region Constructors

        public SandboxEventArgs(object data, string message, SandboxEventType eventType) {
            Data = data;
            Message = message;
            EventType = eventType;
        }

        #endregion

    }
}