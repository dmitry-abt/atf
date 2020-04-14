namespace Rtr.Atf.Core
{
    /// <summary>
    /// An object that stores sniffed communication and provides validation tools.
    /// </summary>
    public interface ICommunicationLog
    {
        /// <summary>
        /// Gets deserialized raw communication log.
        /// </summary>
        CommunicationObject RawCommunication { get; }

        /// <summary>
        /// Gets a tool for communication validation.
        /// </summary>
        CommunicationValidator Validate { get; }
    }
}
