using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Security;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
     IConfiguration config)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("WWW-Authenticate", "Pagination")
                .WithOrigins("http://localhost:3000", "http://localhost:3001", "https://localhost:3000");
            });
        });

        services.AddFluentValidationAutoValidation()
               .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<ProductValidator>();

        //services.AddValidatorsFromAssemblyContaining<ProductValidator>();
        //services.AddScoped<IValidator<ProductDto>, ProductValidator>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddProduct.Handler).Assembly));
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AddProduct.Handler>());

        services.AddAutoMapper(typeof(MappingProfiles).Assembly);

        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, UserAccessor>();

        return services;
    }
}