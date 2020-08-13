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
        public SandboxModule[] Modules => modules.ToArray();

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
                    module.Run();
                    return true;
                } catch { return false; }
            }

            return false;
        }

        #endregion

        #region Module Discovery

        private void Discover() {
            IEnumerable<Type> moduleTypes = Assembly.GetAssembly(typeof(ModuleManager)).GetTypes().Where(w => typeof(SandboxModule).IsAssignableFrom(w));
            foreach (Type moduleType in moduleTypes)
                modules.Add((SandboxModule)Activator.CreateInstance(moduleType));
        }

        #endregion

        #region Module Events

        private void Module_ExecutionStarted(object sender, SandboxEventArgs e) => OnExecutionStarted(sender, e);
        private void Module_ExecutionCompleted(object sender, SandboxEventArgs e) => OnExecutionCompleted(sender, e);
        private void Module_ExecutionFailed(object sender, SandboxEventArgs e) => OnExecutionFailed(sender, e);
        private void Module_ExecutionCancelled(object sender, SandboxEventArgs e) => OnExecutionCancelled(sender, e);
        private void Module_RequestReceived(object sender, SandboxEventArgs e) => OnRequestReceived(sender, e);
        private void Module_ResponseReceived(object sender, SandboxEventArgs e) => OnResponseReceived(sender, e);

        #endregion

        #region Events

        public event EventHandler<SandboxEventArgs> ExecutionStarted;
        protected virtual void OnExecutionStarted(object sender, SandboxEventArgs response) => ExecutionStarted?.Invoke(sender, response);
        public event EventHandler<SandboxEventArgs> ExecutionCompleted;
        protected virtual void OnExecutionCompleted(object sender, SandboxEventArgs response) {
            ExecutionCompleted?.Invoke(sender, response);
            SandboxModule module = sender as SandboxModule;
            module.ExecutionStarted -= Module_ExecutionStarted;
            module.ExecutionFailed -= Module_ExecutionFailed;
            module.ExecutionCompleted -= Module_ExecutionCompleted;
            module.ExecutionCancelled -= Module_ExecutionCancelled;
            module.InputRequested -= Module_RequestReceived;
            module.ResponseReceived -= Module_ResponseReceived;
        }
        public event EventHandler<SandboxEventArgs> ExecutionFailed;
        protected virtual void OnExecutionFailed(object sender, SandboxEventArgs response) => ExecutionFailed?.Invoke(sender, response);
        public event EventHandler<SandboxEventArgs> ExecutionCancelled;
        protected virtual void OnExecutionCancelled(object sender, SandboxEventArgs response) => ExecutionCancelled?.Invoke(sender, response);
        public event EventHandler<SandboxEventArgs> RequestReceived;
        protected virtual void OnRequestReceived(object sender, SandboxEventArgs eventArgs) => RequestReceived?.Invoke(sender, eventArgs);
        public event EventHandler<SandboxEventArgs> ResponseReceived;
        protected virtual void OnResponseReceived(object sender, SandboxEventArgs response) => ResponseReceived?.Invoke(sender, response);

        #endregion

    }
}