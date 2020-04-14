using System;

namespace Rtr.Atf.Core
{
    /// <summary>
    /// Extension methods for <see cref="float"/> structure.
    /// </summary>
    public static class SingleExtensions
    {
        /// <summary>
        /// Compares two floating-point values in defined tolerance.
        /// </summary>
        /// <param name="value1">Fisrt value for comparing.</param>
        /// <param name="value2">Second value for comparing.</param>
        /// <param name="tolerance">
        /// the maximum difference between the numbers
        /// to which these numbers will be considered equal.
        /// </param>
        /// <returns>A value indication whether two floating-point values are equal
        /// in defined tolerance.</returns>
        public static bool NearlyEquals(this float value1, float? value2, float tolerance)
        {
            if (value1 != value2)
            {
                if (value2 == null)
                {
                    return false;
                }

                return Math.Abs(value1 - value2.Value) < tolerance;
            }

            return true;
        }
    }
}
