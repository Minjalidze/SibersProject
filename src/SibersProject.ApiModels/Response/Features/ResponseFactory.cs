using SibersProject.ApiModels.Response.Implemintations;

namespace SibersProject.ApiModels.Response.Features
{
    /// <summary>
    /// This static helper class provides methods for creating different types of response objects.
    /// </summary>
    /// <typeparam name="T">The generic type representing the data in the response.</typeparam>
    public static class ResponseFactory<T>
    {
        /// <summary>
        /// Creates a success response object with the provided data model.
        /// </summary>
        /// <param name="model">The data model to be included in the response.</param>
        /// <returns>A success response object of type 'BaseResponse&lt;T&gt;'.</returns>
        public static BaseResponse<T> CreateSuccessResponse(T model)
        {
            return new BaseResponse<T>
            {
                IsSuccess = true,
                Data = model,
                StatusCode = 200,
            };
        }

        /// <summary>
        /// Creates a response object indicating that no records were found in the database.
        /// </summary>
        /// <param name="exception">The exception that occurred while searching for records.</param>
        /// <returns>A response object of type 'BaseResponse&lt;T&gt;' with a status code of 204 (No Content).</returns>
        public static BaseResponse<T> CreateNotFoundResponse(Exception exception)
        {
            return new BaseResponse<T>()
            {
                StatusCode = 204,
                IsSuccess = false,
                Message = $"No records found in the database. Error: {exception}"
            };
        }

        /// <summary>
        /// Creates an error response object indicating an internal server error.
        /// </summary>
        /// <param name="exception">The exception that occurred causing the error.</param>
        /// <returns>A response object of type 'BaseResponse&lt;T&gt;' with a status code of 500 (Internal Server Error).</returns>
        public static BaseResponse<T> CreateErrorResponse(Exception exception)
        {
            return new BaseResponse<T>()
            {
                StatusCode = 500,
                IsSuccess = false,
                Message = $"Internal server error: {exception}"
            };
        }

        /// <summary>
        /// Creates an error response object indicating an invalid operation.
        /// </summary>
        /// <param name="exception">The exception that occurred causing the invalid operation.</param>
        /// <returns>A response object of type 'BaseResponse&lt;T&gt;' with a status code of 400 (Bad Request).</returns>
        public static BaseResponse<T> CreateInvalidOperationResponse(Exception exception)
        {
            return new BaseResponse<T>()
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = $"Unable to create the specified model. Error: {exception}"
            };
        }

        /// <summary>
        /// Creates an error response object indicating a failed authorization attempt.
        /// </summary>
        /// <param name="exception">The exception that occurred during the authorization attempt.</param>
        /// <returns>A response object of type 'BaseResponse&lt;T&gt;' with a status code of 401 (Unauthorized).</returns>
        public static BaseResponse<T> CreateUnauthorizedResponse(Exception exception)
        {
            return new BaseResponse<T>()
            {
                StatusCode = 401,
                IsSuccess = false,
                Message = $"Failed authorization attempt. Error: {exception}"
            };
        }
    }
}
