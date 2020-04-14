namespace Rtr.Atf.Core
{
    /// <summary>
    /// Represents info about position of item on screen.
    /// </summary>
    public interface IScreenCoordinates
    {
        /// <summary>
        /// Gets X coordinate.
        /// </summary>
        int X { get; }

        /// <summary>
        /// Gets Y coordinate.
        /// </summary>
        int Y { get; }
    }
}
