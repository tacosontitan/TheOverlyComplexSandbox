using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox.Core {
    public abstract class SandboxModule {

        #region Field

        private bool containsSteps = false;
        private int currentStep = 0;
        private MethodInfo[] executionSteps = null;

        #endregion

        #region Properties

        public string Name { get; private set; }
        public string ExecutionKey { get; private set; }
        public string Description { get; private set; }

        #endregion

        #region Constructors

        public SandboxModule() {
            DiscoverModuleDescription();
            DiscoverModuleSteps();
        }

        #endregion

        #region Public Methods

        public abstract void Execute();
        public void CaptureResponse(ModuleCommunicationData response) {
            if (string.Equals("exit", response.Data.ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                OnExecutionCancelled();
                return;
            }

            ProcessResponse(response);
        }

        #endregion

        #region Protected Methods

        protected void BeginSteppedExecution(object sender) => Step(sender, new ModuleCommunicationData(ModuleCommunicationType.None, null));
        protected void Step(object sender, ModuleCommunicationData data) {
            if (currentStep < executionSteps.Length) {
                executionSteps[currentStep++].Invoke(sender, new object[] { data });
                return;
            }

            OnExecutionCompleted(data);
        }
        protected void RequestInput(ModuleCommunicationType communicationType, object data) => OnInputRequested(new ModuleCommunicationData(communicationType, data));
        protected abstract void ProcessResponse(ModuleCommunicationData data);
        protected void ReprocessPreviousStep(object sender, ModuleCommunicationData data) {
            currentStep--;
            Step(sender, data);
        }
        protected void SendResponse(ModuleCommunicationType responseType, object data) => OnResponseReceived(new ModuleCommunicationData(responseType, data));

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
        private void DiscoverModuleSteps() {
            try {
                IEnumerable<MethodInfo> methods = GetType().GetMethods().Where(w => w.GetCustomAttribute<ModuleStepAttribute>() != null);
                executionSteps = methods.OrderBy(o => o.GetCustomAttribute<ModuleStepAttribute>().StepNumber).ToArray();

                if (executionSteps == null || executionSteps?.Length == 0)
                    containsSteps = false;
            } catch {
                containsSteps = false;
            }
        }

        #endregion

        #region Execution Events

        public event EventHandler<ModuleCommunicationData> ExecutionStarted;
        protected virtual void OnExecutionStarted(ModuleCommunicationData response = null) {
            ExecutionStarted?.Invoke(this, new ModuleCommunicationData(ModuleCommunicationType.Information, $"The {Name} module has started execution."));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }
        public event EventHandler<ModuleCommunicationData> ExecutionCancelled;
        protected virtual void OnExecutionCancelled(ModuleCommunicationData response = null) {
            ExecutionCancelled?.Invoke(this, new ModuleCommunicationData(ModuleCommunicationType.Information, $"The {Name} module has successfully stopped execution."));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }
        public event EventHandler<ModuleCommunicationData> ExecutionCompleted;
        protected virtual void OnExecutionCompleted(ModuleCommunicationData response = null) {
            ExecutionCompleted?.Invoke(this, new ModuleCommunicationData(ModuleCommunicationType.Information, $"The {Name} module has completed execution."));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }
        public event EventHandler<ModuleCommunicationData> ExecutionFailed;
        protected virtual void OnExecutionFailed(ModuleCommunicationData response = null) {
            ExecutionFailed?.Invoke(this, new ModuleCommunicationData(ModuleCommunicationType.Information, $"The {Name} module failed."));
            if (response != null)
                ExecutionStarted?.Invoke(this, response);
        }

        #endregion

        #region Messaging Events

        public event EventHandler<ModuleCommunicationData> InputRequested;
        protected virtual void OnInputRequested(ModuleCommunicationData request) => InputRequested?.Invoke(this, request);
        public event EventHandler<ModuleCommunicationData> ResponseReceived;
        protected virtual void OnResponseReceived(ModuleCommunicationData response) => ResponseReceived?.Invoke(this, response);

        #endregion

    }
}