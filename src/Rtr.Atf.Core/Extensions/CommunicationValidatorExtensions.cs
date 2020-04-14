using System.Collections.Generic;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Extension methods for <see cref="CommunicationValidator"/> class.
    /// </summary>
    public static class CommunicationValidatorExtensions
    {
        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.WriteHreg"/> was sent for
        /// given register and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="tolerance">Maximum defference between expected and actual floating-point values.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool WriteHregSent<TR>(this CommunicationValidator validator, TR register, float value, float tolerance = 0.0001f)
        {
            return validator.RegisterCommandSent(CommunicationCommands.WriteHreg, register, value, tolerance);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.ReadHreg"/> was received for
        /// given register and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="tolerance">Maximum defference between expected and actual floating-point values.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool ReadHregSent<TR>(this CommunicationValidator validator, TR register, float value, float tolerance = 0.0001f)
        {
            return validator.RegisterCommandSent(CommunicationCommands.ReadHreg, register, value, tolerance);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.ReadIreg"/> was received for
        /// given register and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="tolerance">Maximum defference between expected and actual floating-point values.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool ReadIregSent<TR>(this CommunicationValidator validator, TR register, float value, float tolerance = 0.0001f)
        {
            return validator.RegisterCommandSent(CommunicationCommands.ReadIreg, register, value, tolerance);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.WriteHreg"/> was sent for
        /// given register and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="expectedSet">Expected checkbox state.</param>
        /// <param name="strict">Whether a strict or loose comparison is required. Use strict for string values and loose for bitfields.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool WriteHregSent<TR>(this CommunicationValidator validator, TR register, string value, bool expectedSet, bool strict = true)
        {
            return validator.RegisterCommandSent(CommunicationCommands.WriteHreg, register, value, expectedSet, strict);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.ReadHreg"/> was received for
        /// given register and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="expectedSet">Expected checkbox state.</param>
        /// <param name="strict">Whether a strict or loose comparison is required. Use strict for string values and loose for bitfields.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool ReadHregSent<TR>(this CommunicationValidator validator, TR register, string value, bool expectedSet, bool strict = true)
        {
            return validator.RegisterCommandSent(CommunicationCommands.ReadHreg, register, value, expectedSet, strict);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.ReadIreg"/> was received for
        /// given register and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="expectedSet">Expected checkbox state.</param>
        /// <param name="strict">Whether a strict or loose comparison is required. Use strict for string values and loose for bitfields.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool ReadIregSent<TR>(this CommunicationValidator validator, TR register, string value, bool expectedSet, bool strict = true)
        {
            return validator.RegisterCommandSent(CommunicationCommands.ReadIreg, register, value, expectedSet, strict);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.WriteHreg"/> was sent for
        /// given register and value.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <typeparam name="TV">Data type value is represented.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool WriteHregSent<TR, TV>(this CommunicationValidator validator, TR register, TV value)
        {
            return validator.RegisterCommandSent(CommunicationCommands.WriteHreg, register, value);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.ReadHreg"/> was received for
        /// given register and value.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <typeparam name="TV">Data type value is represented.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool ReadHregSent<TR, TV>(this CommunicationValidator validator, TR register, TV value)
        {
            return validator.RegisterCommandSent(CommunicationCommands.ReadHreg, register, value);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.ReadIreg"/> was received for
        /// given register and value.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <typeparam name="TV">Data type value is represented.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool ReadIregSent<TR, TV>(this CommunicationValidator validator, TR register, TV value)
        {
            return validator.RegisterCommandSent(CommunicationCommands.ReadIreg, register, value);
        }

        /// <summary>
        /// Verifies that command <see cref="CommunicationCommands.WriteDynamicVariable"/> for
        /// Primary Variable was sent with given value.
        /// </summary>
        /// <param name="validator">Validator instance.</param>
        /// <param name="value">Value sent to device.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public static bool SetPVSent(this CommunicationValidator validator, int value)
        {
            return validator.CommandSent(CommunicationCommands.WriteDynamicVariable, (0, value));
        }

        /// <summary>
        /// Gets values of the specified register number for <see cref="CommunicationCommands.ReadIreg"/> commands.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register.</param>
        /// <returns>A collection of <see cref="Reg"/> objects.</returns>
        public static IEnumerable<Reg> GetReadIregRegisterValues<TR>(this CommunicationValidator validator, TR register)
        {
            return validator.GetRegisterValue<TR>(CommunicationCommands.ReadIreg, register);
        }

        /// <summary>
        /// Gets values of the specified register number for <see cref="CommunicationCommands.ReadHreg"/> commands.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register.</param>
        /// <returns>A collection of <see cref="Reg"/> objects.</returns>
        public static IEnumerable<Reg> GetReadHregRegisterValues<TR>(this CommunicationValidator validator, TR register)
        {
            return validator.GetRegisterValue<TR>(CommunicationCommands.ReadHreg, register);
        }

        /// <summary>
        /// Gets values of the specified register number for <see cref="CommunicationCommands.WriteHreg"/> commands.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <param name="validator">Validator instance.</param>
        /// <param name="register">Device register.</param>
        /// <returns>A collection of <see cref="Reg"/> objects.</returns>
        public static IEnumerable<Reg> GetWriteHregRegisterValues<TR>(this CommunicationValidator validator, TR register)
        {
            return validator.GetRegisterValue<TR>(CommunicationCommands.WriteHreg, register);
        }
    }
}
