using SibersProject.ApiModels.Auth.Models;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.MainDomain.Models.Abstractions.BaseUsers;

namespace SibersProject.Services.Services.Interfaces
{
    /// <summary>
    /// Represents an authentication manager for a specific model.
    /// </summary>
    /// <typeparam name="TModel">The type of the model, which should be a subtype of ApplicationUser.</typeparam>
    public interface IAuthManager<TModel> where TModel : ApplicationUser
    {
        /// <summary>
        /// Logs in a user with the provided login model.
        /// </summary>
        /// <param name="model">The login model containing the user's credentials.</param>
        /// <returns>A task that represents the asynchronous operation and contains an <see cref="IBaseResponse{T}"/> where T is an <see cref="AuthResultStruct"/> indicating the authentication result.</returns>
        Task<IBaseResponse<AuthResultStruct>> Login(LoginModel model);

        /// <summary>
        /// Registers a new user with the provided registration model.
        /// </summary>
        /// <param name="model">The registration model containing the user's information.</param>
        /// <returns>A task that represents the asynchronous operation and contains an <see cref="IBaseResponse{T}"/> where T is a string id indicating the success or failure of the registration process.</returns>
        Task<IBaseResponse<string>> Register(RegisterModel model);

        /// <summary>
        /// Registers a new administrator with the provided registration model.
        /// </summary>
        /// <param name="model">The registration model containing the administrator's information.</param>
        /// <returns>A task that represents the asynchronous operation and contains an <see cref="IBaseResponse{T}"/> where T is a string id indicating the success or failure of the registration process.</returns>
        Task<IBaseResponse<string>> RegisterAdmin(RegisterModel model);

        /// <summary>
        /// Refreshes an access token using the provided token model.
        /// </summary>
        /// <param name="tokenModel">The token model containing the refresh token.</param>
        /// <returns>A task that represents the asynchronous operation and contains an <see cref="IBaseResponse{T}"/> where T is an <see cref="AuthResultStruct"/> indicating the authentication result.</returns>
        Task<IBaseResponse<AuthResultStruct>> RefreshToken(TokenModel tokenModel);

        /// <summary>
        /// Revokes the refresh token for a user with the specified username.
        /// </summary>
        /// <param name="username">The username of the user for whom to revoke the refresh token.</param>
        /// <returns>A task that represents the asynchronous operation and contains an <see cref="IBaseResponse{T}"/> where T is a boolean indicating the success or failure of the token revocation.</returns>
        Task<IBaseResponse<bool>> RevokeRefreshTokenByUsernameAsync(string username);

        /// <summary>
        /// Revokes all refresh tokens.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation and contains an <see cref="IBaseResponse{T}"/> where T is a boolean indicating the success or failure of the token revocation.</returns>
        Task<IBaseResponse<bool>> RevokeAllRefreshTokensAsync();
    }
}
