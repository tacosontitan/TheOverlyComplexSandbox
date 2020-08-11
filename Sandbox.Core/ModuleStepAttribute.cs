using System;

namespace Sandbox.Core {
    public class ModuleStepAttribute : Attribute {

        #region Properties

        public int StepNumber { get; private set; }

        #endregion

        #region Constructors

        public ModuleStepAttribute(int stepNumber) => StepNumber = stepNumber;

        #endregion

    }
}