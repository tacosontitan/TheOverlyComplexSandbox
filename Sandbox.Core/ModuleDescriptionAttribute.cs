﻿using System;

namespace Sandbox.Core {
    public class ModuleDescriptionAttribute : Attribute {

        #region Properties

        public string Name { get; private set; } = string.Empty;
        public string ExecutionKey { get; private set; } = string.Empty;
        public string Description { get; set; }

        #endregion

        #region Constructors

        public ModuleDescriptionAttribute(string name, string executionKey, string description) {
            Name = name;
            ExecutionKey = executionKey;
            Description = description;
        }

        #endregion

    }
}