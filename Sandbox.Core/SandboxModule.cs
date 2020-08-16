using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox.Core {
    public abstract class SandboxModule {

        #region Properties

        public string Name { get; private set; }
        public string Category { get; private set; }
        public string ExecutionKey { get; private set; }
        public string Description { get; private set; }

        #endregion

        #region Constructors

        public SandboxModule() {
            Category = GetType().Namespace.Replace("Sandbox.Modules.", string.Empty).Replace('.', '/').Replace('_', ' ');
            SandboxModuleAttribute moduleDescription = GetType().GetCustomAttribute<SandboxModuleAttribute>();
            if (moduleDescription != null) {
                Name = moduleDescription.Name;
                ExecutionKey = moduleDescription.ExecutionKey;
                Description = moduleDescription.Description;
            }
        }

        #endregion

        #region Public Methods

        public void Run() {
            try {
                OnExecutionStarted();
                Execute();
                OnExecutionCompleted();
            } catch (Exception e) {
                OnExecutionFailed(new SandboxEventArgs(e, Name, e.Message, SandboxEventType.Failure));
            }
        }

        #endregion

        #region Protected Methods

        protected abstract void Execute();

        protected void RequestInput(SandboxEventArgs args) => OnInputRequested(args);
        protected void SendResponse(SandboxEventType responseType, string message) => OnResponseReceived(new SandboxEventArgs(null, Name, message, responseType));

        #endregion

        #region Execution Events

        public event EventHandler<SandboxEventArgs> ExecutionStarted;
        protected virtual void OnExecutionStarted(SandboxEventArgs args = null) {
            ExecutionStarted?.Invoke(this, new SandboxEventArgs(null, Name, $"The {Name} module has started execution.", SandboxEventType.Information));
            if (args != null)
                ExecutionStarted?.Invoke(this, args);
        }
        public event EventHandler<SandboxEventArgs> ExecutionCancelled;
        protected virtual void OnExecutionCancelled(SandboxEventArgs args = null) {
            ExecutionCancelled?.Invoke(this, new SandboxEventArgs(null, Name, $"The {Name} module has successfully stopped execution.", SandboxEventType.Information));
            if (args != null)
                ExecutionStarted?.Invoke(this, args);
        }
        public event EventHandler<SandboxEventArgs> ExecutionCompleted;
        protected virtual void OnExecutionCompleted(SandboxEventArgs args = null) {
            ExecutionCompleted?.Invoke(this, new SandboxEventArgs(null, Name, $"The {Name} module has completed execution.", SandboxEventType.Success));
            if (args != null)
                ExecutionStarted?.Invoke(this, args);
        }
        public event EventHandler<SandboxEventArgs> ExecutionFailed;
        protected virtual void OnExecutionFailed(SandboxEventArgs args = null) {
            ExecutionFailed?.Invoke(this, new SandboxEventArgs(null, Name, $"The {Name} module failed.", SandboxEventType.Failure));
            if (args != null)
                ExecutionStarted?.Invoke(this, args);
        }

        #endregion

        #region Messaging Events

        public event EventHandler<SandboxEventArgs> InputRequested;
        protected virtual void OnInputRequested(SandboxEventArgs args) => InputRequested?.Invoke(this, args);
        public event EventHandler<SandboxEventArgs> ResponseReceived;
        protected virtual void OnResponseReceived(SandboxEventArgs args) => ResponseReceived?.Invoke(this, args);

        #endregion

    }
}