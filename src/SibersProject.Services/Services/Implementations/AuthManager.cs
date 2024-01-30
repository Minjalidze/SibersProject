using SibersProject.ApiModels.Auth.Models;
using SibersProject.ApiModels.Response.Features;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.MainDomain.Models.Abstractions.BaseUsers;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Interfaces;
using SibersProject.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SibersProject.Services.Services.Implementations
{
    public class AuthManager<T> : IAuthManager<T>
         where T : ApplicationUser
    {
        private readonly UserManager<Employee> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthManager(UserManager<Employee> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        public async Task<IBaseResponse<AuthResultStruct>> Login(LoginModel model)
        {
            try
            {
                ObjectValidator<LoginModel>.CheckIsNotNullObject(model);
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    foreach (var userRole in userRoles)
                    {

                        if (userRole == Roles.Employee.ToString())
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, "Employee"));
                        }
                        if (userRole == Roles.Manager.ToString())
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, "Manager"));
                        }
                        if (userRole == Roles.Supervisor.ToString())
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, "Supervisor"));
                        }
                        if (userRole == Roles.Admin.ToString())
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        }

                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = CreateToken(authClaims);
                    var refreshToken = GenerateRefreshToken();

                    if (int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays))
                    {
                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                        await _userManager.UpdateAsync(user);
                        string userId = user.Id;

                        return ResponseFactory<AuthResultStruct>.CreateSuccessResponse(new AuthResultStruct
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            RefreshToken = refreshToken,
                            Expiration = token.ValidTo,
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid configuration for refresh token validity");
                    }
                }
                throw new UnauthorizedAccessException("Access denied. User is not authorized.");
            }
            catch (InvalidOperationException ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateUnauthorizedResponse(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateUnauthorizedResponse(ex);
            }
            catch (ArgumentException ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateErrorResponse(ex);
            }
        }


        public async Task<IBaseResponse<string>> Register(RegisterModel model)
        {
            try
            {
                ObjectValidator<RegisterModel>.CheckIsNotNullObject(model);

                var userExists = await _userManager.FindByNameAsync(model.UserName);
                if (userExists != null)
                {
                    throw new UnauthorizedAccessException(" Thsit User already exists! Please check user Name");
                }


                var userEmployee = new Employee
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(userEmployee, model.Password);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException("User creation failed! Please check user details and try again." +
                        $"  Identity Errors: Enter correct password");
                }
                return ResponseFactory<string>.CreateSuccessResponse(userEmployee.Id);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseFactory<string>.CreateUnauthorizedResponse(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ResponseFactory<string>.CreateUnauthorizedResponse(ex);
            }
            catch (ArgumentException ex)
            {
                return ResponseFactory<string>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<string>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<string>> RegisterAdmin(RegisterModel model)
        {
            try
            {
                ObjectValidator<RegisterModel>.CheckIsNotNullObject(model);
                var userExists = await _userManager.FindByNameAsync(model.UserName);
                if (userExists != null)
                {
                    throw new UnauthorizedAccessException("User already exists!");
                }

                var userEmployee = new Employee
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName
                };

                var result = await _userManager.CreateAsync(userEmployee, model.Password);
                if (!result.Succeeded)
                {
                    throw new UnauthorizedAccessException("User creation failed! Please check user details and try again.");
                }

                if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                }
                if (!await _roleManager.RoleExistsAsync(Roles.Employee.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
                }

                if (await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                {
                    await _userManager.AddToRoleAsync(userEmployee, Roles.Admin.ToString());
                }
                if (await _roleManager.RoleExistsAsync(Roles.Employee.ToString()))
                {
                    await _userManager.AddToRoleAsync(userEmployee, Roles.Employee.ToString());
                }

                return ResponseFactory<string>.CreateSuccessResponse(userEmployee.Id);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseFactory<string>.CreateUnauthorizedResponse(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ResponseFactory<string>.CreateUnauthorizedResponse(ex);
            }
            catch (ArgumentException ex)
            {
                return ResponseFactory<string>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<string>.CreateErrorResponse(ex);
            }


        }

        public async Task<IBaseResponse<AuthResultStruct>> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                ObjectValidator<TokenModel>.CheckIsNotNullObject(tokenModel);
                if (tokenModel == null)
                {
                    throw new UnauthorizedAccessException("Invalid client request");
                }

                string accessToken = tokenModel.AccessToken;
                string refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    throw new UnauthorizedAccessException("Invalid access token or refresh token");
                }

                string username = principal.Identity.Name;

                var user = await _userManager.FindByNameAsync(username);

                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    throw new UnauthorizedAccessException("Invalid access token or refresh token");
                }

                var newAccessToken = CreateToken(principal.Claims.ToList());
                var newRefreshToken = GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                await _userManager.UpdateAsync(user);

                return ResponseFactory<AuthResultStruct>.CreateSuccessResponse(new AuthResultStruct
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    RefreshToken = newRefreshToken,
                    Expiration = newAccessToken.ValidTo
                });
            }
            catch (InvalidOperationException ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateUnauthorizedResponse(ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateUnauthorizedResponse(ex);
            }
            catch (ArgumentException ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<AuthResultStruct>.CreateErrorResponse(ex);
            }
        }
        public async Task<IBaseResponse<bool>> RevokeRefreshTokenByUsernameAsync(string username)
        {
            try
            {
                StringValidator.CheckIsNotNull(username);

                var user = await _userManager.FindByNameAsync(username);
                ObjectValidator<ApplicationUser>.CheckIsNotNullObject(user);

                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> RevokeAllRefreshTokensAsync()
        {
            try
            {
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    user.RefreshToken = null;
                    await _userManager.UpdateAsync(user);
                }
                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }


        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            ObjectValidator<List<Claim>>.CheckIsNotNullObject(authClaims);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            StringValidator.CheckIsNotNull(token);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
