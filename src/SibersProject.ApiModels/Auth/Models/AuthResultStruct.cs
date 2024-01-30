namespace SibersProject.ApiModels.Auth.Models
{
    /// <summary>
    /// Struct representing the result of an authentication operation.
    /// It includes properties for the token, refresh token, and expiration date.
    /// </summary>
    public struct AuthResultStruct
    {
        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the token.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}
