using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using System.Text;

namespace API.Extensions;
public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
    {
        var builder = services.AddIdentityCore<AppUser>(o =>
        {
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequireDigit = true;
            o.Password.RequireUppercase = true;
            o.SignIn.RequireConfirmedEmail = true;
            o.Password.RequiredLength = 8;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);

        builder.AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddDefaultTokenProviders();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("IsOrderOwner", policy =>
            {
                policy.Requirements.Add(new IsOrderRequirement());
            });

        //services.AddAuthorization(opt =>
        //{
        //    opt.AddPolicy("IsOrderOwner", policy =>
        //    {
        //        policy.Requirements.Add(new IsOrderRequirement());
        //    });
        //});

        services.AddTransient<IAuthorizationHandler, IsOrderRequirementHandler>();

        services.AddScoped<TokenService>();

        return services;
    }
}
