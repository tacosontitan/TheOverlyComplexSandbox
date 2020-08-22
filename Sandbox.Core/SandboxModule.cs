using System;
using System.Reflection;

namespace Sandbox.Core {
    public abstract class SandboxModule {

        #region Properties

        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Language { get; private set; }
        public string FriendlyLanguage { get; private set; }
        public string Category { get; private set; }
        public string[] Tags { get; private set; }
        public string ExecutionKey { get; private set; }
        public string Description { get; private set; }
        public string ShortDescription {
            get {
                string description = Description.Substring(0, Math.Min(Description.Length, 100));
                if (Description.Length > 100)
                    description += "...";

                return description;
            }
        }

        #endregion

        #region Constructors

        public SandboxModule() {
            // Get module description. If no description is present, module is invalid.
            SandboxModuleAttribute moduleDescription = GetType().GetCustomAttribute<SandboxModuleAttribute>();
            if (moduleDescription == null)
                throw new InvalidOperationException("All sandbox modules are required to have a description through the SandboxModuleAttribute object.");
            else {
                Name = moduleDescription.Name;
                ExecutionKey = moduleDescription.ExecutionKey;
                Description = moduleDescription.Description;
            }

            // Create this session's ID for this module.
            ID = Guid.NewGuid();

            // Check for module tags.
            ModuleTagsAttribute tags = GetType().GetCustomAttribute<ModuleTagsAttribute>();
            if (tags != null)
                Tags = tags.Values;

            // TODO: Implement a cleaner method to accomplish this.
            // There are better ways, this is just easy for the sake of implementation time.
            // However, it's prone to easy bugs.
            string nspace = GetType().Namespace.Replace("Sandbox.Modules.", string.Empty);
            string[] nspaceBreakdown = nspace.Split('.');
            Category = nspaceBreakdown[1];
            FriendlyLanguage = nspaceBreakdown[0].ToLowerInvariant();
            switch (FriendlyLanguage) {
                case "csharp": Language = "C#"; break;
                case "fsharp": Language = "F#"; break;
                case "visualbasic": Language = "VB"; break;
                case "python": Language = "PY"; break;
                case "cplusplus": Language = "C++"; break;
                case "javascript": Language = "JS"; break;
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