using Microsoft.AspNetCore.Identity;

namespace SibersProject.MainDomain.Models.Abstractions.BaseUsers
{
    /// <summary>
    /// Custom application user class derived from IdentityUser.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
