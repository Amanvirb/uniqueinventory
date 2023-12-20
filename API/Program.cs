using API.Extensions;
using API.Middleware;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Seeds;
using Swashbuckle.AspNetCore.Filters;
using System;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<DataContext>();

//builder.Services.AddApplicationServices(builder.Configuration);

//builder.Services.AddAuthorizationBuilder();
//builder.Services
//    .AddIdentityApiEndpoints<AppUser>()
//    .AddEntityFrameworkStores<DataContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();

}
);

//builder.Services.AddIdentityServices();

builder.Services.AddAuthorization();

//builder.Services.AddIdentityApiEndpoints<AppUser>()
//     .AddEntityFrameworkStores<DataContext>()
//       .AddSignInManager<SignInManager<AppUser>>()
//        .AddRoleManager<RoleManager<IdentityRole>>()
//        .AddUserManager<UserManager<AppUser>>();


var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();


app.UseXContentTypeOptions();
app.UseReferrerPolicy(opt => opt.NoReferrer());
app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
app.UseXfo(opt => opt.Deny());
app.UseCsp(opt => opt
    .BlockAllMixedContent()
    //.StyleSources(s=>s.Self().CustomSources("https://fonts.googlepris.com"))
    //.FontSources(s=>s.Self().CustomSources("http://fonts.gstatic.com", "data:"))
    .StyleSources(s => s.Self().UnsafeInline())
    .FontSources(s => s.Self())
    .FormActions(s => s.Self())
    .FrameAncestors(s => s.Self())
    //.ImageSources(s=>s.Self().CustomSources("blob:", "http://res.cloudinary.com"))
    .ImageSources(s => s.Self().CustomSources("data:"))
    .ScriptSources(s => s.Self().UnsafeInline())
); ;


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000");
            await next.Invoke();
        });
}

app.MapIdentityApi<AppUser>();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();

    DefaultUsers.SeedUsers1Async(services).Wait();

    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    //await DefaultRoles.SeedRolesAsync(roleManager);

    //var userManager = services.GetRequiredService<UserManager<AppUser>>();
    //await DefaultUsers.SeedUsersAsync(userManager);

    //await Seed.SeedData(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.MapIdentityApi<AppUser>();

app.Run();


