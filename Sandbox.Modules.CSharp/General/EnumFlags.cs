using Sandbox.Core;
using System;

namespace Sandbox.Modules.General {
    /// <summary>
    /// The test enum to demonstrate the use of bit flags.
    /// </summary>
    [Flags]
    public enum Test {
        None = 0b0,
        One = 0b1,
        Two = 0b10,
        Three = 0b100,
        Four = 0b1000,
        Five = 0b10000
    }
    [SandboxModule("Enum Flag Discovery", "cs-enum-flags", "No description available currently.")]
    public class EnumFlags : SandboxModule {

        #region Parameters

        [ModuleParameter("Flag One", "Should flag one be added to the enum?", DisplayElement.Checkbox, Required = true)]
        public bool FlagOne { get; set; }
        [ModuleParameter("Flag Two", "Should flag two be added to the enum?", DisplayElement.Checkbox, Required = true)]
        public bool FlagTwo { get; set; }
        [ModuleParameter("Flag Three", "Should flag three be added to the enum?", DisplayElement.Checkbox, Required = true)]
        public bool FlagThree { get; set; }
        [ModuleParameter("Flag Four", "Should flag four be added to the enum?", DisplayElement.Checkbox, Required = true)]
        public bool FlagFour { get; set; }
        [ModuleParameter("Flag Five", "Should flag five be added to the enum?", DisplayElement.Checkbox, Required = true)]
        public bool FlagFive { get; set; }

        #endregion

        #region Module Implementation

        /// <summary>
        /// Executes the EnumFlags module.
        /// </summary>
        protected override void Execute() {
            Test testEnum = Test.None;
            if (FlagOne)
                testEnum |= Test.One;
            if (FlagTwo)
                testEnum |= Test.Two;
            if (FlagThree)
                testEnum |= Test.Three;
            if (FlagFour)
                testEnum |= Test.Four;
            if (FlagFive)
                testEnum |= Test.Five;

            // Count the number of flags the variable has.
            SendResponse(SandboxEventType.Information, $"The test enum value has {CountBits((int)(object)testEnum)} flags set.");

            // Check the variable for flags, and print any flags it may have.
            CheckEnumForFlags(testEnum);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Counts the number of bits equal to 1 in the supplied value.
        /// </summary>
        /// <param name="value">The 64 bit signed integer to count the bits in.</param>
        /// <returns>The number of bits equal to 1.</returns>
        private int CountBits(long value) {
            int count = 0;
            for (; value > 0; count++)
                value &= value - 1;

            return count;
        }
        /// <summary>
        /// Checks the supplied enum value for flags and prints any flags it contains.
        /// </summary>
        private void CheckEnumForFlags(Enum value) {
            string[] enumNames = Enum.GetNames(value.GetType());
            foreach (string enumName in enumNames) {
                Enum enumValue = (Enum)Enum.Parse(value.GetType(), enumName);
                if (value.HasFlag(enumValue))
                    SendResponse(SandboxEventType.Information, $"The supplied value contains a flag for {enumName}.");
            }
        }

        #endregion

    }
}