using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Seeds;
public static class DefaultUsers
{
    public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    {

        var users = new List<AppUser>
        {
            new()
            {
                UserName = "Amanvir",
                Email = "aman@test.com",
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString()
            },
            new()
            {
                UserName = "Rajpal",
                Email = "raj@test.com",
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString()
            },
            new()
            {
                UserName = "John",
                Email = "john@test.com",
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString()
            },
            new()
            {
                UserName = "Bob",
                Email = "bob@test.com",
                EmailConfirmed = true,
                Id = Guid.NewGuid().ToString()
            }
        };

        foreach (var user in users)
        {
            await userManager.CreateAsync(user, "Pa$$w0rd");

            //if(user.Email =="aman@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());

            //if (user.Email == "raj@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            //if (user.Email == "john@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.Employee.ToString());

            //if (user.Email == "bob@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.Customer.ToString());

        }
    }

    public static async Task SeedUsers1Async(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

        var users = new List<AppUser>
            {
                new()
                {
                    UserName = "Amanvir",
                    Email = "aman@test.com",
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    UserName = "Rajpal",
                    Email = "raj@test.com",
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    UserName = "John",
                    Email = "john@test.com",
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString()
                },
                new()
                {
                    UserName = "Bob",
                    Email = "bob@test.com",
                    EmailConfirmed = true,
                    Id = Guid.NewGuid().ToString()
                }
            };
        foreach (var user in users)
        {
            var userId = await EnsureUser(userManager, user.UserName, "Pa$$w0rd", user.Email);

            //if(user.Email =="aman@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());

            //if (user.Email == "raj@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            //if (user.Email == "john@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.Employee.ToString());

            //if (user.Email == "bob@test.com")
            //    await userManager.AddToRoleAsync(user, Roles.Customer.ToString());

        }
    }


    private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager,
                                             string userName, string userPassword, string email)
    {
        var user = await userManager.FindByNameAsync(userName);

        if (user == null)
        {
            user = new IdentityUser(userName)
            {
                EmailConfirmed = true,
                Email = email
            };
            await userManager.CreateAsync(user, userPassword);
        }

        return user.Id;
    }
}

