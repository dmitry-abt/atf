using System.Collections.Generic;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/> interface.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Checks if enumeration contains single element.
        /// </summary>
        /// <typeparam name="T">Type of elements in enumeration.</typeparam>
        /// <param name="enumerable">Enumeration to check.</param>
        /// <returns>Value indicating whether enumeration has single element or not.</returns>
        public static bool HasOne<T>(this IEnumerable<T> enumerable)
        {
            var enumerator = enumerable.GetEnumerator();
            return enumerator.MoveNext() && !enumerator.MoveNext();
        }
    }
}
