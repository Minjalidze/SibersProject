using Microsoft.AspNetCore.Identity;
namespace SibersProject.MainDomain.Models.Abstractions.BaseUsers
{
    /// <summary>
    /// Custom application role class derived from IdentityRole.
    /// </summary>
    public class ApplicationRole : IdentityRole<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        public ApplicationRole() : base()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class with the specified role name.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        public ApplicationRole(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
