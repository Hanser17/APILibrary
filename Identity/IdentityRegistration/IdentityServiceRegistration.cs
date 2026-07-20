using Application.Interfaces.IdentityInterfaces;
using Domain.JWTSettings;
using Identity.DBContext;
using Identity.Entities;
using Identity.InterfacesImplementation;
using Identity.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
namespace Identity.IdentityRegistration
{
    public static class IdentityServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts

            services.AddDbContext<IdentityContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            });

            #endregion

            #region Identity

            services.AddIdentityCore<User>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromSeconds(300);
            });

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;

            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(24);
                opt.LoginPath = "/User";
                opt.AccessDeniedPath = "/User/AccessDenied";
            });
            #endregion

            services.AddScoped<IIdentityService, IdentityService>();

            services.Configure<JWT_Secrets>(
             configuration.GetSection("JWT_Secrets"));

            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromSeconds(300);
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = false;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWT_Secrets:Issuer"],
                    ValidAudience = configuration["JWT_Secrets:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT_Secrets:Key"]))
                };

                opt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";

                        string errorMessage = c.Exception is SecurityTokenExpiredException
                            ? "El token ha expirado"
                            : "Token inválido o error de autenticación";

                        var result = JsonConvert.SerializeObject(new string($"{errorMessage}"));
                        return c.Response.WriteAsync(result);
                    },

                    OnChallenge = c =>
                    {
                        if (!c.Response.HasStarted)
                        {
                            c.HandleResponse();
                            c.Response.StatusCode = 401;
                            c.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new string("No esta autorizado"));
                            return c.Response.WriteAsync(result);
                        }
                        return Task.CompletedTask;
                    },

                    OnForbidden = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new string("No esta autorizado para este recurso"));
                        return c.Response.WriteAsync(result);
                    }
                };
            });



        }

        public static async Task RunSeeds(this IServiceProvider service)
        {
            using (var scope = service.CreateScope())
            {
                var servicesprovider = scope.ServiceProvider;
                try
                {
                    var userManager = servicesprovider.GetRequiredService<UserManager<User>>();
                    var roleManager = servicesprovider.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(roleManager);
                    await DefaultBasicUser.SeedAsync(userManager);
                    await DefaultAdminUser.SeedAsync(userManager);
                    await DefaultMCPUser.SeedAsync(userManager);

                }
                catch (Exception ex)
                {

                }
            }
        }
    }

}
