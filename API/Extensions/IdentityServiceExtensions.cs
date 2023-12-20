using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace API.Extensions;
public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<AppUser>();
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.Services.AddIdentityApiEndpoints<AppUser>()
        .AddEntityFrameworkStores<DataContext>()
       .AddSignInManager<SignInManager<AppUser>>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddUserManager<UserManager<AppUser>>();


        return services;
    }
}
