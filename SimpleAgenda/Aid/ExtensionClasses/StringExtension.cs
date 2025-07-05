
namespace SimpleAgenda.Aid.ExtensionClasses
{
    internal static class StringExtension
    {
        /// <summary>
        /// Validates if the provided string is neither null nor empty or white space.
        /// </summary>
        /// <param name="value">The string to be validated.</param>
        /// <returns>
        /// Returns the original string if it is not null or empty or white space.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided string is null, empty, or consists only of white-space characters.
        /// </exception>
        internal static string NullOrEmptyValidator(this string? value)
        {
            return !String.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("The provided value is null or empty.");
        }
    }
}
