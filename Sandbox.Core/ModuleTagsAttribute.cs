using System;
using System.Collections.Generic;

namespace Sandbox.Core {
    public class ModuleTagsAttribute : Attribute {

        #region Fields

        private List<string> values = new List<string>();

        #endregion

        #region Properties

        public string[] Values => values.ToArray();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new module tag collection with the specified tags. Up to five may be supplied.
        /// </summary>
        /// <param name="tags">The tags associated with a module..</param>
        /// <remarks>
        /// Currently, this method prevents more than 5 tags from ever being added, by design.
        /// </remarks>
        public ModuleTagsAttribute(params string[] tags) {
            foreach (string tag in tags) {
                if (values.Count >= 5)
                    break;
                if (!string.IsNullOrWhiteSpace(tag))
                    values.Add(tag);
            }
        }

        #endregion

    }
}