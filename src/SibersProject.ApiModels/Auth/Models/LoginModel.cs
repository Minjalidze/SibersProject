using System.ComponentModel.DataAnnotations;

namespace SibersProject.ApiModels.Auth.Models
{
    /// <summary>
    /// Model class representing the login information for a user.
    /// It includes properties for the username and password.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the user name.
        /// Nullable property.
        /// </summary>
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// Nullable property.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
