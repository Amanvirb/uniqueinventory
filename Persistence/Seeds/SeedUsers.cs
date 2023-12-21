using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seeds;
public class SeedUsers
{
    public static async Task SeedUserDataAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
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

            IEnumerable<string> superAdminRoles = new List<string>()
            {
                Roles.SuperAdmin.ToString(),
                Roles.SuperAdmin.ToString(),
                Roles.Employee.ToString(),
                Roles.Customer.ToString()
            };


            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");

                if (user.Email == "aman@test.com")
                    await userManager.AddToRolesAsync(user, superAdminRoles);

                if (user.Email == "raj@test.com")
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());

                if (user.Email == "john@test.com")
                    await userManager.AddToRoleAsync(user, Roles.Employee.ToString());

                if (user.Email == "bob@test.com")
                    await userManager.AddToRoleAsync(user, Roles.Customer.ToString());

            }
        }

    }
}

