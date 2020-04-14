namespace Rtr.Atf.Core
{
    /// <summary>
    /// Represents info about width and height of an element on screen.
    /// </summary>
    public interface IElementDimensions
    {
        /// <summary>
        /// Gets width of an element.
        /// </summary>
        double Width { get; }

        /// <summary>
        /// Gets height of an element.
        /// </summary>
        double Height { get; }
    }
}
