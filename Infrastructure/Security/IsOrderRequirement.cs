using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Infrastructure.Security;

public class IsOrderRequirement : IAuthorizationRequirement
{    
}

public class IsOrderRequirementHandler : AuthorizationHandler<IsOrderRequirement>
{
    private readonly DataContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IsOrderRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOrderRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return Task.CompletedTask;

        var orderId = _httpContextAccessor.HttpContext?.Request.RouteValues
            .SingleOrDefault(x => x.Key == "orderId").Value?.ToString();

        var orderOwner = _dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AppUserId == userId && x.OrderId == orderId)
            .Result;

        if (orderOwner == null) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
