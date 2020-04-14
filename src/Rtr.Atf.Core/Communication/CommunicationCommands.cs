using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides a collection of known device commands.
    /// </summary>
    internal static class CommunicationCommands
    {
        /// <summary>
        /// Gets a collection of known commands for writing value into Holding register.
        /// </summary>
        internal static IReadOnlyCollection<long> WriteHreg => new ReadOnlyCollection<long>(new List<long>
        {
            212,
            216,
        });

        /// <summary>
        /// Gets a collection of known commands for reading value from Holding register.
        /// </summary>
        internal static IReadOnlyCollection<long> ReadHreg => new ReadOnlyCollection<long>(new List<long>
        {
            211,
            215,
        });

        /// <summary>
        /// Gets a collection of known commands for reading value from Input register.
        /// </summary>
        internal static IReadOnlyCollection<long> ReadIreg => new ReadOnlyCollection<long>(new List<long>
        {
            210,
            214,
        });

        /// <summary>
        /// Gets a collection of known commands for writing value to dynamic variable.
        /// </summary>
        internal static IReadOnlyCollection<long> WriteDynamicVariable => new ReadOnlyCollection<long>(new List<long>
        {
            51,
        });
    }
}
