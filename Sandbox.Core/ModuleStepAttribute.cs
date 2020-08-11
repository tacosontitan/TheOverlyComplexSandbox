using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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