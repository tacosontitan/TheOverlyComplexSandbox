using System;

namespace Sandbox.Core {
    /// <summary>
    /// Marks an object as a module parameter.
    /// </summary>
    public class ModuleParameterAttribute : Attribute {

        #region Properties

        /// <summary>
        /// Is this parameter required?
        /// </summary>
        /// <remarks>When required is set to true, the module is permitted to continue requesting this information until it is received or the user exits.</remarks>
        public bool Required { get; set; }
        /// <summary>
        /// The display name for this parameter.
        /// </summary>
        /// <remarks>This is used to fill placeholders.</remarks>
        public string DisplayName { get; set; }
        /// <summary>
        /// The message displayed to an end user to request this parameter.
        /// </summary>
        /// <example>Please supply a number between 1 and 10.</example>
        public string RequestMessage { get; set; }
        /// <summary>
        /// The type of element used to display this parameter in Windows Forms and Web.
        /// </summary>
        public DisplayElement DisplayElement { get; set; }
        /// <summary>
        /// The minimum acceptable value for this parameter. Only used if DisplayElement is set to Slider.
        /// </summary>
        public int MinValue { get; set; }
        /// <summary>
        /// The maximum acceptable value for this parameter. Only used if DisplayElement is set to Slider.
        /// </summary>
        public int MaxValue { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new module parameter with a specified display name and request message.
        /// </summary>
        /// <param name="displayName">The display name for this parameter.</param>
        /// <param name="requestMessage">The message displayed to an end user to request this parameter.</param>
        public ModuleParameterAttribute(string displayName, string requestMessage) {
            DisplayName = displayName;
            RequestMessage = requestMessage;
        }
        /// <summary>
        /// Create a new module parameter with a specified display name, request message, and display element.
        /// </summary>
        /// <param name="displayName">The display name for this parameter.</param>
        /// <param name="requestMessage">The message displayed to an end user to request this parameter.</param>
        /// <param name="displayElement">The type of element used to display this parameter in Windows Forms and Web.</param>
        public ModuleParameterAttribute(string displayName, string requestMessage, DisplayElement displayElement) : this(displayName, requestMessage) => DisplayElement = displayElement;

        #endregion

    }
}
