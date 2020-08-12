using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox.Core {
    public abstract class SandboxModule {

        #region Fields

        private PropertyInfo[] parameters;

        #endregion

        #region Properties

        public bool HasParameters { get; private set; } = false;
        public string Name { get; private set; }
        public string ExecutionKey { get; private set; }
        public string Description { get; private set; }

        #endregion

        #region Constructors

        public SandboxModule() {
            DiscoverModuleDescription();
            DiscoverModuleParameters();
        }

        #endregion

        #region Public Methods

        public void Run() {
            OnExecutionStarted();

            if (HasParameters) {
                foreach (PropertyInfo parameter in parameters) {
                    bool parameterSet = false;
                    do {
                        ModuleParameterAttribute description = parameter.GetCustomAttribute<ModuleParameterAttribute>();
                        var instance = Convert.ChangeType(this, GetType());
                        SandboxEventArgs eventArgs = new SandboxEventArgs(parameter.GetValue(instance), description.RequestMessage, SandboxEventType.None);
                        RequestInput(eventArgs);

                        try {
                            parameter.SetValue(instance, Convert.ChangeType(eventArgs.Data, parameter.PropertyType));
                            parameterSet = true;
                        } catch {
                            SendResponse(SandboxEventType.Failure, $"The response received was invalid.");
                            if (!description.Required)
                                parameterSet = true;
                        }
                    } while (!parameterSet);
                }
            }

            Execute();
            OnExecutionCompleted();
        }

        #endregion

        #region Protected Methods

        protected abstract void Execute();

        protected void RequestInput(SandboxEventArgs eventArgs) => OnInputRequested(eventArgs);
        protected void SendResponse(SandboxEventType responseType, string message) => OnResponseReceived(new SandboxEventArgs(null, message, responseType));

        #endregion

        #region Private Methods

        private void DiscoverModuleDescription() {
            SandboxModuleAttribute moduleDescription = GetType().GetCustomAttribute<SandboxModuleAttribute>();
            if (moduleDescription != null) {
                Name = moduleDescription.Name;
                ExecutionKey = moduleDescription.ExecutionKey;
                Description = moduleDescription.Description;
            }
        }
        private void DiscoverModuleParameters() {
            try {
                parameters = GetType().GetProperties().Where(w => w.GetCustomAttribute<ModuleParameterAttribute>() != null).ToArray();
                if (parameters != null && parameters?.Length > 0)
                    HasParameters = true;
            } catch {
                HasParameters = false;
            }
        }

        #endregion

        #region Execution Events

        public event EventHandler<SandboxEventArgs> ExecutionStarted;
        protected virtual void OnExecutionStarted(SandboxEventArgs response = null) {
            ExecutionStarted?.Invoke(this, new SandboxEventArgs(null, $"The {Name} module has started execution.", SandboxEventType.Information));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }
        public event EventHandler<SandboxEventArgs> ExecutionCancelled;
        protected virtual void OnExecutionCancelled(SandboxEventArgs response = null) {
            ExecutionCancelled?.Invoke(this, new SandboxEventArgs(null, $"The {Name} module has successfully stopped execution.", SandboxEventType.Information));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }
        public event EventHandler<SandboxEventArgs> ExecutionCompleted;
        protected virtual void OnExecutionCompleted(SandboxEventArgs response = null) {
            ExecutionCompleted?.Invoke(this, new SandboxEventArgs(null, $"The {Name} module has completed execution.", SandboxEventType.Success));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }
        public event EventHandler<SandboxEventArgs> ExecutionFailed;
        protected virtual void OnExecutionFailed(SandboxEventArgs response = null) {
            ExecutionFailed?.Invoke(this, new SandboxEventArgs(null, $"The {Name} module failed.", SandboxEventType.Failure));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }

        #endregion

        #region Messaging Events

        public event EventHandler<SandboxEventArgs> InputRequested;
        protected virtual void OnInputRequested(SandboxEventArgs eventArgs) => InputRequested?.Invoke(this, eventArgs);
        public event EventHandler<SandboxEventArgs> ResponseReceived;
        protected virtual void OnResponseReceived(SandboxEventArgs response) => ResponseReceived?.Invoke(this, response);

        #endregion

    }
}