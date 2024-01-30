using System.ComponentModel.DataAnnotations;

namespace SibersProject.ApiModels.Auth.Models
{
    /// <summary>
    /// Model class representing the registration information for a user.
    /// It includes properties for the user name, email, and password.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the user name.
        /// Nullable property.
        /// </summary>
        [Required(ErrorMessage = "User Name is required")]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// Nullable property.
        /// </summary>
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// Nullable property.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
