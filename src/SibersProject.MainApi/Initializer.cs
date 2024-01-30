using SibersProject.MainApi.Logs;
using SibersProject.DataAL.Repository.Implemintations;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.DataAL.SqlServer;
using SibersProject.MainDomain.Models.Abstractions.BaseUsers;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Implementations;
using SibersProject.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SibersProject.API
{
    public static class Initializer
    {
        public static IServiceCollection InitializeRepositories(this IServiceCollection services)
        {
            #region Base_Repositories 
            services.AddScoped(typeof(IBaseAsyncRepository<>), typeof(BaseAsyncRepository<>));
            services.AddScoped(typeof(UserManager<>));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            #endregion
            return services;
        }

        public static IServiceCollection InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthManager<Employee>, AuthManager<Employee>>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskItemService, TaskItemService>();

            return services;

        }

        public static IServiceCollection InitializeIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<RoleManager<IdentityRole>>();

            services.AddScoped<IAuthManager<Employee>>(provider =>
            {
                var userManager = provider.GetRequiredService<UserManager<Employee>>();
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
                return new AuthManager<Employee>(userManager, roleManager, configuration);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppDbContext>()
                 .AddDefaultTokenProviders();

            services.AddScoped<IUserStore<Employee>, UserStore<Employee, IdentityRole, AppDbContext, string>>();
            services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();

            return services;
        }

        public static async Task InitializeRoles(this IServiceCollection services)
        {
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();
            await SeedRoles(roleManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)).Cast<Roles>())
            {
                var roleName = role.ToString();

                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async static Task SeedAdmins(this IServiceCollection services)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<Employee>>();
            const string adminName = "mainadministrator";

            var user = await userManager.FindByIdAsync("ExampleGUID");

            if (user != null) return;

            var admin = new Employee()
            {
                Id = "ExampleGUID",
                FirstName = "Admin",
                LastName = "Admin",
                MiddleName = "Admin",             
                Email = "admin@admin.com",
                UserName = adminName,
                NormalizedUserName = "mainadministrator".Normalize(),
                NormalizedEmail = "admin@admin.com".Normalize(),
            };

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, "P@ssw0rd!");


            await userManager.CreateAsync(admin);
            await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
        }


        public static void IntialiseLogger(this ILoggingBuilder loggingBuilder, Action<DbLoggerOptions> configure)
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            loggingBuilder.Services.Configure(configure);
        }

    }
}
