using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sandbox.ASP {
    public class MessageQueue : Queue<SandboxEventArgs> {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static MessageQueue instance;
        public static MessageQueue Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new MessageQueue();

                return instance;
            }
        }

        #endregion

    }
}