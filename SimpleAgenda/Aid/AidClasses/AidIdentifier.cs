
namespace SimpleAgenda.Aid.AidClasses
{
    internal static class AidIdentifier
    {
        /// <summary>
        /// Generates a random number with the specified number of digits.
        /// If the number of digits is less than 1 (one), it returns a default value of 4 (four) digits.
        /// </summary>
        /// <param name="length">The desired number of digits for the random number.</param>
        /// <returns>A random number with the specified number of digits.</returns>
        internal static int RandomIntId(int length)
        {
            if (length <= 0) length = 4;

            return Random.Shared.Next((int)Math.Pow(10, length - 1), (int)Math.Pow(10, length) - 1);
        }
    }
}
