using System;

namespace Sandbox.Core {
    public class SandboxModuleAttribute : Attribute {

        #region Properties

        public string Name { get; private set; } = string.Empty;
        public string ExecutionKey { get; private set; } = string.Empty;
        public string Description { get; set; }

        #endregion

        #region Constructors

        public SandboxModuleAttribute(string name, string executionKey, string description) {
            Name = name;
            ExecutionKey = executionKey;
            Description = description;
        }

        #endregion

    }
}