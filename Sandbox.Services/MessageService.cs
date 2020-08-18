using Sandbox.Core;
using System.Collections.Generic;

namespace Sandbox.Services {
    public class MessageService : Queue<SandboxEventArgs> {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static MessageService instance;
        public static MessageService Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new MessageService();

                return instance;
            }
        }

        #endregion

    }
}