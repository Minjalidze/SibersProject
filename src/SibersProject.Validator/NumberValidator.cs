namespace SibersProject.Validator
{
    public static class NumberValidator<T> where T : struct, IComparable, IConvertible
    {
        /// <summary>
        /// Checks if the value is not zero.
        /// </summary>
        /// <param name="n">The value to be checked.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsNotZero(T n)
        {
            if (n.CompareTo(default(T)) == 0)
            {
                throw new ArgumentException("The value must not be zero.", nameof(n));
            }
        }

        /// <summary>
        /// Checks if the value is positive.
        /// </summary>
        /// <param name="n">The value to be checked.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsPositive(T n)
        {
            if (n.CompareTo(default(T)) <= 0)
            {
                throw new ArgumentException("The value must be positive.", nameof(n));
            }
        }

        /// <summary>
        /// Checks if the value is negative.
        /// </summary>
        /// <param name="n">The value to be checked.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsNegative(T n)
        {
            if (n.CompareTo(default(T)) >= 0)
            {
                throw new ArgumentException("The value must be negative.", nameof(n));
            }
        }

        /// <summary>
        /// Checks if the value is odd.
        /// </summary>
        /// <param name="n">The value to be checked.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsOdd(T n)
        {
            if (Convert.ToInt32(n) % 2 == 0)
            {
                throw new ArgumentException("The value must be odd.", nameof(n));
            }
        }

        /// <summary>
        /// Checks if the value is even.
        /// </summary>
        /// <param name="n">The value to be checked.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsEven(T n)
        {
            if (Convert.ToInt32(n) % 2 != 0)
            {
                throw new ArgumentException("The value must be even.", nameof(n));
            }
        }

        /// <summary>
        /// Checks if the value is within the specified range.
        /// </summary>
        /// <param name="n">The value to be checked.</param>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        /// <exception cref="ArgumentException"></exception>
        public static void IsRange(T n, T minValue, T maxValue)
        {
            if (n.CompareTo(minValue) < 0 || n.CompareTo(maxValue) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n,
                    $"The value must be in the range from {minValue} to {maxValue}.");
            }
        }
    }
    
}
