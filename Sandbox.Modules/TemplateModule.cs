using Sandbox.Core;
using System;

namespace $rootnamespace$ {
    [SandboxModule("Module Name", "moduleKey", "Module description.")]
    public class $safeitemname$ : SandboxModule {

        #region Parameters

        [ModuleParameter("ExampleDisplayName", "This parameter is an example.", DisplayElement.Textbox, Required = true)]
        public string ExampleParameter { get; set; }

        #endregion

        #region Module Implementation

        /// <summary>
        /// Executes the $safeitemname$ module.
        /// </summary>
        protected override void Execute() {
            throw new NotImplementedException();
        }
        
        #endregion
    
    }
}