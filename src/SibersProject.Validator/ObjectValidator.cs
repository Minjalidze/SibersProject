namespace SibersProject.Validator
{
    /// <summary>
    /// Object Validation.
    /// </summary>
    /// <typeparam name="T"> Тип данных. </typeparam>
    public static class ObjectValidator<T>
    {
        /// <summary>
        /// Checks if the object is not null.
        /// </summary>
        /// <param name="modelObject">The object to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when the object is null.</exception>
        public static void CheckIsNotNullObject(T modelObject)
        {
            if (modelObject == null)
            {
                throw new ArgumentNullException(nameof(modelObject), "The object must not be Null");
            }
        }
    }  
}
