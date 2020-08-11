using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox.Modules {
    public class ModuleManager {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static ModuleManager instance;
        public static ModuleManager Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new ModuleManager();

                return instance;
            }
        }

        #endregion

        #region Fields

        private List<SandboxModule> modules = new List<SandboxModule>();

        #endregion

        #region Properties

        public object SuccessColor { get; set; }
        public object WarningColor { get; set; }
        public object ErrorColor { get; set; }
        public object InformationColor { get; set; }
        public object DefaultColor { get; set; }

        #endregion

        #region Constructors

        private ModuleManager() => Discover();

        #endregion

        #region Public Methods

        public bool TryExecute(string key) {
            if (modules.Exists(e => string.Equals(e.ExecutionKey, key, StringComparison.InvariantCultureIgnoreCase))) {
                try {
                    SandboxModule module = modules.Single(s => string.Equals(s.ExecutionKey, key, StringComparison.InvariantCultureIgnoreCase));
                    module.ExecutionStarted += Module_ExecutionStarted;
                    module.ExecutionFailed += Module_ExecutionFailed;
                    module.ExecutionCompleted += Module_ExecutionCompleted;
                    module.ExecutionCancelled += Module_ExecutionCancelled;
                    module.InputRequested += Module_RequestReceived;
                    module.ResponseReceived += Module_ResponseReceived;
                    module.Execute();
                    return true;
                } catch { return false; }
            }

            return false;
        }
        public void ProvideResponse(SandboxModule recipient, ModuleCommunicationData response) => recipient.CaptureResponse(response);

        #endregion

        #region Module Discovery

        private void Discover() {
            IEnumerable<Type> moduleTypes = Assembly.GetAssembly(typeof(ModuleManager)).GetTypes().Where(w => typeof(SandboxModule).IsAssignableFrom(w));
            foreach (Type moduleType in moduleTypes)
                modules.Add((SandboxModule)Activator.CreateInstance(moduleType));
        }

        #endregion

        #region Module Events

        private void Module_ExecutionStarted(object sender, ModuleCommunicationData e) => OnExecutionStarted(sender, e);
        private void Module_ExecutionCompleted(object sender, ModuleCommunicationData e) => OnExecutionCompleted(sender, e);
        private void Module_ExecutionFailed(object sender, ModuleCommunicationData e) => OnExecutionFailed(sender, e);
        private void Module_ExecutionCancelled(object sender, ModuleCommunicationData e) => OnExecutionCancelled(sender, e);
        private void Module_RequestReceived(object sender, ModuleCommunicationData e) => OnRequestReceived(sender, e);
        private void Module_ResponseReceived(object sender, ModuleCommunicationData e) => OnResponseReceived(sender, e);

        #endregion

        #region Events

        public event EventHandler<ModuleCommunicationData> ExecutionStarted;
        protected virtual void OnExecutionStarted(object sender, ModuleCommunicationData response) => ExecutionStarted?.Invoke(sender, response);
        public event EventHandler<ModuleCommunicationData> ExecutionCompleted;
        protected virtual void OnExecutionCompleted(object sender, ModuleCommunicationData response) => ExecutionCompleted?.Invoke(sender, response);
        public event EventHandler<ModuleCommunicationData> ExecutionFailed;
        protected virtual void OnExecutionFailed(object sender, ModuleCommunicationData response) => ExecutionFailed?.Invoke(sender, response);
        public event EventHandler<ModuleCommunicationData> ExecutionCancelled;
        protected virtual void OnExecutionCancelled(object sender, ModuleCommunicationData response) => ExecutionCancelled?.Invoke(sender, response);
        public event EventHandler<ModuleCommunicationData> RequestReceived;
        protected virtual void OnRequestReceived(object sender, ModuleCommunicationData response) => RequestReceived?.Invoke(sender, response);
        public event EventHandler<ModuleCommunicationData> ResponseReceived;
        protected virtual void OnResponseReceived(object sender, ModuleCommunicationData response) => ResponseReceived?.Invoke(sender, response);

        #endregion

    }
}