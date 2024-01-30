namespace SibersProject.ApiModels.Auth.Models
{
    /// <summary>
    /// Model class representing a token.
    /// It includes properties for the access token and the refresh token.
    /// </summary>
    public class TokenModel
    {
        /// <summary>
        /// Gets or sets the access token.
        /// Nullable property.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// Nullable property.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}

