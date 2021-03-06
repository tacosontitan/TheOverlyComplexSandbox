﻿using Sandbox.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sandbox.Services {
    public class ModuleService {

        #region Singleton Setup

        private static readonly object instanceLock = new object();
        private static ModuleService instance;
        public static ModuleService Instance {
            get {
                if (instance == null)
                    lock (instanceLock)
                        if (instance == null)
                            instance = new ModuleService();

                return instance;
            }
        }

        #endregion

        #region Fields

        private List<SandboxModule> modules = new List<SandboxModule>();

        #endregion

        #region Properties

        public SandboxModule[] Modules => modules.ToArray();

        #endregion

        #region Constructors

        private ModuleService() => Discover();

        #endregion

        #region Public Methods

        public bool Exists(Guid id) => modules.Exists(e => e.ID == id);
        public bool TryExecute(Guid id, params ModuleParameter[] parameters) {
            if (Exists(id)) {
                try {
                    SandboxModule module = modules.Single(s => s.ID == id);
                    var instance = Convert.ChangeType(module, module.GetType());
                    PropertyInfo[] moduleParameters = module.GetType().GetProperties().Where(w => w.GetCustomAttribute<ModuleParameterAttribute>() != null).ToArray();
                    if (moduleParameters != null && moduleParameters.Length > 0) {
                        foreach (ModuleParameter param in parameters) {
                            PropertyInfo property = moduleParameters.Single(s => s.Name == param.Name);
                            dynamic paramValue = Convert.ChangeType(param.Value, property.PropertyType);
                            property.SetValue(instance, paramValue);
                        }
                    }

                    module.ExecutionStarted += Module_ExecutionStarted;
                    module.ExecutionFailed += Module_ExecutionFailed;
                    module.ExecutionCompleted += Module_ExecutionCompleted;
                    module.ExecutionCancelled += Module_ExecutionCancelled;
                    module.InputRequested += Module_RequestReceived;
                    module.ResponseReceived += Module_ResponseReceived;
                    module.Run();
                    return true;
                } catch { return false; }
            } return false;
        }
        public ModuleParameter[] GetModuleParameters(Guid id) {
            List<ModuleParameter> result = new List<ModuleParameter>();
            SandboxModule module = modules.Single(s => s.ID == id);
            var instance = Convert.ChangeType(module, module.GetType());
            PropertyInfo[] parameters = module.GetType().GetProperties().Where(w => w.GetCustomAttribute<ModuleParameterAttribute>() != null).ToArray();
            if (parameters != null && parameters?.Length > 0) {
                foreach (PropertyInfo parameter in parameters) {
                    ModuleParameterAttribute description = parameter.GetCustomAttribute<ModuleParameterAttribute>();
                    result.Add(new ModuleParameter() {
                        Name = parameter.Name,
                        DisplayName = description.DisplayName,
                        DisplayElement = description.DisplayElement,
                        RequestMessage = description.RequestMessage,
                        Required = description.Required,
                        MinValue = description.MinValue,
                        MaxValue = description.MaxValue,
                        Value = parameter.GetValue(instance)
                    });
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Module Discovery

        private void Discover() {
            Assembly serviceAssembly = Assembly.GetExecutingAssembly();
            string serviceAssemblyLocation = serviceAssembly.Location;
            string serviceAssemblyName = serviceAssembly.GetName().Name;
            IEnumerable<string> assemblyPaths = Directory.GetFiles(serviceAssemblyLocation.Replace($"{serviceAssemblyName}.dll", string.Empty)).Where(w => w.EndsWith("dll"));
            foreach (string assemblyPath in assemblyPaths) {
                Assembly assembly = Assembly.LoadFile(assemblyPath);
                IEnumerable<Type> moduleTypes = assembly.GetTypes().Where(w => typeof(SandboxModule).IsAssignableFrom(w));
                foreach (Type moduleType in moduleTypes) {
                    if (moduleType == typeof(SandboxModule))
                        continue;

                    modules.Add((SandboxModule)Activator.CreateInstance(moduleType));
                }
            }
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