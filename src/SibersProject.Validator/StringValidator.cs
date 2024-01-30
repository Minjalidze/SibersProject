namespace SibersProject.Validator
{

    /// <summary>
    /// String validation.
    /// </summary>
    public static class StringValidator
    {
        /// <summary>
        /// Checks if the string is not null or empty.
        /// </summary>
        /// <param name="text">The string to be checked.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CheckIsNotNull(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text), "The parameter must not be Nullы");
            }
        }
    }
    
}
