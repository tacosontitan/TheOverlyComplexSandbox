namespace Sandbox.Core {
    /// <summary>
    /// Supported types of display elements.
    /// </summary>
    /// <remarks>
    /// The thought of this needing to be reworked as an object instead of an enum is worth looking into.
    ///     For now, enum has the fastest implementation time.
    /// </remarks>
    public enum DisplayElement {
        /// <summary>
        /// Display as a checkbox.
        /// </summary>
        Checkbox,
        /// <summary>
        /// Display as a textbox.
        /// </summary>
        Textbox,
        /// <summary>
        /// Display as a rich textbox.
        /// </summary>
        RichTextbox,
        /// <summary>
        /// Displays as a numeric range slider.
        /// </summary>
        Slider
    }
}
