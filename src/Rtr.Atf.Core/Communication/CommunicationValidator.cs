using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// A service that provides tools for communication validation.
    /// </summary>
    public class CommunicationValidator
    {
        /// <summary>
        /// Stores communication transaction to be validated.
        /// </summary>
        private readonly CommunicationObject communicationObject;

        /// <summary>
        /// A logging service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationValidator"/> class.
        /// </summary>
        /// <param name="communicationObject">An object that stores communication transactions
        /// to be validated.</param>
        /// <param name="logger">A logging service.</param>
        internal CommunicationValidator(CommunicationObject communicationObject, ILogger logger)
        {
            this.communicationObject = communicationObject;
            this.logger = logger;
        }

        /// <summary>
        /// Verifies that given command was sent for given device register and value.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <typeparam name="TV">A type of value parameter.</typeparam>
        /// <param name="command">A command number to validate.</param>
        /// <param name="register">A device register to validate.</param>
        /// <param name="value">A value to validate.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public bool RegisterCommandSent<TR, TV>(long command, TR register, TV value)
        {
            return this.RegisterCommandSent(new List<long> { command }, register, value);
        }

        /// <summary>
        /// Verifies that at least one command from a given command list was sent for
        /// given device register and value.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <typeparam name="TV">A type of value parameter.</typeparam>
        /// <param name="commands">A list of commands to validate.</param>
        /// <param name="register">A device register to validate.</param>
        /// <param name="value">A value to validate.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public bool RegisterCommandSent<TR, TV>(IEnumerable<long> commands, TR register, TV value)
        {
            this.logger.Info($"{nameof(CommunicationValidator)}: Validate reg command sent: cmd range = ( {string.Join(", ", commands)} ), register = {register}, value = {value}");

            var lines = this.FilterByCommand(this.communicationObject.Transactions, commands);

            var regs = lines
                .Where(line => line.Rego?.Regs != null)
                .SelectMany(line => line.Rego?.Regs);

            regs = this.FilterByRegister(regs, register);
            regs = this.FilterByValue(regs, value);
            return regs.Any();
        }

        /// <summary>
        /// Gets register values according to register number and <see cref="CommunicationCommands"/> specified.
        /// </summary>
        /// <typeparam name="TR">A type of register parameter.</typeparam>
        /// <param name="commands">A list of commands.</param>
        /// <param name="register">A device register.</param>
        /// <returns>A collection of <see cref="Reg"/> objects.</returns>
        public IEnumerable<Reg> GetRegisterValue<TR>(IEnumerable<long> commands, TR register)
        {
            this.logger.Info($"{nameof(CommunicationValidator)}: Getting register values: cmd range = ( {string.Join(", ", commands)} ), register = {register}");

            var lines = this.FilterByCommand(this.communicationObject.Transactions, commands);

            var regs = lines
                        .Where(line => line.Rego?.Regs != null)
                        .SelectMany(line => line.Rego?.Regs);
            regs = this.FilterByRegister(regs, register);
            return regs;
        }

        /// <summary>
        /// Verifies that at least one command from a given command list was sent for
        /// given parameter and bitstring value.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <param name="commands">A list of commands to validate.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="expectedSet">Expected checkbox state.</param>
        /// <param name="strict">Whether a strict or loose comparison is required. Use strict for string values and loose for bitfields.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public bool RegisterCommandSent<TR>(IEnumerable<long> commands, TR register, string value, bool expectedSet, bool strict = true)
        {
            this.logger.Info($"{nameof(CommunicationValidator)}: Validate reg command sent: cmd range = ( {string.Join(", ", commands)} ), register = {register}, value = {value}, strict = {strict}");

            var lines = this.FilterByCommand(this.communicationObject.Transactions, commands);

            var regs = lines
                .Where(line => line.Rego?.Regs != null)
                .SelectMany(line => line.Rego?.Regs);

            regs = this.FilterByRegister(regs, register);
            if (expectedSet)
            {
                regs = this.FilterByValue(regs, value, strict);
                return regs.Any();
            }

            return regs.Any() && !this.FilterByValue(regs, value, strict).Any();
        }

        /// <summary>
        /// Verifies that at least one command from a given command list was sent for
        /// given parameter and floating-point value in defined tolerance.
        /// </summary>
        /// <typeparam name="TR">Data type register is represented.</typeparam>
        /// <param name="commands">A list of commands to validate.</param>
        /// <param name="register">Device register to validate.</param>
        /// <param name="value">Value sent to device.</param>
        /// <param name="tolerance">Maximum defference between expected and actual floating-point values.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public bool RegisterCommandSent<TR>(IEnumerable<long> commands, TR register, float value, float tolerance = 0.0001f)
        {
            this.logger.Info($"{nameof(CommunicationValidator)}: Validate reg command sent: cmd range = ( {string.Join(", ", commands)} ), register = {register}, value = {value}, tolerance = {tolerance}");

            var lines = this.FilterByCommand(this.communicationObject.Transactions, commands);

            var regs = lines
                .Where(line => line.Rego?.Regs != null)
                .SelectMany(line => line.Rego?.Regs);

            regs = this.FilterByRegister(regs, register);
            regs = this.FilterByValue(regs, value, tolerance);
            return regs.Any();
        }

        /// <summary>
        /// Verifies that at least one command from a given command list was sent for
        /// given set of combinations of parameter indexes and values.
        /// </summary>
        /// <param name="commands">A list of commands to validate.</param>
        /// <param name="parameters">Set pairs of indexes and values to validate.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public bool CommandSent(IEnumerable<long> commands, params (int index, object value)[] parameters)
        {
            var paramStrings = new List<string>();
            foreach (var (index, value) in parameters)
            {
                paramStrings.Add($"[{index}, {value}]");
            }

            this.logger.Info($"{nameof(CommunicationValidator)}: Validate reg command sent: cmd range = ( {string.Join(", ", commands)} ), parameters = ( {paramStrings} )");

            IEnumerable<Transaction> lines = this.FilterByCommand(this.communicationObject.Transactions, commands)
                .Where(l => l.Mapo != null)
                .Where(l => l.Mapo.ParamDict.Any());

            foreach (var (index, value) in parameters)
            {
                lines = lines
                .Where(l => l.Mapo.ParamDict[index].ValNum == value.ToString());
            }

            return lines.Any();
        }

        /// <summary>
        /// Verifies that given command was sent for given set of combinations
        /// of parameter indexes and values.
        /// </summary>
        /// <param name="command">A command to validate.</param>
        /// <param name="parameters">Set pairs of indexes and values to validate.</param>
        /// <returns>A value indicating whether command was sent or not.</returns>
        public bool CommandSent(int command, params (int index, object value)[] parameters)
        {
            return this.CommandSent(new List<long> { command }, parameters);
        }

        /// <summary>
        /// Filters transactions by command numbers.
        /// </summary>
        /// <param name="transactions">Transactions to filter.</param>
        /// <param name="commands">Commnands that bypass filter.</param>
        /// <returns>Transactions passed a filter.</returns>
        private IEnumerable<Transaction> FilterByCommand(IEnumerable<Transaction> transactions, IEnumerable<long> commands)
        {
            return transactions.Where(l => commands.Contains(l.Cmd));
        }

        /// <summary>
        /// Filters registers by name or number.
        /// </summary>
        /// <typeparam name="T">A type of register parameter.</typeparam>
        /// <param name="regs">Registers to filter.</param>
        /// <param name="register">A register name or number.</param>
        /// <returns>Registers passed a filter.</returns>
        private IEnumerable<Reg> FilterByRegister<T>(IEnumerable<Reg> regs, T register)
        {
            if (typeof(T) == typeof(string))
            {
                return regs.Where(r => r.Lname == register.ToString());
            }
            else
            {
                return regs.Where(r => r.RegNum == Convert.ToInt64(register));
            }
        }

        /// <summary>
        /// Filters registers by floating-point value with tolerance.
        /// </summary>
        /// <typeparam name="T">A type of register parameter.</typeparam>
        /// <param name="regs">Registers to filter.</param>
        /// <param name="value">A register value to filter.</param>
        /// <param name="tolerance">Maximum defference between expected and actual floating-point values.</param>
        /// <returns>Registers passed a filter.</returns>
        private IEnumerable<Reg> FilterByValue(IEnumerable<Reg> regs, float value, float tolerance = 0.0001f)
        {
            var culture = new CultureInfo("en-US"); // todo pass settings parameter
            return regs
                .Where(r => r.Type.ToUpper() == "FLOAT")
                .Where(r =>
                {
                    var v1 = Convert.ToSingle(r.ValNum, culture);
                    var v2 = Convert.ToSingle(value, culture);
                    return v1.NearlyEquals(v2, tolerance);
                });
        }

        /// <summary>
        /// Filters registers by bitstring value.
        /// </summary>
        /// <typeparam name="T">A type of register parameter.</typeparam>
        /// <param name="regs">Registers to filter.</param>
        /// <param name="value">A register value to filter.</param>
        /// <param name="strict">Whether a strict or loose comparison is required. Use strict for string values and loose for bitfields.</param>
        /// <returns>Registers passed a filter.</returns>
        private IEnumerable<Reg> FilterByValue(IEnumerable<Reg> regs, string value, bool strict = true)
        {
            if (strict)
            {
                return regs.Where(r => r.ValStr == value.ToString());
            }
            else
            {
                return regs.Where(r => r.ValStr.Contains(value.ToString()));
            }
        }

        /// <summary>
        /// Filters registers by value.
        /// </summary>
        /// <typeparam name="T">A type of register parameter.</typeparam>
        /// <param name="regs">Registers to filter.</param>
        /// <param name="value">A register value to filter.</param>
        /// <returns>Registers passed a filter.</returns>
        private IEnumerable<Reg> FilterByValue<T>(IEnumerable<Reg> regs, T value)
        {
            if (typeof(T) == typeof(float))
            {
                var culture = new CultureInfo("en-US"); // todo pass settings parameter
                return regs
                    .Where(r => r.Type.ToUpper() == "FLOAT" &&
                    Convert.ToSingle(r.ValNum, culture) == Convert.ToSingle(value, culture));
            }
            else if (value is string)
            {
                return regs.Where(r => r.ValStr == value.ToString());
            }
            else if (long.TryParse(value.ToString(), out var v))
            {
                return regs
                    .Where(r => (r.Type.ToUpper() == "DWORD" || r.Type.ToUpper() == "ENUM") &&
                    Convert.ToInt64(r.ValNum) == Convert.ToInt64(v));
            }

            var e = new ArgumentException($"Unknown type of parameter {nameof(value)}, actual type is {typeof(T).FullName}");
            this.logger.Error(e);
            throw e;
        }
    }
}
